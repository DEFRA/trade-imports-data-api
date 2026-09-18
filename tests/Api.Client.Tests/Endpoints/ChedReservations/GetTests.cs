using Argon;
using Defra.TradeImportsDataApi.Domain.Traces;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Defra.TradeImportsDataApi.Api.Client.Tests.Endpoints.ChedReservations;

public class GetTests : WireMockTestBase<WireMockContext>
{
    private TradeImportsDataApiClient Subject { get; }

    private readonly VerifySettings _settings;

    public GetTests(WireMockContext context)
        : base(context)
    {
        Subject = new TradeImportsDataApiClient(context.HttpClient);

        _settings = new VerifySettings();
        _settings.DontScrubGuids();
        _settings.DontScrubDateTimes();
        _settings.AddExtraSettings(settings => settings.DefaultValueHandling = DefaultValueHandling.Include);
    }

    [Fact]
    public async Task GetChedReservation_WhenNotFound_ShouldBeNull()
    {
        var result = await Subject.GetChedReservation("unknown", "unknown", CancellationToken.None);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetChedReservation_WhenFound_ShouldNotBeNull()
    {
        const string chedId = "CHED";
        const string mrn = "MRN";
        var created = new DateTime(2025, 4, 7, 11, 0, 0, DateTimeKind.Utc);
        var updated = created.AddMinutes(15);

        WireMock
            .Given(Request.Create().WithPath($"/traces-cheds/{chedId}/reservation/{mrn}").UsingGet())
            .RespondWith(
                Response
                    .Create()
                    .WithBody(
                        JsonSerializer.Serialize(
                            new Defra.TradeImportsDataApi.Api.Endpoints.ChedReservation.ChedReservationResponse(
                                new Reservation
                                {
                                    ChedId = chedId,
                                    Mrn = mrn,
                                    Status = "Reserved",
                                    Timestamp = created,
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
                                created,
                                updated
                            )
                        )
                    )
                    .WithStatusCode(StatusCodes.Status200OK)
                    .WithHeader("ETag", "\"etag\"")
            );

        var result = await Subject.GetChedReservation(chedId, mrn, CancellationToken.None);

        result.Should().NotBeNull();
        await Verify(result, _settings);
    }
}
