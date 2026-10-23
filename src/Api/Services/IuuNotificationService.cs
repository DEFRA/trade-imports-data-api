using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Events;

namespace Defra.TradeImportsDataApi.Api.Services;

public class IuuNotificationService(
    IDbContext dbContext,
    IIuuNotificationRepository iuuNotificationRepository,
    IResourceEventRepository resourceEventRepository,
    IResourceEventService resourceEventService
) : IIuuNotificationService
{
    public async Task<IuuNotificationEntity?> Get(string iuuReference, CancellationToken cancellationToken) =>
        await iuuNotificationRepository.Get(iuuReference, cancellationToken);

    public async Task<IuuNotificationEntity> Insert(IuuNotificationEntity entity, CancellationToken cancellationToken)
    {
        await dbContext.StartTransaction(cancellationToken);
        var inserted = iuuNotificationRepository.Insert(entity);

        var resourceEvent = inserted.ToResourceEvent(ResourceEventOperations.Created);

        var resourceEventEntity = resourceEventRepository.Insert(resourceEvent);

        await dbContext.SaveChanges(cancellationToken);
        await dbContext.CommitTransaction(cancellationToken);

        await resourceEventService.Publish(resourceEventEntity, cancellationToken);

        return inserted;
    }

    public async Task<IuuNotificationEntity> Update(
        IuuNotificationEntity entity,
        string etag,
        CancellationToken cancellationToken
    )
    {
        await dbContext.StartTransaction(cancellationToken);

        var (_, updated) = await iuuNotificationRepository.Update(entity, etag, cancellationToken);

        var resourceEvent = updated.ToResourceEvent(ResourceEventOperations.Updated);

        var resourceEventEntity = resourceEventRepository.Insert(resourceEvent);

        await dbContext.SaveChanges(cancellationToken);
        await dbContext.CommitTransaction(cancellationToken);

        await resourceEventService.Publish(resourceEventEntity, cancellationToken);

        return updated;
    }
}
