using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Data;

public interface IChedReservationRepository
{
    Task<List<ChedReservationEntity>> GetAll(string[] ids, CancellationToken cancellationToken);

    Task<List<ChedReservationEntity>> GetByChedIds(string[] chedIds, CancellationToken cancellationToken);

    ChedReservationEntity Insert(ChedReservationEntity entity);

    Task<(ChedReservationEntity Existing, ChedReservationEntity Updated)> Update(
        ChedReservationEntity entity,
        string etag,
        CancellationToken cancellationToken
    );

    Task DeleteById(string id, CancellationToken cancellationToken);
}
