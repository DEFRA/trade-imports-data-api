using System.Diagnostics.CodeAnalysis;

namespace Defra.TradeImportsDataApi.Api.Metrics;

[ExcludeFromCodeCoverage]
public static class EmfExportExtensions
{
    public static IApplicationBuilder UseEmfExporter(this IApplicationBuilder builder)
    {
        var config = builder.ApplicationServices.GetRequiredService<IConfiguration>();
        var enabled = config.GetValue("AWS_EMF_ENABLED", true);

        if (enabled)
        {
            var ns = config.GetValue<string>("AWS_EMF_NAMESPACE");
            var env = config.GetValue<string>("AWS_EMF_ENVIRONMENT") ?? string.Empty;

            if (string.IsNullOrWhiteSpace(ns) && env.Equals("Local"))
                ns = typeof(Program).Namespace;

            if (string.IsNullOrWhiteSpace(ns))
                throw new InvalidOperationException("AWS_EMF_NAMESPACE is not set but metrics are enabled");

            EmfExporter.Init(builder.ApplicationServices.GetRequiredService<ILoggerFactory>(), ns);
        }

        return builder;
    }
}
