using Defra.TradeImportsDataApi.Api.Exceptions;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Data;

public class ProcessingErrorRepository(IDbContext dbContext) : IProcessingErrorRepository
{
    public async Task<ProcessingErrorEntity?> Get(string id, CancellationToken cancellationToken) =>
        await dbContext.ProcessingErrors.Find(id, cancellationToken);

    public async Task<ProcessingErrorEntity> Insert(ProcessingErrorEntity entity, CancellationToken cancellationToken)
    {
        await dbContext.ProcessingErrors.Insert(entity, cancellationToken);

        return entity;
    }

    public async Task<(ProcessingErrorEntity Existing, ProcessingErrorEntity Updated)> Update(
        ProcessingErrorEntity entity,
        string etag,
        CancellationToken cancellationToken
    )
    {
        var existing = await dbContext.ProcessingErrors.Find(entity.Id, cancellationToken);
        if (existing == null)
        {
            throw new EntityNotFoundException(nameof(ProcessingErrorEntity), entity.Id);
        }

        entity.Created = existing.Created;

        await dbContext.ProcessingErrors.Update(entity, etag, cancellationToken);

        return (existing, entity);
    }
}
