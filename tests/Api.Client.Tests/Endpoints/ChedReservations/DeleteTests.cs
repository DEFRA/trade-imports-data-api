using Defra.TradeImportsDataApi.Domain.Traces;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace Defra.TradeImportsDataApi.Api.Client.Tests.Endpoints.ChedReservations;

public class DeleteTests(WireMockContext context) : WireMockTestBase<WireMockContext>(context)
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
    public async Task DeleteChedReservation_WhenSuccessful_ShouldNotThrow()
    {
        const string chedId = "CHED";
        const string mrn = "MRN";
        WireMock
            .Given(Request.Create().WithPath($"/traces-cheds/{chedId}/reservation/{mrn}").UsingDelete())
            .RespondWith(Response.Create().WithStatusCode(StatusCodes.Status204NoContent));

        var act = async () =>
            await Subject.DeleteChedReservation(chedId, mrn, CreateReservation(chedId, mrn), CancellationToken.None);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task DeleteChedReservation_WhenBadRequest_ShouldThrow()
    {
        const string chedId = "CHED";
        const string mrn = "MRN";
        WireMock
            .Given(Request.Create().WithPath($"/traces-cheds/{chedId}/reservation/{mrn}").UsingDelete())
            .RespondWith(Response.Create().WithStatusCode(StatusCodes.Status400BadRequest));

        var act = async () =>
            await Subject.DeleteChedReservation(chedId, mrn, CreateReservation(chedId, mrn), CancellationToken.None);

        await act.Should().ThrowAsync<HttpRequestException>();
    }
}
