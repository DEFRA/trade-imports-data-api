using Defra.TradeImportsDataApi.Api.Tests;
using Defra.TradeImportsDataApi.Api.Tests.Endpoints;
using Xunit.Abstractions;

namespace Defra.TradeImportsDataApi.Api.IntegrationTests.Endpoints.Health;

public class GetTests(ApiWebApplicationFactory factory, ITestOutputHelper outputHelper)
    : EndpointTestBase(factory, outputHelper)
{
    [Fact]
    public async Task Get_Health_ReturnsAnonymous()
    {
        var client = CreateClient(addDefaultAuthorizationHeader: false);

        var response = await client.GetAsync("/health");

        await Verify(response);
        await Verify(await response.Content.ReadAsStringAsync())
            .UseMethodName(nameof(Get_Health_ReturnsAnonymous) + "_content");
    }
}
