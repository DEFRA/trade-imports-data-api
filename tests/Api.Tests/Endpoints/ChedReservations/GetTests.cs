using System.Net;
using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Traces;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using WireMock.Server;
using Xunit.Abstractions;

namespace Defra.TradeImportsDataApi.Api.Tests.Endpoints.ChedReservations;

public class GetTests : EndpointTestBase, IClassFixture<WireMockContext>
{
    private IChedReservationRepository MockChedReservationRepository { get; } =
        Substitute.For<IChedReservationRepository>();
    private WireMockServer WireMock { get; }
    private const string ChedId = "chedId";
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
        _settings.DontScrubGuids();
        _settings.DontIgnoreEmptyCollections();
    }

    protected override void ConfigureTestServices(IServiceCollection services)
    {
        base.ConfigureTestServices(services);

        services.AddTransient<IChedReservationRepository>(_ => MockChedReservationRepository);
    }

    [Fact]
    public async Task Get_WhenNotFound_ShouldNotBeFound()
    {
        var client = CreateClient();
        MockChedReservationRepository.GetAll(Arg.Any<string[]>(), Arg.Any<CancellationToken>()).Returns([]);

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.ChedReservations.Get(ChedId, Mrn));

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings);
    }

    [Fact]
    public async Task Get_WhenFound_ShouldReturnContent()
    {
        var client = CreateClient();
        MockChedReservationRepository
            .GetAll(Arg.Is<string[]>(x => x.SequenceEqual(new[] { $"{ChedId}_{Mrn}" })), Arg.Any<CancellationToken>())
            .Returns([
                new ChedReservationEntity
                {
                    Id = $"{ChedId}_{Mrn}",
                    Reservation = new Reservation
                    {
                        ChedId = ChedId,
                        Mrn = Mrn,
                        Status = "Reserved",
                        Timestamp = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
                        Commodities =
                        [
                            new ReservationCommodity
                            {
                                GoodsItemNumber = 1,
                                CommodityCode = "123",
                                CertificateLineNumber = 1,
                                UnitOfMeasure = "KGM",
                                Quantity = 100,
                            },
                        ],
                    },
                    Created = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
                    Updated = new DateTime(2025, 4, 3, 10, 15, 0, DateTimeKind.Utc),
                    ETag = "etag",
                },
            ]);

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.ChedReservations.Get(ChedId, Mrn));

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings)
            .UseMethodName(nameof(Get_WhenFound_ShouldReturnContent));
        await Verify(response, _settings).UseMethodName($"{nameof(Get_WhenFound_ShouldReturnContent)}_response");
    }

    [Fact]
    public async Task Get_WhenUnauthorized_ShouldBeUnauthorized()
    {
        var client = CreateClient(addDefaultAuthorizationHeader: false);

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.ChedReservations.Get(ChedId, Mrn));

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Get_WhenWriteOnly_ShouldBeForbidden()
    {
        var client = CreateClient(testUser: TestUser.WriteOnly);

        var response = await client.GetAsync(TradeImportsDataApi.Testing.Endpoints.ChedReservations.Get(ChedId, Mrn));

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
