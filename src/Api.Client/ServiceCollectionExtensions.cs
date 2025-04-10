using Microsoft.Extensions.DependencyInjection;

namespace Defra.TradeImportsDataApi.Api.Client;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add the Trade Imports Data API client - once registered, additional concerns such as
    /// resilience handlers, header propagation and client authentication should be added.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IHttpClientBuilder AddTradeImportsDataApiClient(this IServiceCollection services) =>
        services.AddHttpClient<ITradeImportsDataApiClient, TradeImportsDataApiClient>();
}
