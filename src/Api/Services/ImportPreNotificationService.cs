using Defra.TradeImportsDataApi.Api.Exceptions;
using Defra.TradeImportsDataApi.Api.Utils;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Events;
using MongoDB.Driver.Linq;

namespace Defra.TradeImportsDataApi.Api.Services;

public class ImportPreNotificationService(IDbContext dbContext, IEventPublisher eventPublisher)
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
        var @event = CreateEvent(
            importPreNotificationEntity.Id,
            ResourceEventOperations.Created,
            importPreNotificationEntity.ImportPreNotification
        );

        await dbContext.ImportPreNotifications.Insert(importPreNotificationEntity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        await eventPublisher.Publish(@event, @event.ResourceType, cancellationToken);

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

        var @event = CreateEvent(
            importPreNotificationEntity.Id,
            ResourceEventOperations.Updated,
            importPreNotificationEntity.ImportPreNotification
        );
        @event.ChangeSet = DiffExtensions.CreateDiff(
            importPreNotificationEntity.ImportPreNotification,
            existing.ImportPreNotification
        );

        await dbContext.ImportPreNotifications.Update(importPreNotificationEntity, etag, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        await eventPublisher.Publish(@event, @event.ResourceType, cancellationToken);

        return importPreNotificationEntity;
    }

    private ResourceEvent<T> CreateEvent<T>(string id, string operation, T body)
    {
        return new ResourceEvent<T>
        {
            ResourceId = id,
            ResourceType = typeof(T).Name,
            Operation = operation,
            Resource = body,
        };
    }
}
