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
        await dbContext.Gmrs.Update(gmrEntity, etag, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return gmrEntity;
    }
}
