using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Events;

namespace Defra.TradeImportsDataApi.Api.Services;

public class ChedReservationService(
    IDbContext dbContext,
    IChedReservationRepository chedReservationRepository,
    IResourceEventRepository resourceEventRepository,
    IResourceEventService resourceEventService
) : IChedReservationService
{
    public async Task<ChedReservationEntity?> Get(string id, CancellationToken cancellationToken) =>
        (await chedReservationRepository.GetAll([id], cancellationToken)).FirstOrDefault();

    public async Task<List<ChedReservationEntity>> GetByChedId(string chedId, CancellationToken cancellationToken) =>
        await chedReservationRepository.GetByChedIds([chedId], cancellationToken);

    public Task<ChedReservationEntity> Upsert(
        ChedReservationEntity entity,
        string? etag,
        CancellationToken cancellationToken
    )
    {
        return string.IsNullOrEmpty(etag) ? Insert(entity, cancellationToken) : Update(entity, etag, cancellationToken);
    }

    public Task Delete(string chedId, string mrn, CancellationToken cancellationToken)
    {
        return chedReservationRepository.DeleteById($"{chedId}_{mrn}", cancellationToken);
    }

    private async Task<ChedReservationEntity> Insert(ChedReservationEntity entity, CancellationToken cancellationToken)
    {
        await dbContext.StartTransaction(cancellationToken);
        var inserted = chedReservationRepository.Insert(entity);

        var resourceEvent = inserted.ToResourceEvent(ResourceEventOperations.Created);

        var resourceEventEntity = resourceEventRepository.Insert(resourceEvent);

        await dbContext.SaveChanges(cancellationToken);
        await dbContext.CommitTransaction(cancellationToken);

        await resourceEventService.Publish(resourceEventEntity, cancellationToken);
        return inserted;
    }

    private async Task<ChedReservationEntity> Update(
        ChedReservationEntity entity,
        string etag,
        CancellationToken cancellationToken
    )
    {
        await dbContext.StartTransaction(cancellationToken);

        var (_, updated) = await chedReservationRepository.Update(entity, etag, cancellationToken);

        var resourceEvent = updated.ToResourceEvent(ResourceEventOperations.Updated);

        var resourceEventEntity = resourceEventRepository.Insert(resourceEvent);

        await dbContext.SaveChanges(cancellationToken);
        await dbContext.CommitTransaction(cancellationToken);

        await resourceEventService.Publish(resourceEventEntity, cancellationToken);

        return updated;
    }
}
