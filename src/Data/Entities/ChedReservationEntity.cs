using Defra.TradeImportsDataApi.Data.Configuration;
using Defra.TradeImportsDataApi.Domain.Traces;

namespace Defra.TradeImportsDataApi.Data.Entities;

[DbCollection("ChedReservation")]
public class ChedReservationEntity : IDataEntity
{
    public required string Id { get; set; }

    public string ETag { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    public required Reservation Reservation { get; set; }

    public void OnSave()
    {
        Id = $"{Reservation.ChedId}_{Reservation.Mrn}";
    }
}
