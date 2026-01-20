using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Services;

public class GmrService(
    IDbContext dbContext,
    IGmrRepository gmrRepository,
    IImportPreNotificationRepository importPreNotificationRepository,
    ICustomsDeclarationRepository customsDeclarationRepository
) : IGmrService
{
    public Task<GmrEntity?> GetGmr(string id, CancellationToken cancellationToken) =>
        gmrRepository.Get(id, cancellationToken);

    public async Task<List<GmrEntity>> GetGmrByChedId(string chedId, CancellationToken cancellationToken)
    {
        var customsDeclarationIdentifier = await importPreNotificationRepository.GetCustomsDeclarationIdentifier(
            chedId,
            cancellationToken
        );
        if (customsDeclarationIdentifier == null)
            return [];

        var customsDeclarationIds = await customsDeclarationRepository.GetAllIds(
            customsDeclarationIdentifier,
            cancellationToken
        );
        if (customsDeclarationIds.Count == 0)
            return [];

        return await gmrRepository.GetAll(customsDeclarationIds.ToArray(), cancellationToken);
    }

    public async Task<GmrEntity> Insert(GmrEntity entity, CancellationToken cancellationToken)
    {
        await dbContext.StartTransaction(cancellationToken);

        var inserted = gmrRepository.Insert(entity);

        await TrackImportPreNotificationUpdate(inserted, cancellationToken);

        await dbContext.SaveChanges(cancellationToken);

        await dbContext.CommitTransaction(cancellationToken);

        return inserted;
    }

    public async Task<GmrEntity> Update(GmrEntity entity, string etag, CancellationToken cancellationToken)
    {
        await dbContext.StartTransaction(cancellationToken);

        var (_, updated) = await gmrRepository.Update(entity, etag, cancellationToken);

        await TrackImportPreNotificationUpdate(updated, cancellationToken);

        await dbContext.SaveChanges(cancellationToken);

        await dbContext.CommitTransaction(cancellationToken);

        return updated;
    }

    private async Task TrackImportPreNotificationUpdate(GmrEntity entity, CancellationToken cancellationToken)
    {
        if (entity.CustomsDeclarationIdentifiers.Count <= 0)
            return;

        var identifiers = await customsDeclarationRepository.GetAllImportPreNotificationIdentifiers(
            entity.CustomsDeclarationIdentifiers.ToArray(),
            cancellationToken
        );

        await importPreNotificationRepository.TrackImportPreNotificationUpdate(
            entity,
            identifiers.ToArray(),
            cancellationToken
        );
    }
}
