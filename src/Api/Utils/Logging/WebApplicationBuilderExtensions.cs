using System.Diagnostics.CodeAnalysis;
using Elastic.Serilog.Enrichers.Web;
using Microsoft.AspNetCore.HeaderPropagation;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;

namespace Defra.TradeImportsDataApi.Api.Utils.Logging;

[ExcludeFromCodeCoverage]
public static class WebApplicationBuilderExtensions
{
    public static void ConfigureLoggingAndTracing(this WebApplicationBuilder builder, bool integrationTest = false)
    {
        builder.Services.AddHttpContextAccessor();
        builder
            .Services.AddOptions<TraceHeader>()
            .Bind(builder.Configuration)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        // Replaces use of AddHeaderPropagation so we can configure outside startup
        // and use the TraceHeader options configured above that will have been validated
        builder.Services.AddSingleton<IConfigureOptions<HeaderPropagationOptions>>(sp =>
        {
            var traceHeader = sp.GetRequiredService<IOptions<TraceHeader>>().Value;
            return new ConfigureOptions<HeaderPropagationOptions>(options =>
            {
                if (!string.IsNullOrWhiteSpace(traceHeader.Name))
                    options.Headers.Add(traceHeader.Name);
            });
        });
        builder.Services.TryAddSingleton<HeaderPropagationValues>();

        if (!integrationTest)
        {
            // Configuring Serilog below wipes out the framework logging
            // so we don't execute the following when the host is running
            // within an integration test
            builder.Host.UseSerilog(ConfigureLogging);
        }
    }

    private static void ConfigureLogging(
        HostBuilderContext hostBuilderContext,
        IServiceProvider services,
        LoggerConfiguration config
    )
    {
        var httpAccessor = services.GetRequiredService<IHttpContextAccessor>();
        var traceIdHeader = hostBuilderContext.Configuration.GetValue<string>("TraceHeader");
        var serviceVersion = Environment.GetEnvironmentVariable("SERVICE_VERSION") ?? "";

        config
            .ReadFrom.Configuration(hostBuilderContext.Configuration)
            .Enrich.WithEcsHttpContext(httpAccessor)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("service.version", serviceVersion)
            .Filter.ByExcluding(x =>
                x.Level == LogEventLevel.Information
                && x.Properties.TryGetValue("RequestPath", out var path)
                && path.ToString().Contains("/health")
                && !x.MessageTemplate.Text.StartsWith("Request finished")
            );

        if (traceIdHeader != null)
        {
            config.Enrich.WithCorrelationId(traceIdHeader);
        }
    }
}
