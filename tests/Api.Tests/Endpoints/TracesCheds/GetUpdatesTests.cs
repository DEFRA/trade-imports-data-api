using System.Net;
using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using WireMock.Server;
using Xunit.Abstractions;

namespace Defra.TradeImportsDataApi.Api.Tests.Endpoints.TracesCheds;

public class GetUpdatesTests : EndpointTestBase, IClassFixture<WireMockContext>
{
    private ITracesChedService MockTracesChedService { get; } = Substitute.For<ITracesChedService>();
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

        services.AddTransient<ITracesChedService>(_ => MockTracesChedService);
    }

    [Fact]
    public async Task Get_WhenInvalidRequest_NoFromDate_ShouldBeBadRequest()
    {
        var client = CreateClient();

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.TracesCheds.GetUpdates());

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings);
    }

    [Fact]
    public async Task Get_WhenInvalidRequest_NoToDate_ShouldBeBadRequest()
    {
        var client = CreateClient();

        var response = await client.GetAsync(
            TradeImportsDataApi.Testing.Endpoints.TracesCheds.GetUpdates(
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
            TradeImportsDataApi.Testing.Endpoints.TracesCheds.GetUpdates(
                EndpointQuery
                    .New.Where(EndpointFilter.From(new DateTime(2025, 5, 28, 13, 55, 0, DateTimeKind.Utc)))
                    .Where(EndpointFilter.To(new DateTime(2025, 5, 28, 14, 55, 1, DateTimeKind.Utc)))
            )
        );

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings);
    }

    [Fact]
    public async Task Get_WhenInvalidRequest_ToBeforeFrom_ShouldBeBadRequest()
    {
        var client = CreateClient();

        var response = await client.GetAsync(
            TradeImportsDataApi.Testing.Endpoints.TracesCheds.GetUpdates(
                EndpointQuery
                    .New.Where(EndpointFilter.From(new DateTime(2025, 5, 28, 14, 55, 0, DateTimeKind.Utc)))
                    .Where(EndpointFilter.To(new DateTime(2025, 5, 28, 13, 55, 0, DateTimeKind.Utc)))
            )
        );

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings);
    }

    [Fact]
    public async Task Get_WhenInvalidRequest_PageLessThan1_ShouldBeBadRequest()
    {
        var client = CreateClient();

        var response = await client.GetAsync(
            TradeImportsDataApi.Testing.Endpoints.TracesCheds.GetUpdates(
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
            TradeImportsDataApi.Testing.Endpoints.TracesCheds.GetUpdates(
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
            TradeImportsDataApi.Testing.Endpoints.TracesCheds.GetUpdates(
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
    public async Task Get_WhenValidRequest_ShouldReturnSingle()
    {
        var client = CreateClient();
        var from = new DateTime(2025, 5, 21, 8, 0, 0, DateTimeKind.Utc);
        var to = new DateTime(2025, 5, 21, 9, 0, 0, DateTimeKind.Utc);
        const int page = 1;
        const int pageSize = 10;
        MockTracesChedService
            .GetUpdates(
                Arg.Is<TracesChedUpdateQuery>(query =>
                    query!.From == from && query.To == to && query.Page == page && query.PageSize == pageSize
                ),
                Arg.Any<CancellationToken>()
            )
            .Returns(
                new TracesChedUpdates(
                    [
                        new TracesChedUpdate(
                            "CHEDPP.GB.2024.5194492",
                            new DateTime(2025, 5, 21, 8, 51, 0, DateTimeKind.Utc)
                        ),
                    ],
                    Total: 1
                )
            );

        var response = await client.GetAsync(
            TradeImportsDataApi.Testing.Endpoints.TracesCheds.GetUpdates(
                EndpointQuery
                    .New.Where(EndpointFilter.From(from))
                    .Where(EndpointFilter.To(to))
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

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.TracesCheds.GetUpdates());

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Get_WhenWriteOnly_ShouldBeForbidden()
    {
        var client = CreateClient(testUser: TestUser.WriteOnly);

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.TracesCheds.GetUpdates());

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
