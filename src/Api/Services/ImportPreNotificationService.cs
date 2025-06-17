using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Events;
using Defra.TradeImportsDataApi.Domain.Ipaffs;

namespace Defra.TradeImportsDataApi.Api.Services;

public class ImportPreNotificationService(
    IDbContext dbContext,
    IResourceEventPublisher resourceEventPublisher,
    IImportPreNotificationRepository importPreNotificationRepository,
    ICustomsDeclarationRepository customsDeclarationRepository
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
        var inserted = await importPreNotificationRepository.Insert(entity, cancellationToken);

        await importPreNotificationRepository.TrackImportPreNotificationUpdate(inserted, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        await resourceEventPublisher.Publish(
            inserted
                .ToResourceEvent(ResourceEventOperations.Created)
                .WithChangeSet(inserted.ImportPreNotification, new ImportPreNotification()),
            cancellationToken
        );

        return inserted;
    }

    public async Task<ImportPreNotificationEntity> Update(
        ImportPreNotificationEntity entity,
        string etag,
        CancellationToken cancellationToken
    )
    {
        var (existing, updated) = await importPreNotificationRepository.Update(entity, etag, cancellationToken);

        await importPreNotificationRepository.TrackImportPreNotificationUpdate(updated, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        await resourceEventPublisher.Publish(
            updated
                .ToResourceEvent(ResourceEventOperations.Updated)
                .WithChangeSet(updated.ImportPreNotification, existing.ImportPreNotification),
            cancellationToken
        );

        return updated;
    }

    public async Task<ImportPreNotificationUpdates> GetImportPreNotificationUpdates(
        ImportPreNotificationUpdateQuery query,
        CancellationToken cancellationToken
    ) => await importPreNotificationRepository.GetUpdates(query, cancellationToken);
}
