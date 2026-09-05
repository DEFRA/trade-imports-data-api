using Defra.TradeImportsDataApi.Api.Exceptions;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Data.Extensions;
using MongoDB.Driver;

namespace Defra.TradeImportsDataApi.Api.Data;

public class TracesChedRepository(IDbContext dbContext) : ITracesChedRepository
{
    public async Task<TracesChedEntity?> Get(string id, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(id))
            return null;

        return await dbContext.TracesCheds.Find(id, cancellationToken);
    }

    public async Task<TracesChedUpdates> GetUpdates(
        TracesChedUpdateQuery query,
        CancellationToken cancellationToken = default
    )
    {
        if (query.Page < 1)
            throw new ArgumentOutOfRangeException(nameof(query), "Page must be greater than 0");

        if (query.PageSize < 1)
            throw new ArgumentOutOfRangeException(nameof(query), "Page size must be greater than 0");

        // See UpdatesIdx index and field order - any changes should check the query plan used
        // A TRACES CHED is only ever written by its own PUT and is keyed on the CHED reference,
        // so unlike import pre-notifications there is already one document per CHED to return

        var filter = Builders<TracesChedEntity>.Filter.And(
            Builders<TracesChedEntity>.Filter.Gte(x => x.Updated, query.From),
            Builders<TracesChedEntity>.Filter.Lt(x => x.Updated, query.To)
        );

        var collection = dbContext.TracesCheds.Collection;

        var updatesTask = collection
            .Find(filter)
            // Sort to ensure same order on each query execution
            .Sort(Builders<TracesChedEntity>.Sort.Ascending(x => x.Updated).Ascending(x => x.Id))
            .Project(x => new TracesChedUpdate(x.Id, x.Updated))
            .Skip((query.Page - 1) * query.PageSize)
            .Limit(query.PageSize)
            .ToListAsync(cancellationToken);

        var countTask = collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        await Task.WhenAll(updatesTask, countTask);

        return new TracesChedUpdates(await updatesTask, (int)await countTask);
    }

    public async Task<List<TracesChedEntity>> GetAll(string[] ids, CancellationToken cancellationToken)
    {
        if (ids.Length == 0)
            return [];

        return await dbContext.TracesCheds.Where(x => ids.Contains(x.Id)).ToListWithFallbackAsync(cancellationToken);
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
