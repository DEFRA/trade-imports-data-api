using Defra.TradeImportsDataApi.Domain.Traces;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Defra.TradeImportsDataApi.Api.Client.Tests.Endpoints.ChedReservations;

public class PutTests(WireMockContext context) : WireMockTestBase<WireMockContext>(context)
{
    private TradeImportsDataApiClient Subject { get; } = new(context.HttpClient);

    private static Reservation CreateReservation(string chedId, string mrn) =>
        new()
        {
            ChedId = chedId,
            Mrn = mrn,
            Status = "Reserved",
            Timestamp = DateTime.UtcNow,
            Commodities = [],
        };

    [Fact]
    public async Task PutChedReservation_WhenNoEtag_ShouldNotThrow()
    {
        const string chedId = "CHED";
        const string mrn = "MRN";
        var data = CreateReservation(chedId, mrn);
        WireMock
            .Given(
                Request
                    .Create()
                    .WithPath($"/traces-cheds/{chedId}/reservation/{mrn}")
                    .WithBody(JsonSerializer.Serialize(data))
                    .WithHeader("If-Match", "", MatchBehaviour.RejectOnMatch)
                    .UsingPut()
            )
            .RespondWith(Response.Create().WithStatusCode(StatusCodes.Status200OK));

        var act = async () => await Subject.PutChedReservation(chedId, mrn, data, etag: null, CancellationToken.None);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task PutChedReservation_WhenHasEtag_ShouldNotThrow()
    {
        const string chedId = "CHED";
        const string mrn = "MRN";
        var data = CreateReservation(chedId, mrn);
        WireMock
            .Given(
                Request
                    .Create()
                    .WithPath($"/traces-cheds/{chedId}/reservation/{mrn}")
                    .WithBody(JsonSerializer.Serialize(data))
                    .WithHeader("If-Match", "\"etag\"")
                    .UsingPut()
            )
            .RespondWith(Response.Create().WithStatusCode(StatusCodes.Status200OK));

        var act = async () =>
            await Subject.PutChedReservation(chedId, mrn, data, etag: "\"etag\"", CancellationToken.None);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task PutChedReservation_WhenBadRequest_ShouldThrow()
    {
        const string chedId = "CHED";
        const string mrn = "MRN";
        var data = CreateReservation(chedId, mrn);
        WireMock
            .Given(
                Request
                    .Create()
                    .WithPath($"/traces-cheds/{chedId}/reservation/{mrn}")
                    .WithBody(JsonSerializer.Serialize(data))
                    .UsingPut()
            )
            .RespondWith(Response.Create().WithStatusCode(StatusCodes.Status400BadRequest));

        var act = async () => await Subject.PutChedReservation(chedId, mrn, data, etag: null, CancellationToken.None);

        await act.Should().ThrowAsync<HttpRequestException>();
    }
}
