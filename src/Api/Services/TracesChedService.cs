using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Events;
using Defra.TradeImportsDataApi.Domain.Ipaffs;

namespace Defra.TradeImportsDataApi.Api.Services;

public class TracesChedService(
    IDbContext dbContext,
    ITracesChedRepository tracesChedRepository,
    ICustomsDeclarationRepository customsDeclarationRepository,
    IResourceEventRepository resourceEventRepository,
    IResourceEventService resourceEventService
) : ITracesChedService
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

    public async Task<TracesChedUpdates> GetUpdates(TracesChedUpdateQuery query, CancellationToken cancellationToken) =>
        await tracesChedRepository.GetUpdates(query, cancellationToken);

    public async Task<List<TracesChedEntity>> GetByMrn(string mrn, CancellationToken cancellationToken)
    {
        var identifiers = await customsDeclarationRepository.GetAllImportPreNotificationIdentifiers(
            mrn,
            cancellationToken
        );

        identifiers = identifiers.Where(x => new ChedIdReference(x).IsValid()).ToList();

        return await tracesChedRepository.GetAll(identifiers.ToArray(), cancellationToken);
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
