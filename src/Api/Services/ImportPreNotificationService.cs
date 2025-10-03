using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Events;
using Defra.TradeImportsDataApi.Domain.Ipaffs;

namespace Defra.TradeImportsDataApi.Api.Services;

public class ImportPreNotificationService(
    IDbContext dbContext,
    IImportPreNotificationRepository importPreNotificationRepository,
    ICustomsDeclarationRepository customsDeclarationRepository,
    IResourceEventRepository resourceEventRepository,
    IResourceEventService resourceEventService
) : IImportPreNotificationService
{
    public async Task<ImportPreNotificationEntity?> GetImportPreNotification(
        string chedId,
        CancellationToken cancellationToken
    ) => await importPreNotificationRepository.Get(chedId, cancellationToken);

    public async Task<List<ImportPreNotificationEntity>> GetImportPreNotificationsByMrn(
        string mrn,
        CancellationToken cancellationToken
    )
    {
        var identifiers = await customsDeclarationRepository.GetAllImportPreNotificationIdentifiers(
            mrn,
            cancellationToken
        );

        return await importPreNotificationRepository.GetAll(identifiers.ToArray(), cancellationToken);
    }

    public async Task<ImportPreNotificationEntity> Insert(
        ImportPreNotificationEntity entity,
        CancellationToken cancellationToken
    )
    {
        await dbContext.StartTransaction(cancellationToken);

        var inserted = importPreNotificationRepository.Insert(entity);

        importPreNotificationRepository.TrackImportPreNotificationUpdate(inserted);

        var resourceEvent = inserted.ToResourceEvent(
            ResourceEventOperations.Created,
            inserted.ImportPreNotification,
            new ImportPreNotification()
        );

        var resourceEventEntity = resourceEventRepository.Insert(resourceEvent);

        await dbContext.SaveChanges(cancellationToken);
        await dbContext.CommitTransaction(cancellationToken);

        await resourceEventService.Publish(resourceEventEntity, cancellationToken);

        return inserted;
    }

    public async Task<ImportPreNotificationEntity> Update(
        ImportPreNotificationEntity entity,
        string etag,
        CancellationToken cancellationToken
    )
    {
        await dbContext.StartTransaction(cancellationToken);

        var (existing, updated) = await importPreNotificationRepository.Update(entity, etag, cancellationToken);

        importPreNotificationRepository.TrackImportPreNotificationUpdate(updated);

        var resourceEvent = updated.ToResourceEvent(
            ResourceEventOperations.Updated,
            updated.ImportPreNotification,
            existing.ImportPreNotification
        );

        var resourceEventEntity = resourceEventRepository.Insert(resourceEvent);

        await dbContext.SaveChanges(cancellationToken);
        await dbContext.CommitTransaction(cancellationToken);

        await resourceEventService.Publish(resourceEventEntity, cancellationToken);

        return updated;
    }

    public async Task<ImportPreNotificationUpdates> GetImportPreNotificationUpdates(
        ImportPreNotificationUpdateQuery query,
        CancellationToken cancellationToken
    ) => await importPreNotificationRepository.GetUpdates(query, cancellationToken);
}
