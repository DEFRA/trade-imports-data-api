using System.Linq.Expressions;
using Defra.TradeImportsDataApi.Api.Exceptions;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Linq;

namespace Defra.TradeImportsDataApi.Api.Services;

public class GmrService(IDbContext dbContext) : IGmrService
{
    public Task<GmrEntity?> GetGmr(string gmrId, CancellationToken cancellationToken)
    {
        return dbContext.Gmrs.Find(gmrId, cancellationToken);
    }

    public Task<List<GmrEntity>> GetGmrByChedId(string chedId, CancellationToken cancellationToken)
    {
        var results =
            from gmr in dbContext.Gmrs
            where
                gmr.Gmr.Declarations != null
                && gmr.Gmr.Declarations.Customs != null
                && gmr.Gmr.Declarations.Customs.Any(c => c.Id == chedId)
            select gmr;

        return results.ToListAsync(cancellationToken);
    }

    public async Task<GmrEntity> Insert(GmrEntity gmrEntity, CancellationToken cancellationToken)
    {
        await dbContext.Gmrs.Insert(gmrEntity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return gmrEntity;
    }

    public async Task<GmrEntity> Update(GmrEntity gmrEntity, string etag, CancellationToken cancellationToken)
    {
        var existing = await dbContext.Gmrs.Find(gmrEntity.Id, cancellationToken);
        if (existing == null)
        {
            throw new EntityNotFoundException(nameof(GmrEntity), gmrEntity.Id);
        }

        gmrEntity.Created = existing.Created;

        await dbContext.Gmrs.Update(gmrEntity, etag, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return gmrEntity;
    }
}
