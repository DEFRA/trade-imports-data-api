using Defra.TradeImportsDataApi.Domain.Traces;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Api.Client.Tests.Endpoints.ChedReservations;

public class DeleteTests(WireMockContext context) : WireMockTestBase<WireMockContext>(context)
{
    private TradeImportsDataApiClient Subject { get; } = new(context.HttpClient);

    // DeleteChedReservation is not yet implemented on TradeImportsDataApiClient - update this test
    // to exercise the real HTTP call once it is.
    [Fact]
    public async Task DeleteChedReservation_WhenCalled_ShouldThrowNotImplemented()
    {
        const string chedId = "CHED";
        var data = new Reservation
        {
            ChedId = chedId,
            Mrn = "MRN",
            Status = "Reserved",
            Timestamp = DateTime.UtcNow,
            Commodities = [],
        };

        var act = async () => await Subject.DeleteChedReservation(chedId, "MRN", data, CancellationToken.None);

        await act.Should().ThrowAsync<NotImplementedException>();
    }
}
