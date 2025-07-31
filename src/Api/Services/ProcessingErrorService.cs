using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Errors;
using Defra.TradeImportsDataApi.Domain.Events;

namespace Defra.TradeImportsDataApi.Api.Services;

public class ProcessingErrorService(
    IDbContext dbContext,
    IResourceEventPublisher resourceEventPublisher,
    IProcessingErrorRepository processingErrorRepository
) : IProcessingErrorService
{
    public async Task<ProcessingErrorEntity?> GetProcessingError(string mrn, CancellationToken cancellationToken) =>
        await processingErrorRepository.Get(mrn, cancellationToken);

    // Service still needs to be updated inline with others

    public async Task<ProcessingErrorEntity> Insert(ProcessingErrorEntity entity, CancellationToken cancellationToken)
    {
        await dbContext.StartTransaction(cancellationToken);

        var inserted = processingErrorRepository.Insert(entity);

        await dbContext.SaveChanges(cancellationToken);

        await resourceEventPublisher.Publish(
            inserted.ToResourceEvent(ResourceEventOperations.Created).WithChangeSet(inserted.ProcessingErrors, []),
            cancellationToken
        );

        await dbContext.CommitTransaction(cancellationToken);

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

        await dbContext.SaveChanges(cancellationToken);

        await resourceEventPublisher.Publish(
            updated
                .ToResourceEvent(ResourceEventOperations.Updated)
                .WithChangeSet(updated.ProcessingErrors, existing.ProcessingErrors),
            cancellationToken
        );

        await dbContext.CommitTransaction(cancellationToken);

        return updated;
    }
}
