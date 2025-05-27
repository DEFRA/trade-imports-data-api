using System.Net;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using WireMock.Server;
using Xunit.Abstractions;

namespace Defra.TradeImportsDataApi.Api.Tests.Endpoints.ImportPreNotifications;

public class GetUpdatesTests : EndpointTestBase, IClassFixture<WireMockContext>
{
    private IImportPreNotificationService MockImportPreNotificationService { get; } =
        Substitute.For<IImportPreNotificationService>();
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

        services.AddTransient<IImportPreNotificationService>(_ => MockImportPreNotificationService);
    }

    [Fact]
    public async Task Get_WhenInvalidRequest_NoFromDate_ShouldBeBadRequest()
    {
        var client = CreateClient();

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.ImportPreNotifications.GetUpdates());

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings);
    }

    [Fact]
    public async Task Get_WhenInvalidRequest_NoToDate_ShouldBeBadRequest()
    {
        var client = CreateClient();

        var response = await client.GetAsync(
            TradeImportsDataApi.Testing.Endpoints.ImportPreNotifications.GetUpdates(
                EndpointQuery.New.Where(EndpointFilter.From(DateTime.UtcNow))
            )
        );

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings);
    }

    [Fact]
    public async Task Get_WhenValidRequest_ShouldReturnSingle()
    {
        var client = CreateClient();
        var from = new DateTime(2025, 5, 21, 8, 0, 0, DateTimeKind.Utc);
        var to = new DateTime(2025, 5, 21, 9, 0, 0, DateTimeKind.Utc);
        string[] pointOfEntry = ["BCP1", "BCP2"];
        string[] type = ["CVEDA", "CVEDP"];
        string[] status = ["DRAFT", "SUBMITTED"];
        MockImportPreNotificationService
            .GetImportPreNotificationUpdates(
                from,
                to,
                Arg.Is<string[]?>(x => x != null && x.SequenceEqual(pointOfEntry)),
                Arg.Is<string[]?>(x => x != null && x.SequenceEqual(type)),
                Arg.Is<string[]?>(x => x != null && x.SequenceEqual(status)),
                Arg.Any<CancellationToken>()
            )
            .Returns(
                [
                    new ImportPreNotificationUpdate(
                        "CHEDPP.GB.2024.5194492",
                        new DateTime(2025, 5, 21, 8, 51, 0, DateTimeKind.Utc)
                    ),
                ]
            );

        var response = await client.GetAsync(
            TradeImportsDataApi.Testing.Endpoints.ImportPreNotifications.GetUpdates(
                EndpointQuery
                    .New.Where(EndpointFilter.From(from))
                    .Where(EndpointFilter.To(to))
                    .Where(EndpointFilter.PointOfEntry(pointOfEntry))
                    .Where(EndpointFilter.Type(type))
                    .Where(EndpointFilter.Status(status))
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
