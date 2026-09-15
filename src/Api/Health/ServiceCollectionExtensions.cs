using System.Diagnostics.CodeAnalysis;
using Defra.TradeImportsDataApi.Api.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Defra.TradeImportsDataApi.Api.Health;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHealth(this IServiceCollection services)
    {
        services
            .AddHealthChecks()
            .AddMongoDb(
                provider => provider.GetRequiredService<IMongoDatabase>(),
                timeout: TimeSpan.FromSeconds(10),
                tags: [WebApplicationExtensions.Extended]
            )
            .AddSns(
                "Upserts topic",
                sp => sp.GetRequiredService<IOptions<ResourceEventOptions>>().Value.TopicArn,
                tags: [WebApplicationExtensions.Extended],
                timeout: TimeSpan.FromSeconds(10)
            )
            .AddSns(
                "Traces Ched Upserts topic",
                sp => sp.GetRequiredService<IOptions<ResourceEventOptions>>().Value.TracesChedTopicArn,
                tags: [WebApplicationExtensions.Extended],
                timeout: TimeSpan.FromSeconds(10)
            )
            .AddSns(
                "Ched Reservation Upserts topic",
                sp => sp.GetRequiredService<IOptions<ResourceEventOptions>>().Value.ChedReservationTopicArn,
                tags: [WebApplicationExtensions.Extended],
                timeout: TimeSpan.FromSeconds(10)
            );

        return services;
    }
}
