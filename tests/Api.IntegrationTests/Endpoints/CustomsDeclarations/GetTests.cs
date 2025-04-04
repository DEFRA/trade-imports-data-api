using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using Defra.TradeImportsDataApi.Testing;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using WireMock.Server;
using Xunit.Abstractions;

namespace Defra.TradeImportsDataApi.Api.IntegrationTests.Endpoints.CustomsDeclarations;

public class GetTests : EndpointTestBase, IClassFixture<WireMockContext>
{
    private ICustomsDeclarationService MockCustomsDeclarationService { get; } =
        Substitute.For<ICustomsDeclarationService>();
    private WireMockServer WireMock { get; }
    private const string Mrn = "mrn";
    private readonly VerifySettings _settings;

    public GetTests(ApiWebApplicationFactory factory, ITestOutputHelper outputHelper, WireMockContext context)
        : base(factory, outputHelper)
    {
        WireMock = context.Server;
        WireMock.Reset();

        _settings = new VerifySettings();
        _settings.ScrubMember("traceId");
        _settings.DontScrubDateTimes();
    }

    protected override void ConfigureTestServices(IServiceCollection services)
    {
        base.ConfigureTestServices(services);

        services.AddTransient<ICustomsDeclarationService>(_ => MockCustomsDeclarationService);
    }

    [Fact]
    public async Task Get_WhenNotFound_ShouldNotBeFound()
    {
        var client = CreateClient();

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.CustomsDeclarations.Get(Mrn));

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings);
    }

    [Fact]
    public async Task Get_WhenFound_ShouldReturnContent()
    {
        var client = CreateClient();
        MockCustomsDeclarationService
            .GetCustomsDeclaration(Mrn, Arg.Any<CancellationToken>())
            .Returns(
                new CustomsDeclarationEntity
                {
                    Id = Mrn,
                    Data = new CustomsDeclaration(),
                    Created = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
                    Updated = new DateTime(2025, 4, 3, 10, 15, 0, DateTimeKind.Utc),
                }
            );

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.CustomsDeclarations.Get(Mrn));

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings);
    }
}
