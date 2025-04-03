using Defra.TradeImportsDataApi.Testing;
using WireMock.Server;
using Xunit.Abstractions;

namespace Defra.TradeImportsDataApi.Api.IntegrationTests.Endpoints.ImportNotifications;

public class GetTests : EndpointTestBase, IClassFixture<WireMockContext>
{
    private WireMockServer WireMock { get; }
    private readonly VerifySettings _settings;

    public GetTests(ApiWebApplicationFactory factory, ITestOutputHelper outputHelper, WireMockContext context)
        : base(factory, outputHelper)
    {
        WireMock = context.Server;
        WireMock.Reset();

        _settings = new VerifySettings();
        _settings.ScrubMember("traceId");
    }

    [Fact(Skip = "Skipping for the moment to focus on the custom declarations")]
    public async Task Get_WhenNotFound_ShouldNotBeFound()
    {
        // Arrange
        var client = CreateClient();

        var response = await client.GetAsync(
            TradeImportsDataApi.Testing.Endpoints.ImportNotifications.Get("does_not_exist")
        );

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings);
    }
}
