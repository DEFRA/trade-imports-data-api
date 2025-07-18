using System.Net;
using Defra.TradeImportsDataApi.Api.Data;
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
    public async Task Get_WhenInvalidRequest_FromAndToGreaterThanOneHour_ShouldBeBadRequest()
    {
        var client = CreateClient();

        var response = await client.GetAsync(
            TradeImportsDataApi.Testing.Endpoints.ImportPreNotifications.GetUpdates(
                EndpointQuery
                    .New.Where(EndpointFilter.From(new DateTime(2025, 5, 28, 13, 55, 0, DateTimeKind.Utc)))
                    .Where(EndpointFilter.To(new DateTime(2025, 5, 28, 14, 55, 1, DateTimeKind.Utc)))
            )
        );

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings);
    }

    [Fact]
    public async Task Get_WhenInvalidRequest_PageLessThan1_ShouldBeBadRequest()
    {
        var client = CreateClient();

        var response = await client.GetAsync(
            TradeImportsDataApi.Testing.Endpoints.ImportPreNotifications.GetUpdates(
                EndpointQuery
                    .New.Where(EndpointFilter.From(new DateTime(2025, 5, 28, 13, 55, 0, DateTimeKind.Utc)))
                    .Where(EndpointFilter.To(new DateTime(2025, 5, 28, 14, 15, 0, DateTimeKind.Utc)))
                    .Where(EndpointFilter.Page(0))
            )
        );

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings);
    }

    [Fact]
    public async Task Get_WhenInvalidRequest_PageSizeLessThan1_ShouldBeBadRequest()
    {
        var client = CreateClient();

        var response = await client.GetAsync(
            TradeImportsDataApi.Testing.Endpoints.ImportPreNotifications.GetUpdates(
                EndpointQuery
                    .New.Where(EndpointFilter.From(new DateTime(2025, 5, 28, 13, 55, 0, DateTimeKind.Utc)))
                    .Where(EndpointFilter.To(new DateTime(2025, 5, 28, 14, 15, 0, DateTimeKind.Utc)))
                    .Where(EndpointFilter.Page(1))
                    .Where(EndpointFilter.PageSize(0))
            )
        );

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings);
    }

    [Fact]
    public async Task Get_WhenInvalidRequest_PageSizeGreaterThan1000_ShouldBeBadRequest()
    {
        var client = CreateClient();

        var response = await client.GetAsync(
            TradeImportsDataApi.Testing.Endpoints.ImportPreNotifications.GetUpdates(
                EndpointQuery
                    .New.Where(EndpointFilter.From(new DateTime(2025, 5, 28, 13, 55, 0, DateTimeKind.Utc)))
                    .Where(EndpointFilter.To(new DateTime(2025, 5, 28, 14, 15, 0, DateTimeKind.Utc)))
                    .Where(EndpointFilter.Page(1))
                    .Where(EndpointFilter.PageSize(1001))
            )
        );

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings);
    }

    [Fact]
    public async Task Get_WhenInvalidRequest_EmptyStringInArrays_ShouldCallWithNone()
    {
        var client = CreateClient();
        MockImportPreNotificationService
            .GetImportPreNotificationUpdates(
                Arg.Is<ImportPreNotificationUpdateQuery>(query =>
                    query.PointOfEntry == null
                    && query.Type == null
                    && query.Status == null
                    && query.ExcludeStatus == null
                ),
                Arg.Any<CancellationToken>()
            )
            .Returns(
                new ImportPreNotificationUpdates(
                    [
                        new ImportPreNotificationUpdate(
                            "CHEDPP.GB.2024.5194492",
                            new DateTime(2025, 5, 21, 8, 51, 0, DateTimeKind.Utc)
                        ),
                    ],
                    Total: 1
                )
            );

        await client.GetAsync(
            TradeImportsDataApi.Testing.Endpoints.ImportPreNotifications.GetUpdates(
                EndpointQuery
                    .New.Where(EndpointFilter.From(DateTime.UtcNow))
                    .Where(EndpointFilter.To(DateTime.UtcNow.AddSeconds(1)))
                    .Where(EndpointFilter.PointOfEntry(""))
                    .Where(EndpointFilter.Type(""))
                    .Where(EndpointFilter.Status(""))
                    .Where(EndpointFilter.ExcludeStatus(""))
            )
        );

        await MockImportPreNotificationService
            .Received(1)
            .GetImportPreNotificationUpdates(
                Arg.Is<ImportPreNotificationUpdateQuery>(query =>
                    query.PointOfEntry == null
                    && query.Type == null
                    && query.Status == null
                    && query.ExcludeStatus == null
                ),
                Arg.Any<CancellationToken>()
            );
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
        string[] excludeStatus = ["AMEND"];
        const int page = 1;
        const int pageSize = 10;
        MockImportPreNotificationService
            .GetImportPreNotificationUpdates(
                Arg.Is<ImportPreNotificationUpdateQuery>(query =>
                    query.From == from
                    && query.To == to
                    && query.PointOfEntry != null
                    && query.PointOfEntry.SequenceEqual(pointOfEntry)
                    && query.Type != null
                    && query.Type.SequenceEqual(type)
                    && query.Status != null
                    && query.Status.SequenceEqual(status)
                    && query.ExcludeStatus != null
                    && query.ExcludeStatus.SequenceEqual(excludeStatus)
                    && query.Page == page
                    && query.PageSize == pageSize
                ),
                Arg.Any<CancellationToken>()
            )
            .Returns(
                new ImportPreNotificationUpdates(
                    [
                        new ImportPreNotificationUpdate(
                            "CHEDPP.GB.2024.5194492",
                            new DateTime(2025, 5, 21, 8, 51, 0, DateTimeKind.Utc)
                        ),
                    ],
                    Total: 1
                )
            );

        var response = await client.GetAsync(
            TradeImportsDataApi.Testing.Endpoints.ImportPreNotifications.GetUpdates(
                EndpointQuery
                    .New.Where(EndpointFilter.From(from))
                    .Where(EndpointFilter.To(to))
                    .Where(EndpointFilter.PointOfEntry(pointOfEntry))
                    .Where(EndpointFilter.Type(type))
                    .Where(EndpointFilter.Status(status))
                    .Where(EndpointFilter.ExcludeStatus(excludeStatus))
                    .Where(EndpointFilter.Page(page))
                    .Where(EndpointFilter.PageSize(pageSize))
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
