using Defra.TradeImportsDataApi.Api.Exceptions;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Events;
using MongoDB.Driver.Linq;

namespace Defra.TradeImportsDataApi.Api.Services;

public class ImportPreNotificationService(IDbContext dbContext, IResourceEventPublisher resourceEventPublisher)
    : IImportPreNotificationService
{
    public async Task<ImportPreNotificationEntity?> GetImportPreNotification(
        string chedId,
        CancellationToken cancellationToken
    )
    {
        return await dbContext.ImportPreNotifications.Find(chedId, cancellationToken);
    }

    public async Task<List<ImportPreNotificationEntity>> GetImportPreNotificationsByMrn(
        string mrn,
        CancellationToken cancellationToken
    )
    {
        var identifiers = await dbContext
            .CustomsDeclarations.Where(x => x.Id == mrn)
            .SelectMany(x => x.ImportPreNotificationIdentifiers)
            .ToListAsync(cancellationToken);

        return await dbContext
            .ImportPreNotifications.Where(x => identifiers.Contains(x.CustomsDeclarationIdentifier))
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<ImportPreNotificationEntity> Insert(
        ImportPreNotificationEntity importPreNotificationEntity,
        CancellationToken cancellationToken
    )
    {
        await dbContext.ImportPreNotifications.Insert(importPreNotificationEntity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        await resourceEventPublisher.Publish(
            importPreNotificationEntity.ToResourceEvent(
                ResourceEventOperations.Created,
                includeEntityAsResource: false
            ),
            cancellationToken
        );

        return importPreNotificationEntity;
    }

    public async Task<ImportPreNotificationEntity> Update(
        ImportPreNotificationEntity importPreNotificationEntity,
        string etag,
        CancellationToken cancellationToken
    )
    {
        var existing = await dbContext.ImportPreNotifications.Find(importPreNotificationEntity.Id, cancellationToken);
        if (existing == null)
        {
            throw new EntityNotFoundException(nameof(ImportPreNotificationEntity), importPreNotificationEntity.Id);
        }

        importPreNotificationEntity.Created = existing.Created;

        await dbContext.ImportPreNotifications.Update(importPreNotificationEntity, etag, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        await resourceEventPublisher.Publish(
            importPreNotificationEntity.ToResourceEvent(
                ResourceEventOperations.Updated,
                includeEntityAsResource: false
            ),
            cancellationToken
        );

        return importPreNotificationEntity;
    }

    public async Task<List<ImportPreNotificationUpdate>> GetImportPreNotificationUpdates(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken
    )
    {
        var query =
            from notification in dbContext.ImportPreNotifications
            where notification.Updated >= @from && notification.Updated < to
            select new ImportPreNotificationUpdate(notification.Id, notification.Updated);

        return await query.ToListAsync(cancellationToken);
    }
}
