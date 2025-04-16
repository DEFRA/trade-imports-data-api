using Defra.TradeImportsDataApi.Api.Exceptions;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Services;

public class GmrService(IDbContext dbContext) : IGmrService
{
    public Task<GmrEntity?> GetGmr(string gmrId, CancellationToken cancellationToken)
    {
        return dbContext.Gmrs.Find(gmrId, cancellationToken);
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
