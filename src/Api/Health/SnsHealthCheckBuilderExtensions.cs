using System.Diagnostics.CodeAnalysis;
using Amazon.SimpleNotificationService;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Defra.TradeImportsDataApi.Api.Health;

[ExcludeFromCodeCoverage]
public static class SnsHealthCheckBuilderExtensions
{
    public static IHealthChecksBuilder AddSns(
        this IHealthChecksBuilder builder,
        string name,
        Func<IServiceProvider, string> queueNameFunc,
        IEnumerable<string>? tags = null,
        TimeSpan? timeout = null
    )
    {
        builder.Add(
            new HealthCheckRegistration(
                name,
                sp => new SnsHealthCheck(sp.GetRequiredService<IAmazonSimpleNotificationService>(), queueNameFunc(sp)),
                HealthStatus.Unhealthy,
                tags,
                timeout
            )
        );

        return builder;
    }
}
