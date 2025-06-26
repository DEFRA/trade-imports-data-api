using System.Net;
using Defra.TradeImportsDataApi.Api.Tests.Utils.InMemoryData;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Gvms;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using WireMock.Server;
using Xunit.Abstractions;

namespace Defra.TradeImportsDataApi.Api.Tests.Endpoints.Admin;

public class GetTests : EndpointTestBase, IClassFixture<WireMockContext>
{
    private WireMockServer WireMock { get; }
    private readonly VerifySettings _settings;
    private MemoryDbContext DbContext { get; } = new();

    public GetTests(ApiWebApplicationFactory factory, ITestOutputHelper outputHelper, WireMockContext context)
        : base(factory, outputHelper)
    {
        WireMock = context.Server;
        WireMock.Reset();

        _settings = new VerifySettings();
        _settings.ScrubMember("traceId");
        _settings.DontScrubDateTimes();
        _settings.DontScrubGuids();
        _settings.DontIgnoreEmptyCollections();
    }

    protected override void ConfigureTestServices(IServiceCollection services)
    {
        base.ConfigureTestServices(services);

        services.AddTransient<IDbContext>(_ => DbContext);
    }

    [Fact]
    public async Task Get_WhenUnauthorized_ShouldBeUnauthorized()
    {
        var client = CreateClient(addDefaultAuthorizationHeader: false);

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.Admin.MaxId);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Get_WhenWriteOnly_ShouldBeForbidden()
    {
        var client = CreateClient(testUser: TestUser.WriteOnly);

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.Admin.MaxId);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Get_WhenAuthorized_MaxId_ShouldBeOk()
    {
        var client = CreateClient();

        DbContext.ImportPreNotifications.AddTestData(
            new ImportPreNotificationEntity
            {
                Id = "CHED1",
                CustomsDeclarationIdentifier = "1234567",
                ImportPreNotification = new ImportPreNotification(),
            }
        );
        DbContext.ImportPreNotifications.AddTestData(
            new ImportPreNotificationEntity
            {
                Id = "CHED2",
                CustomsDeclarationIdentifier = "1234568",
                ImportPreNotification = new ImportPreNotification(),
            }
        );

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.Admin.MaxId);

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings);
    }
}
