using System.Reflection;
using Amazon.SimpleNotificationService;
using Defra.TradeImportsDataApi.Api.Authentication;
using Defra.TradeImportsDataApi.Api.Configuration;
using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Api.Endpoints.Admin;
using Defra.TradeImportsDataApi.Api.Endpoints.CustomsDeclarations;
using Defra.TradeImportsDataApi.Api.Endpoints.Gmrs;
using Defra.TradeImportsDataApi.Api.Endpoints.ImportPreNotifications;
using Defra.TradeImportsDataApi.Api.Endpoints.ProcessingErrors;
using Defra.TradeImportsDataApi.Api.Endpoints.RelatedImportDeclarations;
using Defra.TradeImportsDataApi.Api.Endpoints.ResourceEvents;
using Defra.TradeImportsDataApi.Api.Health;
using Defra.TradeImportsDataApi.Api.Metrics;
using Defra.TradeImportsDataApi.Api.OpenApi;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Api.Utils;
using Defra.TradeImportsDataApi.Api.Utils.Logging;
using Defra.TradeImportsDataApi.Data.Extensions;
using Elastic.CommonSchema.Serilog;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Http.Resilience;
using Polly;
using Serilog;

Log.Logger = new LoggerConfiguration().WriteTo.Console(new EcsTextFormatter()).CreateBootstrapLogger();

try
{
    var app = CreateWebApplication(args);
    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    await Log.CloseAndFlushAsync();
}

return;

static WebApplication CreateWebApplication(string[] args)
{
    var builder = WebApplication.CreateBuilder(args);
    var generatingOpenApiFromCli = Assembly.GetEntryAssembly()?.GetName().Name == "dotnet-swagger";

    ConfigureWebApplication(builder, args, generatingOpenApiFromCli);

    return BuildWebApplication(builder, generatingOpenApiFromCli);
}

static void ConfigureWebApplication(WebApplicationBuilder builder, string[] args, bool generatingOpenApiFromCli)
{
    var integrationTest = args.Contains("--integrationTest=true");
    var cdpAppSettingsOptional = generatingOpenApiFromCli || integrationTest;

    builder.Configuration.AddJsonFile(
        $"appsettings.cdp.{Environment.GetEnvironmentVariable("ENVIRONMENT")?.ToLower()}.json",
        cdpAppSettingsOptional
    );
    builder.Configuration.AddEnvironmentVariables();

    // This must happen before Mongo and Http client connections
    builder.Services.AddCustomTrustStore();

    builder.ConfigureLoggingAndTracing(integrationTest);

    builder.Services.ConfigureHttpClientDefaults(options =>
    {
        var resilienceOptions = new HttpStandardResilienceOptions { Retry = { UseJitter = true } };
        resilienceOptions.Retry.DisableForUnsafeHttpMethods();

        options.ConfigureHttpClient(c =>
        {
            // Disable the HttpClient timeout to allow the resilient pipeline below
            // to handle all timeouts
            c.Timeout = Timeout.InfiniteTimeSpan;
        });

        options.AddResilienceHandler(
            "All",
            builder =>
            {
                builder
                    .AddTimeout(resilienceOptions.TotalRequestTimeout)
                    .AddRetry(resilienceOptions.Retry)
                    .AddTimeout(resilienceOptions.AttemptTimeout);
            }
        );
    });
    builder.Services.Configure<RouteHandlerOptions>(o =>
    {
        // Without this, bad request detail will only be thrown in DEVELOPMENT mode
        o.ThrowOnBadRequest = true;
    });
    builder.Services.AddProblemDetails();
    builder.Services.AddHealth();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddOpenApi(builder.Configuration);
    builder.Services.AddHttpClient();

    builder.Services.AddTransient<IRelatedImportDeclarationsService, RelatedImportDeclarationsService>();
    builder.Services.AddTransient<IGmrService, GmrService>();
    builder.Services.AddTransient<IImportPreNotificationService, ImportPreNotificationService>();
    builder.Services.AddTransient<ICustomsDeclarationService, CustomsDeclarationService>();
    builder.Services.AddTransient<IProcessingErrorService, ProcessingErrorService>();
    builder.Services.AddOptions<ResourceEventOptions>().BindConfiguration("ResourceEvents").ValidateOptions();
    builder.Services.AddSingleton<IResourceEventPublisher, ResourceEventPublisher>();
    builder.Services.AddTransient<IResourceEventService, ResourceEventService>();
    builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
    builder.Services.AddAWSService<IAmazonSimpleNotificationService>();

    builder.Services.AddDbContext(builder.Configuration, integrationTest);
    builder.Services.AddTransient<IImportPreNotificationRepository, ImportPreNotificationRepository>();
    builder.Services.AddTransient<ICustomsDeclarationRepository, CustomsDeclarationRepository>();
    builder.Services.AddTransient<IGmrRepository, GmrRepository>();
    builder.Services.AddTransient<IProcessingErrorRepository, ProcessingErrorRepository>();
    builder.Services.AddTransient<IResourceEventRepository, ResourceEventRepository>();

    builder.Services.AddAuthenticationAuthorization();

    builder.Services.AddTransient<MetricsMiddleware>();
    builder.Services.AddSingleton<RequestMetrics>();
}

static WebApplication BuildWebApplication(WebApplicationBuilder builder, bool generatingOpenApiFromCli)
{
    var app = builder.Build();

    if (!generatingOpenApiFromCli)
        app.UseEmfExporter();

    app.UseHeaderPropagation();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseMiddleware<MetricsMiddleware>();
    app.MapHealth();
    app.MapGmrEndpoints();
    app.MapImportPreNotificationEndpoints();
    app.MapCustomsDeclarationEndpoints();
    app.MapProcessingErrorEndpoints();
    app.MapSearchEndpoints();
    app.MapAdminEndpoints();
    app.MapResourceEventEndpoints();
    app.UseOpenApi();
    app.UseStatusCodePages();
    app.UseExceptionHandler(
        new ExceptionHandlerOptions
        {
            AllowStatusCode404Response = true,
            ExceptionHandler = async context =>
            {
                var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                var error = exceptionHandlerFeature?.Error;
                string? detail = null;

                if (error is BadHttpRequestException badHttpRequestException)
                {
                    context.Response.StatusCode = badHttpRequestException.StatusCode;
                    detail = badHttpRequestException.Message;
                }

                await context
                    .RequestServices.GetRequiredService<IProblemDetailsService>()
                    .WriteAsync(
                        new ProblemDetailsContext
                        {
                            HttpContext = context,
                            AdditionalMetadata = exceptionHandlerFeature?.Endpoint?.Metadata,
                            ProblemDetails = { Status = context.Response.StatusCode, Detail = detail },
                        }
                    );
            },
        }
    );

    return app;
}

#pragma warning disable S2094
namespace Defra.TradeImportsDataApi.Api
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Program;
}
#pragma warning restore S2094
