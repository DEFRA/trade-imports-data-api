using System.Linq.Expressions;
using Defra.TradeImportsDataApi.Api.Exceptions;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Data.Extensions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Defra.TradeImportsDataApi.Api.Data;

public class ImportPreNotificationRepository(IDbContext dbContext, ILogger<ImportPreNotificationRepository> logger)
    : IImportPreNotificationRepository
{
    public async Task<ImportPreNotificationEntity?> Get(string id, CancellationToken cancellationToken) =>
        await dbContext.ImportPreNotifications.Find(id, cancellationToken);

    public async Task<ImportPreNotificationEntity?> GetByCustomsDeclarationIdentifier(
        string customsDeclarationIdentifier,
        CancellationToken cancellationToken
    ) =>
        (
            await dbContext
                .ImportPreNotifications.Where(x => x.CustomsDeclarationIdentifier == customsDeclarationIdentifier)
                .ToListWithFallbackAsync(cancellationToken)
        ).SingleOrDefault();

    public async Task<List<ImportPreNotificationEntity>> GetAll(
        string[] customsDeclarationIdentifiers,
        CancellationToken cancellationToken
    ) =>
        await dbContext
            .ImportPreNotifications.Where(x => customsDeclarationIdentifiers.Contains(x.CustomsDeclarationIdentifier))
            .ToListWithFallbackAsync(cancellationToken);

    public async Task<List<ImportPreNotificationEntity>> GetAll(
        Expression<Func<ImportPreNotificationEntity, bool>> predicate,
        CancellationToken cancellationToken
    ) => await dbContext.ImportPreNotifications.Where(predicate).ToListWithFallbackAsync(cancellationToken);

    public async Task<List<ImportPreNotificationUpdateEntity>> GetAllUpdates(
        string[] ids,
        CancellationToken cancellationToken
    ) => await dbContext.ImportPreNotificationUpdates.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);

    public async Task<string?> GetCustomsDeclarationIdentifier(string id, CancellationToken cancellationToken) =>
        await dbContext
            .ImportPreNotifications.Where(x => x.Id == id)
            .Select(x => x.CustomsDeclarationIdentifier)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<List<ImportPreNotificationUpdate>> GetUpdates(
        DateTime from,
        DateTime to,
        string[]? pointOfEntry = null,
        string[]? type = null,
        string[]? status = null,
        CancellationToken cancellationToken = default
    )
    {
        // See UpdatesIdx index and field order - query order matches the index field order
        // _id included in index as final projection produces an update object, which means
        // only the index is needed to provide the result from this query.

        var where = new BsonDocument
        {
            {
                "updated",
                new BsonDocument { { "$gte", from }, { "$lt", to } }
            },
        };

        if (pointOfEntry is { Length: > 0 })
            where.Add("pointOfEntry", new BsonDocument { { "$in", new BsonArray(pointOfEntry) } });

        if (type is { Length: > 0 })
            where.Add("importNotificationType", new BsonDocument { { "$in", new BsonArray(type) } });

        if (status is { Length: > 0 })
            where.Add("status", new BsonDocument { { "$in", new BsonArray(status) } });

        var pipeline = new[]
        {
            new BsonDocument("$match", where),
            new BsonDocument(
                "$project",
                new BsonDocument
                {
                    // _id is always returned
                    { "updated", "$source.updated" },
                }
            ),
        };

        var start = TimeProvider.System.GetTimestamp();
        var query = string.Join(",\n", pipeline.Select(x => x.ToString()));

        var aggregate = await dbContext.ImportPreNotificationUpdates.Collection.AggregateAsync<NotificationUpdate>(
            pipeline,
            cancellationToken: cancellationToken
        );

        var updates = (await aggregate.ToListAsync(cancellationToken)).ToDictionary(x => x.Id, x => x);

        var elapsed = TimeProvider.System.GetElapsedTime(start);
        logger.LogInformation("Updates query {Elapsed} {Query}", elapsed.TotalMilliseconds, query);

        return updates.Values.Select(x => new ImportPreNotificationUpdate(x.Id, x.Updated)).ToList();
    }

    // ReSharper disable once ClassNeverInstantiated.Local
    private sealed record NotificationUpdate(string Id, DateTime Updated);

    public async Task<ImportPreNotificationEntity> Insert(
        ImportPreNotificationEntity entity,
        CancellationToken cancellationToken
    )
    {
        await dbContext.ImportPreNotifications.Insert(entity, cancellationToken);

        return entity;
    }

    public async Task<(ImportPreNotificationEntity Existing, ImportPreNotificationEntity Updated)> Update(
        ImportPreNotificationEntity entity,
        string etag,
        CancellationToken cancellationToken
    )
    {
        var existing = await dbContext.ImportPreNotifications.Find(entity.Id, cancellationToken);
        if (existing == null)
        {
            throw new EntityNotFoundException(nameof(ImportPreNotificationEntity), entity.Id);
        }

        entity.Created = existing.Created;

        await dbContext.ImportPreNotifications.Update(entity, etag, cancellationToken);

        return (existing, entity);
    }

    public async Task TrackImportPreNotificationUpdate(
        IDataEntity source,
        string[] customsDeclarationIdentifiers,
        CancellationToken cancellationToken
    )
    {
        var notifications = await GetAll(customsDeclarationIdentifiers, cancellationToken);

        await TrackImportPreNotificationUpdate(source, notifications, cancellationToken);
    }

    public async Task TrackImportPreNotificationUpdate(
        ImportPreNotificationEntity entity,
        CancellationToken cancellationToken
    ) => await TrackImportPreNotificationUpdate(entity, [entity], cancellationToken);

    private async Task TrackImportPreNotificationUpdate(
        IDataEntity source,
        List<ImportPreNotificationEntity> notifications,
        CancellationToken cancellationToken
    )
    {
        if (notifications.Count == 0)
            return;

        var updates = await GetAllUpdates(notifications.Select(x => x.Id).ToArray(), cancellationToken);

        foreach (var importPreNotification in notifications)
        {
            var update = updates.FirstOrDefault(x => x.Id == importPreNotification.Id);
            var existed = update != null;

            update ??= new ImportPreNotificationUpdateEntity { Id = importPreNotification.Id };

            update.PointOfEntry = importPreNotification.ImportPreNotification.PartOne?.PointOfEntry;
            update.ImportNotificationType = importPreNotification.ImportPreNotification.ImportNotificationType;
            update.Status = importPreNotification.ImportPreNotification.Status;
            update.SetSource(source);

            if (existed)
                await dbContext.ImportPreNotificationUpdates.Update(update, cancellationToken);
            else
                await dbContext.ImportPreNotificationUpdates.Insert(update, cancellationToken);
        }
    }
}
