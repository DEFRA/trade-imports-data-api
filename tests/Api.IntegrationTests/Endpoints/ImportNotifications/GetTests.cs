using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using Defra.TradeImportsDataApi.Testing;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using WireMock.Server;
using Xunit.Abstractions;

namespace Defra.TradeImportsDataApi.Api.IntegrationTests.Endpoints.ImportNotifications;

public class GetTests : EndpointTestBase, IClassFixture<WireMockContext>
{
    private IImportNotificationService MockImportNotificationService { get; } =
        Substitute.For<IImportNotificationService>();
    private WireMockServer WireMock { get; }
    private const string ChedId = "chedId";
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

        services.AddTransient<IImportNotificationService>(_ => MockImportNotificationService);
    }

    [Fact]
    public async Task Get_WhenNotFound_ShouldNotBeFound()
    {
        var client = CreateClient();

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.ImportNotifications.Get(ChedId));

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings);
    }

    [Fact]
    public async Task Get_WhenFound_ShouldReturnContent()
    {
        var client = CreateClient();
        MockImportNotificationService
            .GetImportNotification(ChedId, Arg.Any<CancellationToken>())
            .Returns(
                new ImportNotificationEntity
                {
                    Id = ChedId,
                    CustomDeclarationIdentifier = ChedId,
                    Data = new ImportNotification(),
                    Created = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
                    Updated = new DateTime(2025, 4, 3, 10, 15, 0, DateTimeKind.Utc),
                }
            );

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.ImportNotifications.Get(ChedId));

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings);
    }
}
