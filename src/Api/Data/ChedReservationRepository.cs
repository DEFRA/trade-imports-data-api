using Defra.TradeImportsDataApi.Api.Exceptions;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Data.Extensions;

namespace Defra.TradeImportsDataApi.Api.Data;

public class ChedReservationRepository(IDbContext dbContext) : IChedReservationRepository
{
    public async Task<List<ChedReservationEntity>> GetAll(string[] ids, CancellationToken cancellationToken)
    {
        if (ids.Length == 0)
            return [];

        return await dbContext
            .ChedReservations.Where(x => ids.Contains(x.Id))
            .ToListWithFallbackAsync(cancellationToken);
    }

    public async Task<List<ChedReservationEntity>> GetByChedIds(string[] chedIds, CancellationToken cancellationToken)
    {
        if (chedIds.Length == 0)
            return [];

        return await dbContext
            .ChedReservations.Where(x => chedIds.Contains(x.Reservation.ChedId))
            .ToListWithFallbackAsync(cancellationToken);
    }

    public ChedReservationEntity Insert(ChedReservationEntity entity)
    {
        dbContext.ChedReservations.Insert(entity);

        return entity;
    }

    public async Task<(ChedReservationEntity Existing, ChedReservationEntity Updated)> Update(
        ChedReservationEntity entity,
        string etag,
        CancellationToken cancellationToken
    )
    {
        var existing = await dbContext.ChedReservations.Find(entity.Id, cancellationToken);
        if (existing == null)
        {
            throw new EntityNotFoundException(nameof(ChedReservationEntity), entity.Id);
        }

        entity.Created = existing.Created;

        dbContext.ChedReservations.Update(entity, etag);

        return (existing, entity);
    }

    public Task DeleteById(string id, CancellationToken cancellationToken)
    {
        dbContext.ChedReservations.Delete(id);
        return Task.CompletedTask;
    }
}
