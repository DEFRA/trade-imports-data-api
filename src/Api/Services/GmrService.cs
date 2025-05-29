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

    public async Task<GmrEntity> Insert(GmrEntity gmrEntity, CancellationToken cancellationToken)
    {
        var inserted = await gmrRepository.Insert(gmrEntity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return inserted;
    }

    public async Task<GmrEntity> Update(GmrEntity gmrEntity, string etag, CancellationToken cancellationToken)
    {
        var (_, updated) = await gmrRepository.Update(gmrEntity, etag, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return updated;
    }
}
