using Defra.TradeImportsDataApi.Api.Utils;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Events;
using Defra.TradeImportsDataApi.Domain.Ipaffs;

namespace Defra.TradeImportsDataApi.Api.Services;

public class ImportNotificationService(IDbContext dbContext, IEventPublisher eventPublisher) : IImportNotificationService
{
    public async Task<ImportNotificationEntity?> GetImportNotification(
        string chedId,
        CancellationToken cancellationToken
    )
    {
        return await dbContext.Notifications.Find(chedId, cancellationToken);
    }

    public async Task<ImportNotificationEntity> Insert(
        ImportNotificationEntity importNotificationEntity,
        CancellationToken cancellationToken
    )
    {
        var @event = CreateEvent(importNotificationEntity.Id, ResourceEventOperations.Created,
            importNotificationEntity.Data);

        await dbContext.Notifications.Insert(importNotificationEntity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        await eventPublisher.Publish(@event, cancellationToken);

        return importNotificationEntity;
    }

    public async Task<ImportNotificationEntity> Update(
        ImportNotificationEntity importNotificationEntity,
        string etag,
        CancellationToken cancellationToken
    )
    {
        var existing = await dbContext.Notifications.Find(importNotificationEntity.Id, cancellationToken);
        var @event = CreateEvent(importNotificationEntity.Id, ResourceEventOperations.Updated,
            importNotificationEntity.Data);
        @event.ChangeSet = DiffExtensions.CreateDiff(importNotificationEntity.Data, existing?.Data);

        await dbContext.Notifications.Update(importNotificationEntity, etag, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        await eventPublisher.Publish(@event, cancellationToken);

        return importNotificationEntity;
    }

    private ResourceEvent<T> CreateEvent<T>(string id, string operation, T body)
    {
        return new ResourceEvent<T>()
        {
            EntityId = id,
            EntityType = typeof(T).Name,
            Operation = operation,
            Body = body
        };
    }
}
