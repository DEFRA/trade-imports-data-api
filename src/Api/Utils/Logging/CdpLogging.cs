using System.Diagnostics.CodeAnalysis;
using Elastic.Serilog.Enrichers.Web;
using Serilog;

namespace Defra.TradeImportsDataApi.Api.Utils.Logging;

[ExcludeFromCodeCoverage]
public static class CdpLogging
{
    public static void Configuration(HostBuilderContext ctx, IServiceProvider services, LoggerConfiguration config)
    {
        var httpAccessor = services.GetRequiredService<IHttpContextAccessor>();
        var traceIdHeader = ctx.Configuration.GetValue<string>("TraceHeader");
        var serviceVersion = Environment.GetEnvironmentVariable("SERVICE_VERSION") ?? "";

        config
            .ReadFrom.Configuration(ctx.Configuration)
            .Enrich.WithEcsHttpContext(httpAccessor)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("service.version", serviceVersion);

        if (traceIdHeader != null)
        {
            config.Enrich.WithCorrelationId(traceIdHeader);
        }
    }
}
