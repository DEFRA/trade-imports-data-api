using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Services;

public class ImportNotificationService(IDbContext dbContext, ILogger<ImportNotificationService> logger)
    : IImportNotificationService
{
    public async Task<ImportNotificationEntity?> GetImportNotification(
        string chedId,
        CancellationToken cancellationToken
    )
    {
        logger.LogInformation("Looking for {ChedId}", chedId);
        return await dbContext.Notifications.Find(chedId, cancellationToken);
    }

    public async Task<ImportNotificationEntity> Insert(
        ImportNotificationEntity importNotificationEntity,
        CancellationToken cancellationToken
    )
    {
        await dbContext.Notifications.Insert(importNotificationEntity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return importNotificationEntity;
    }

    public async Task<ImportNotificationEntity> Update(
        ImportNotificationEntity importNotificationEntity,
        string etag,
        CancellationToken cancellationToken
    )
    {
        await dbContext.Notifications.Update(importNotificationEntity, etag, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return importNotificationEntity;
    }
}
