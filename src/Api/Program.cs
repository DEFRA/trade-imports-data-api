using System.Reflection;
using Defra.TradeImportsDataApi.Api.Authentication;
using Defra.TradeImportsDataApi.Api.Endpoints.CustomsDeclarations;
using Defra.TradeImportsDataApi.Api.Endpoints.Gmrs;
using Defra.TradeImportsDataApi.Api.Endpoints.ImportNotifications;
using Defra.TradeImportsDataApi.Api.Health;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Api.Utils;
using Defra.TradeImportsDataApi.Api.Utils.Logging;
using Defra.TradeImportsDataApi.Data.Extensions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Serilog;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();

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

    ConfigureWebApplication(builder, args);

    return BuildWebApplication(builder);
}

static void ConfigureWebApplication(WebApplicationBuilder builder, string[] args)
{
    var generatingOpenApiFromCli = Assembly.GetEntryAssembly()?.GetName().Name == "dotnet-swagger";
    var integrationTest = args.Contains("--integrationTest=true");
    var cdpAppSettingsOptional = generatingOpenApiFromCli || integrationTest;

    builder.Configuration.AddJsonFile(
        $"appsettings.cdp.{Environment.GetEnvironmentVariable("ENVIRONMENT")?.ToLower()}.json",
        cdpAppSettingsOptional
    );
    builder.Configuration.AddEnvironmentVariables();

    // Load certificates into Trust Store - Note must happen before Mongo and Http client connections
    builder.Services.AddCustomTrustStore();

    // Configure logging to use the CDP Platform standards.
    builder.Services.AddHttpContextAccessor();
    if (!integrationTest)
        // Configuring Serilog below wipes out the framework logging
        // so we don't execute the following when the host is running
        // within an integration test
        builder.Host.UseSerilog(CdpLogging.Configuration);

    // This adds default rate limiter, total request timeout, retries, circuit breaker and timeout per attempt
    builder.Services.ConfigureHttpClientDefaults(options => options.AddStandardResilienceHandler());
    builder.Services.AddProblemDetails();
    builder.Services.AddHealth();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.AddServer(
            new OpenApiServer
            {
                Url = "https://" + (builder.Configuration.GetValue<string>("OpenApi:Host") ?? "localhost"),
            }
        );
        c.AddSecurityDefinition(
            "Basic",
            new OpenApiSecurityScheme
            {
                Description = "RFC8725 Compliant JWT",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Scheme = "Basic",
                Type = SecuritySchemeType.Http,
            }
        );
        c.AddSecurityRequirement(
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Basic" },
                    },
                    []
                },
            }
        );
        c.IncludeXmlComments(Assembly.GetExecutingAssembly());
        c.CustomSchemaIds(x => x.FullName);
        c.UseAllOfToExtendReferenceSchemas();
        c.SwaggerDoc(
            "v1",
            new OpenApiInfo
            {
                Description = "TBC",
                Contact = new OpenApiContact
                {
                    Email = "tbc@defra.gov.uk",
                    Name = "DEFRA",
                    Url = new Uri(
#pragma warning disable S1075
                        "https://www.gov.uk/government/organisations/department-for-environment-food-rural-affairs"
#pragma warning restore S1075
                    ),
                },
                Title = "Trade Imports Data API",
                Version = "v1",
            }
        );
    });
    builder.Services.AddHttpClient();
    builder.Services.AddHeaderPropagation(options =>
    {
        var traceHeader = builder.Configuration.GetValue<string>("TraceHeader");
        if (!string.IsNullOrWhiteSpace(traceHeader))
            options.Headers.Add(traceHeader);
    });

    builder.Services.AddTransient<IGmrService, GmrService>();
    builder.Services.AddTransient<IImportNotificationService, ImportNotificationService>();
    builder.Services.AddTransient<ICustomsDeclarationService, CustomsDeclarationService>();

    builder.Services.AddDbContext(builder.Configuration);

    builder.Services.AddAuthenticationAuthorization();
}

static WebApplication BuildWebApplication(WebApplicationBuilder builder)
{
    var app = builder.Build();
    var isDevelopment = app.Environment.IsDevelopment();

    app.UseHeaderPropagation();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapHealth();
    app.MapGmrEndpoints(isDevelopment);
    app.MapImportNotificationEndpoints(isDevelopment);
    app.MapCustomsDeclarationEndpoints(isDevelopment);

    app.UseSwagger(options =>
    {
        options.RouteTemplate = "/.well-known/openapi/{documentName}/openapi.json";
    });
    app.UseReDoc(options =>
    {
        options.ConfigObject.ExpandResponses = "200";
        options.DocumentTitle = "Trade Import Data API";
        options.RoutePrefix = "redoc";
        options.SpecUrl = "/.well-known/openapi/v1/openapi.json";
    });

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
