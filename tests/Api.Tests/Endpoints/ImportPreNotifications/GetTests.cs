using System.Net;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using WireMock.Server;
using Xunit.Abstractions;

namespace Defra.TradeImportsDataApi.Api.Tests.Endpoints.ImportPreNotifications;

public class GetTests : EndpointTestBase, IClassFixture<WireMockContext>
{
    private IImportPreNotificationService MockImportPreNotificationService { get; } =
        Substitute.For<IImportPreNotificationService>();
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

        services.AddTransient<IImportPreNotificationService>(_ => MockImportPreNotificationService);
    }

    [Fact]
    public async Task Get_WhenNotFound_ShouldNotBeFound()
    {
        var client = CreateClient();

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.ImportPreNotifications.Get(ChedId));

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings);
    }

    [Fact]
    public async Task Get_WhenFound_ShouldReturnContent()
    {
        var client = CreateClient();
        MockImportPreNotificationService
            .GetImportPreNotification(ChedId, Arg.Any<CancellationToken>())
            .Returns(
                new ImportPreNotificationEntity
                {
                    Id = ChedId,
                    CustomsDeclarationIdentifier = ChedId,
                    ImportPreNotification = new ImportPreNotification(),
                    Created = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
                    Updated = new DateTime(2025, 4, 3, 10, 15, 0, DateTimeKind.Utc),
                    ETag = "etag",
                }
            );

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.ImportPreNotifications.Get(ChedId));

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings)
            .UseMethodName(nameof(Get_WhenFound_ShouldReturnContent));
        await Verify(response, _settings).UseMethodName($"{nameof(Get_WhenFound_ShouldReturnContent)}_response");
    }

    [Fact]
    public async Task Get_WhenUnauthorized_ShouldBeUnauthorized()
    {
        var client = CreateClient(addDefaultAuthorizationHeader: false);

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.ImportPreNotifications.Get(ChedId));

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Get_WhenWriteOnly_ShouldBeForbidden()
    {
        var client = CreateClient(testUser: TestUser.WriteOnly);

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.ImportPreNotifications.Get(ChedId));

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
