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

    public async Task<List<GmrEntity>> GetGmrByChedId(string chedId, CancellationToken cancellationToken)
    {
        var importNotification = await dbContext.ImportPreNotifications.Find(x => x.Id == chedId, cancellationToken);
        if (importNotification == null)
            return [];

        var customsDeclaration = await dbContext.CustomsDeclarations.FindMany(
            x => x.ImportPreNotificationIdentifiers.Contains(importNotification.CustomsDeclarationIdentifier),
            cancellationToken
        );

        var customsDeclarationIds = customsDeclaration.Select(c => c.Id).ToList();
        if (customsDeclarationIds.Count == 0)
            return [];

        return await dbContext.Gmrs.FindMany(
            x =>
                x.Gmr.Declarations!.Customs!.Any(c => customsDeclarationIds.Any(cId => cId == c.Id))
                || x.Gmr.Declarations!.Transits!.Any(t => customsDeclarationIds.Any(cId => cId == t.Id)),
            cancellationToken
        );
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
