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

    public async Task<ProcessingErrorEntity> Insert(ProcessingErrorEntity entity, CancellationToken cancellationToken)
    {
        var inserted = await processingErrorRepository.Insert(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        await resourceEventPublisher.Publish(
            inserted.ToResourceEvent(ResourceEventOperations.Created).WithChangeSet(inserted.ProcessingErrors, []),
            cancellationToken
        );

        return inserted;
    }

    public async Task<ProcessingErrorEntity> Update(
        ProcessingErrorEntity entity,
        string etag,
        CancellationToken cancellationToken
    )
    {
        var (existing, updated) = await processingErrorRepository.Update(entity, etag, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        await resourceEventPublisher.Publish(
            updated
                .ToResourceEvent(ResourceEventOperations.Updated)
                .WithChangeSet(updated.ProcessingErrors, existing.ProcessingErrors),
            cancellationToken
        );

        return updated;
    }
}
