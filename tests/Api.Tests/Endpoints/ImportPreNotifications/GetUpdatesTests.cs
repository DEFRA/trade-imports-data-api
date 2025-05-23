using System.Net;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Api.Tests.Utils.InMemoryData;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using WireMock.Server;
using Xunit.Abstractions;

namespace Defra.TradeImportsDataApi.Api.Tests.Endpoints.ImportPreNotifications;

public class GetUpdatesTests : EndpointTestBase, IClassFixture<WireMockContext>
{
    private WireMockServer WireMock { get; }
    private readonly VerifySettings _settings;

    public GetUpdatesTests(ApiWebApplicationFactory factory, ITestOutputHelper outputHelper, WireMockContext context)
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

        services.AddTransient<IDbContext>(_ => new MemoryDbContext());
        services.AddTransient<IResourceEventPublisher>(_ => Substitute.For<IResourceEventPublisher>());
    }

    [Fact]
    public async Task Get_WhenInvalidRequest_ShouldBeBadRequest()
    {
        var client = CreateClient();

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.ImportPreNotifications.GetUpdates());

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings);
    }

    [Fact]
    public async Task Get_WhenValidRequest_ShouldReturnSingle()
    {
        var client = CreateClient();

        var response = await client.GetAsync(
            TradeImportsDataApi.Testing.Endpoints.ImportPreNotifications.GetUpdates(
                EndpointQuery
                    .New.Where(EndpointFilter.From(new DateTime(2025, 5, 21, 10, 30, 0, DateTimeKind.Utc)))
                    .Where(EndpointFilter.To(new DateTime(2025, 5, 21, 10, 40, 0, DateTimeKind.Utc)))
            )
        );

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings);
    }

    [Fact]
    public async Task Get_WhenUnauthorized_ShouldBeUnauthorized()
    {
        var client = CreateClient(addDefaultAuthorizationHeader: false);

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.ImportPreNotifications.GetUpdates());

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Get_WhenWriteOnly_ShouldBeForbidden()
    {
        var client = CreateClient(testUser: TestUser.WriteOnly);

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.ImportPreNotifications.GetUpdates());

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
