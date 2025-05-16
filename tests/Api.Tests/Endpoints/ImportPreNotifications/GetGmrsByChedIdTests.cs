using System.Net;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Gvms;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using WireMock.Server;
using Xunit.Abstractions;

namespace Defra.TradeImportsDataApi.Api.Tests.Endpoints.ImportPreNotifications;

public class GetGmrsByChedIdTests : EndpointTestBase, IClassFixture<WireMockContext>
{
    private IImportPreNotificationService MockImportPreNotificationService { get; } =
        Substitute.For<IImportPreNotificationService>();
    private IGmrService MockGmrService { get; } = Substitute.For<IGmrService>();
    private WireMockServer WireMock { get; }
    private const string ChedId = "chedId";
    private readonly VerifySettings _settings;

    public GetGmrsByChedIdTests(
        ApiWebApplicationFactory factory,
        ITestOutputHelper outputHelper,
        WireMockContext context
    )
        : base(factory, outputHelper)
    {
        WireMock = context.Server;
        WireMock.Reset();

        _settings = new VerifySettings();
        _settings.ScrubMember("traceId");
        _settings.DontScrubDateTimes();
        _settings.DontScrubGuids();
    }

    protected override void ConfigureTestServices(IServiceCollection services)
    {
        base.ConfigureTestServices(services);

        services.AddTransient<IImportPreNotificationService>(_ => MockImportPreNotificationService);
        services.AddTransient<IGmrService>(_ => MockGmrService);
    }

    [Fact]
    public async Task Get_WhenNotFound_ShouldReturnAnEmptyArray()
    {
        var client = CreateClient();
        MockGmrService
            .GetGmrByChedId(ChedId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new List<GmrEntity>()));

        var response = await client.GetAsync(
            TradeImportsDataApi.Testing.Endpoints.ImportPreNotifications.GetGmrs(ChedId)
        );

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings);
    }

    [Fact]
    public async Task Get_WhenFound_ShouldReturnContent()
    {
        var client = CreateClient();
        MockGmrService
            .GetGmrByChedId(ChedId, Arg.Any<CancellationToken>())
            .Returns(
                [
                    new GmrEntity
                    {
                        Id = "123",
                        ETag = "456",
                        Created = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
                        Updated = new DateTime(2025, 4, 3, 10, 15, 0, DateTimeKind.Utc),
                        Gmr = new Gmr(),
                    },
                ]
            );

        var response = await client.GetAsync(
            TradeImportsDataApi.Testing.Endpoints.ImportPreNotifications.GetGmrs(ChedId)
        );

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings)
            .UseMethodName(nameof(Get_WhenFound_ShouldReturnContent));
        await Verify(response, _settings).UseMethodName($"{nameof(Get_WhenFound_ShouldReturnContent)}_response");
    }

    [Fact]
    public async Task Get_WhenUnauthorized_ShouldBeUnauthorized()
    {
        var client = CreateClient(addDefaultAuthorizationHeader: false);

        var response = await client.GetAsync(
            TradeImportsDataApi.Testing.Endpoints.ImportPreNotifications.GetGmrs(ChedId)
        );

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Get_WhenWriteOnly_ShouldBeForbidden()
    {
        var client = CreateClient(testUser: TestUser.WriteOnly);

        var response = await client.GetAsync(
            TradeImportsDataApi.Testing.Endpoints.ImportPreNotifications.GetGmrs(ChedId)
        );

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
