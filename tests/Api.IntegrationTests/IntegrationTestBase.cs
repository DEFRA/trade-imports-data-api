using Defra.TradeImportsDataApi.Api.Client;

namespace Defra.TradeImportsDataApi.Api.IntegrationTests;

[Trait("Category", "IntegrationTest")]
public abstract class IntegrationTestBase
{
    protected static TradeImportsDataApiClient CreateDataApiClient() =>
        new(new HttpClient { BaseAddress = new Uri("http://localhost:8080") });
}
