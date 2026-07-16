using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Events;

namespace Defra.TradeImportsDataApi.Api.Services;

public class TracesChedService(IDbContext dbContext, ITracesChedRepository tracesChedRepository,
IResourceEventRepository resourceEventRepository,
    IResourceEventService resourceEventService) : ITracesChedService
{
    public async Task<TracesChedEntity?> Get(string chedId, CancellationToken cancellationToken) =>
        await tracesChedRepository.Get(chedId, cancellationToken);

    public async Task<TracesChedEntity> Insert(TracesChedEntity entity, CancellationToken cancellationToken)
    {
        await dbContext.StartTransaction(cancellationToken);
        var inserted = tracesChedRepository.Insert(entity);

        var resourceEvent = inserted.ToResourceEvent(ResourceEventOperations.Created);

        var resourceEventEntity = resourceEventRepository.Insert(resourceEvent);

        await dbContext.SaveChanges(cancellationToken);
        await dbContext.CommitTransaction(cancellationToken);

        await resourceEventService.Publish(resourceEventEntity, cancellationToken);
        return inserted;
    }

    public async Task<TracesChedEntity> Update(
        TracesChedEntity entity,
        string etag,
        CancellationToken cancellationToken
    )
    {
        await dbContext.StartTransaction(cancellationToken);

        var (_, updated) = await tracesChedRepository.Update(entity, etag, cancellationToken);

        var resourceEvent = updated.ToResourceEvent(ResourceEventOperations.Updated);

        var resourceEventEntity = resourceEventRepository.Insert(resourceEvent);

        await dbContext.SaveChanges(cancellationToken);
        await dbContext.CommitTransaction(cancellationToken);

        await resourceEventService.Publish(resourceEventEntity, cancellationToken);

        return updated;
    }
}
