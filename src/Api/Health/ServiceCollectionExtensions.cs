using MongoDB.Driver;

namespace Defra.TradeImportsDataApi.Api.Health;

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
            );

        return services;
    }
}
