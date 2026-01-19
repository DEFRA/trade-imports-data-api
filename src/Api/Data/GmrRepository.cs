using System.Linq.Expressions;
using Defra.TradeImportsDataApi.Api.Exceptions;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Data.Extensions;
using Defra.TradeImportsDataApi.Data.Mongo;
using MongoDB.Driver;

namespace Defra.TradeImportsDataApi.Api.Data;

public class GmrRepository(IDbContext dbContext) : IGmrRepository
{
    public async Task<GmrEntity?> Get(string id, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(id))
            return null;

        return await dbContext.Gmrs.Find(id, cancellationToken);
    }

    public Task<GmrEntity?> Get(Expression<Func<GmrEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return dbContext.Gmrs.Where(predicate).FirstOrDefaultWithFallbackAsync(cancellationToken);
    }

    public async Task<GmrEntity?> GetCaseInsensitive(
        Expression<Func<GmrEntity, bool>> predicate,
        CancellationToken cancellationToken
    )
    {
        if (dbContext is MongoDbContext)
        {
            return await dbContext
                .Gmrs.Collection.Aggregate(
                    new AggregateOptions() { Collation = new Collation("en", strength: CollationStrength.Secondary) }
                )
                .Match(predicate)
                .FirstOrDefaultAsync(cancellationToken);
        }

        return await dbContext.Gmrs.Where(predicate).FirstOrDefaultWithFallbackAsync(cancellationToken);
    }

    public async Task<List<GmrEntity>> GetAll(string[] customsDeclarationIds, CancellationToken cancellationToken)
    {
        if (customsDeclarationIds.Length == 0)
            return [];

        return await dbContext.Gmrs.FindMany(
            x => x.CustomsDeclarationIdentifiers.Any(id => customsDeclarationIds.Any(cId => cId == id)),
            cancellationToken
        );
    }

    public GmrEntity Insert(GmrEntity entity)
    {
        dbContext.Gmrs.Insert(entity);

        return entity;
    }

    public async Task<(GmrEntity Existing, GmrEntity Updated)> Update(
        GmrEntity entity,
        string etag,
        CancellationToken cancellationToken
    )
    {
        var existing = await dbContext.Gmrs.Find(entity.Id, cancellationToken);
        if (existing == null)
        {
            throw new EntityNotFoundException(nameof(GmrEntity), entity.Id);
        }

        entity.Created = existing.Created;

        dbContext.Gmrs.Update(entity, etag);

        return (existing, entity);
    }
}
