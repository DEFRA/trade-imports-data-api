using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Services;

public class ImportPreNotificationService(IDbContext dbContext) : IImportPreNotificationService
{
    public async Task<ImportPreNotificationEntity?> GetImportPreNotification(
        string chedId,
        CancellationToken cancellationToken
    )
    {
        return await dbContext.ImportPreNotifications.Find(chedId, cancellationToken);
    }

    public async Task<ImportPreNotificationEntity> Insert(
        ImportPreNotificationEntity importPreNotificationEntity,
        CancellationToken cancellationToken
    )
    {
        await dbContext.ImportPreNotifications.Insert(importPreNotificationEntity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return importPreNotificationEntity;
    }

    public async Task<ImportPreNotificationEntity> Update(
        ImportPreNotificationEntity importPreNotificationEntity,
        string etag,
        CancellationToken cancellationToken
    )
    {
        await dbContext.ImportPreNotifications.Update(importPreNotificationEntity, etag, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return importPreNotificationEntity;
    }
}
