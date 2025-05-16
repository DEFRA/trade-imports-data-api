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

    public async Task<List<GmrEntity>> GetGmrByChedId(string chedId, CancellationToken cancellationToken)
    {
        var mrnIdentifiers = await dbContext
            .ImportPreNotifications.Where(x => x.Id == chedId)
            .SelectMany(x => x.MrnIdentifiers)
            .ToListAsync(cancellationToken);

        if (mrnIdentifiers.Count == 0)
            return [];

        var gmrs = await dbContext
            .Gmrs.Where(x => mrnIdentifiers.Intersect(x.MrnIdentifiers).Any())
            .ToListAsync(cancellationToken);

        return gmrs;
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
