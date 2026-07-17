using Defra.TradeImportsDataApi.Api.Exceptions;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Data;

public class TracesChedRepository(IDbContext dbContext) : ITracesChedRepository
{
    public async Task<TracesChedEntity?> Get(string id, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(id))
            return null;

        return await dbContext.TracesCheds.Find(id, cancellationToken);
    }

    public TracesChedEntity Insert(TracesChedEntity entity)
    {
        dbContext.TracesCheds.Insert(entity);

        return entity;
    }

    public async Task<(TracesChedEntity Existing, TracesChedEntity Updated)> Update(
        TracesChedEntity entity,
        string etag,
        CancellationToken cancellationToken
    )
    {
        var existing = await dbContext.TracesCheds.Find(entity.Id, cancellationToken);
        if (existing == null)
        {
            throw new EntityNotFoundException(nameof(TracesChedEntity), entity.Id);
        }

        entity.Created = existing.Created;

        dbContext.TracesCheds.Update(entity, etag);

        return (existing, entity);
    }
}
