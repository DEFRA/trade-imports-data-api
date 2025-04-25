using Defra.TradeImportsDataApi.Api.Exceptions;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Services;

public class ProcessingErrorService(IDbContext dbContext) : IProcessingErrorService
{
    public async Task<ProcessingErrorEntity?> GetProcessingError(string mrn, CancellationToken cancellationToken)
    {
        return await dbContext.ProcessingErrors.Find(mrn, cancellationToken);
    }

    public async Task<ProcessingErrorEntity> Insert(
        ProcessingErrorEntity processingErrorEntity,
        CancellationToken cancellationToken
    )
    {
        await dbContext.ProcessingErrors.Insert(processingErrorEntity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return processingErrorEntity;
    }

    public async Task<ProcessingErrorEntity> Update(
        ProcessingErrorEntity processingErrorEntity,
        string etag,
        CancellationToken cancellationToken
    )
    {
        var existing = await dbContext.ProcessingErrors.Find(processingErrorEntity.Id, cancellationToken);
        if (existing == null)
        {
            throw new EntityNotFoundException(nameof(ProcessingErrorEntity), processingErrorEntity.Id);
        }

        processingErrorEntity.Created = existing.Created;

        await dbContext.ProcessingErrors.Update(processingErrorEntity, etag, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return processingErrorEntity;
    }
}
