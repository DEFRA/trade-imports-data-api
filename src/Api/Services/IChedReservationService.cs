using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Services;

public interface IChedReservationService
{
    Task<ChedReservationEntity?> Get(string id, CancellationToken cancellationToken);

    Task<ChedReservationEntity> Upsert(ChedReservationEntity entity, string? etag, CancellationToken cancellationToken);

    Task Delete(string chedId, string mrn, CancellationToken cancellationToken);
}
