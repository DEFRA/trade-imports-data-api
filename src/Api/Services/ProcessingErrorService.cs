using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Events;

namespace Defra.TradeImportsDataApi.Api.Services;

public class ProcessingErrorService(
    IDbContext dbContext,
    IProcessingErrorRepository processingErrorRepository,
    IResourceEventRepository resourceEventRepository,
    IResourceEventService resourceEventService
) : IProcessingErrorService
{
    public async Task<ProcessingErrorEntity?> GetProcessingError(string mrn, CancellationToken cancellationToken) =>
        await processingErrorRepository.Get(mrn, cancellationToken);

    public async Task<ProcessingErrorEntity> Insert(ProcessingErrorEntity entity, CancellationToken cancellationToken)
    {
        await dbContext.StartTransaction(cancellationToken);

        var inserted = processingErrorRepository.Insert(entity);

        var resourceEvent = inserted
            .ToResourceEvent(ResourceEventOperations.Created)
            .WithChangeSet(inserted.ProcessingErrors, []);

        var resourceEventEntity = resourceEventRepository.Insert(resourceEvent);

        await dbContext.SaveChanges(cancellationToken);
        await dbContext.CommitTransaction(cancellationToken);

        await resourceEventService.Publish(resourceEventEntity, cancellationToken);

        return inserted;
    }

    public async Task<ProcessingErrorEntity> Update(
        ProcessingErrorEntity entity,
        string etag,
        CancellationToken cancellationToken
    )
    {
        await dbContext.StartTransaction(cancellationToken);

        var (existing, updated) = await processingErrorRepository.Update(entity, etag, cancellationToken);

        var resourceEvent = updated
            .ToResourceEvent(ResourceEventOperations.Updated)
            .WithChangeSet(updated.ProcessingErrors, existing.ProcessingErrors);

        var resourceEventEntity = resourceEventRepository.Insert(resourceEvent);

        await dbContext.SaveChanges(cancellationToken);
        await dbContext.CommitTransaction(cancellationToken);

        await resourceEventService.Publish(resourceEventEntity, cancellationToken);

        return updated;
    }
}
