using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Services;

public class TracesChedService(IDbContext dbContext, ITracesChedRepository tracesChedRepository) : ITracesChedService
{
    public async Task<TracesChedEntity?> Get(string chedId, CancellationToken cancellationToken) =>
        await tracesChedRepository.Get(chedId, cancellationToken);

    public async Task<TracesChedEntity> Insert(TracesChedEntity entity, CancellationToken cancellationToken)
    {
        await dbContext.StartTransaction(cancellationToken);
        var inserted = tracesChedRepository.Insert(entity);
        await dbContext.SaveChanges(cancellationToken);
        await dbContext.CommitTransaction(cancellationToken);
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

        await dbContext.SaveChanges(cancellationToken);
        await dbContext.CommitTransaction(cancellationToken);

        return updated;
    }
}
