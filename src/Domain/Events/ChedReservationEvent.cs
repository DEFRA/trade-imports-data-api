using Defra.TradeImportsDataApi.Domain.Traces;

namespace Defra.TradeImportsDataApi.Domain.Events;

public class ChedReservationEvent
{
    public required string Id { get; set; }

    public string Etag { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    public Reservation? Reservation { get; set; }
}
