using Defra.TradeImportsDataApi.Api.Exceptions;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Data;

public class ProcessingErrorRepository(IDbContext dbContext) : IProcessingErrorRepository
{
    public async Task<ProcessingErrorEntity?> Get(string id, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(id))
            return null;

        return await dbContext.ProcessingErrors.Find(id, cancellationToken);
    }

    public ProcessingErrorEntity Insert(ProcessingErrorEntity entity)
    {
        dbContext.ProcessingErrors.Insert(entity);

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

        dbContext.ProcessingErrors.Update(entity, etag);

        return (existing, entity);
    }
}
