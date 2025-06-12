using System.Linq.Expressions;
using Defra.TradeImportsDataApi.Api.Exceptions;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Data.Extensions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Defra.TradeImportsDataApi.Api.Data;

public class ImportPreNotificationRepository(IDbContext dbContext) : IImportPreNotificationRepository
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

    private async Task<List<ImportPreNotificationUpdateEntity>> GetAllUpdates(
        string[] ids,
        CancellationToken cancellationToken
    ) => await dbContext.ImportPreNotificationUpdates.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);

    public async Task<string?> GetCustomsDeclarationIdentifier(string id, CancellationToken cancellationToken) =>
        await dbContext
            .ImportPreNotifications.Where(x => x.Id == id)
            .Select(x => x.CustomsDeclarationIdentifier)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<ImportPreNotificationUpdates> GetUpdates(
        ImportPreNotificationUpdateQuery query,
        CancellationToken cancellationToken = default
    )
    {
        if (query.Page < 1)
            throw new ArgumentOutOfRangeException(nameof(query), "Page must be greater than 0");

        if (query.PageSize < 1)
            throw new ArgumentOutOfRangeException(nameof(query), "Page size must be greater than 0");

        // See UpdatesIdx index and field order - query order matches the index field order
        // _id included in index as final projection produces an update object, which means
        // only the index is needed to provide the result from this query.

        var where = new BsonDocument
        {
            {
                "updated",
                new BsonDocument { { "$gte", query.From }, { "$lt", query.To } }
            },
        };

        if (query.PointOfEntry is { Length: > 0 })
            where.Add("pointOfEntry", new BsonDocument { { "$in", new BsonArray(query.PointOfEntry) } });

        if (query.Type is { Length: > 0 })
            where.Add("importNotificationType", new BsonDocument { { "$in", new BsonArray(query.Type) } });

        if (query.Status is { Length: > 0 })
            where.Add("status", new BsonDocument { { "$in", new BsonArray(query.Status) } });

        if (query.ExcludeStatus is { Length: > 0 })
            where.Add("status", new BsonDocument { { "$nin", new BsonArray(query.ExcludeStatus) } });

        var aggregatePipeline = new[]
        {
            new BsonDocument("$match", where),
            new BsonDocument("$sort", new BsonDocument("updated", 1)),
            new BsonDocument("$skip", (query.Page - 1) * query.PageSize),
            new BsonDocument("$limit", query.PageSize),
            new BsonDocument(
                "$project",
                new BsonDocument
                {
                    // _id is always returned
                    { "updated", "$source.updated" },
                }
            ),
        };

        var countPipeline = new[] { new BsonDocument("$match", where), new BsonDocument("$count", "total") };

        var aggregateTask = dbContext.ImportPreNotificationUpdates.Collection.AggregateAsync<NotificationUpdate>(
            aggregatePipeline,
            cancellationToken: cancellationToken
        );
        var countTask = dbContext.ImportPreNotificationUpdates.Collection.AggregateAsync<BsonDocument>(
            countPipeline,
            cancellationToken: cancellationToken
        );

        await Task.WhenAll(aggregateTask, countTask);

        var aggregate = await aggregateTask;
        var count = await countTask;

        var updates = (await aggregate.ToListAsync(cancellationToken)).ToDictionary(x => x.Id, x => x);
        var total = (await count.FirstOrDefaultAsync(cancellationToken))?.GetValue("total").AsInt32 ?? 0;

        return new ImportPreNotificationUpdates(
            updates.Values.Select(x => new ImportPreNotificationUpdate(x.Id, x.Updated)).ToList(),
            total
        );
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
