using System.Net.Http.Headers;
using Defra.TradeImportsDataApi.Api.Client;

namespace Defra.TradeImportsDataApi.Api.IntegrationTests;

[Trait("Category", "IntegrationTest")]
[Collection("Integration Tests")]
public abstract class IntegrationTestBase
{
    protected static TradeImportsDataApiClient CreateDataApiClient()
    {
        var httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:8080") };
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Basic",
            // See compose.yml for username, password and scope configuration
            Convert.ToBase64String("IntegrationTests:integration-tests-pwd"u8.ToArray())
        );

        return new TradeImportsDataApiClient(httpClient);
    }
}
