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
    public async Task<ImportPreNotificationEntity?> Get(string id, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(id))
            return null;

        return await dbContext.ImportPreNotifications.Find(id, cancellationToken);
    }

    public async Task<ImportPreNotificationEntity?> GetByCustomsDeclarationIdentifier(
        string customsDeclarationIdentifier,
        CancellationToken cancellationToken
    )
    {
        if (string.IsNullOrWhiteSpace(customsDeclarationIdentifier))
            return null;

        return (
            await dbContext
                .ImportPreNotifications.Where(x => x.CustomsDeclarationIdentifier == customsDeclarationIdentifier)
                .ToListWithFallbackAsync(cancellationToken)
        ).SingleOrDefault();
    }

    public async Task<List<ImportPreNotificationEntity>> GetAll(
        string[] customsDeclarationIdentifiers,
        CancellationToken cancellationToken
    )
    {
        if (customsDeclarationIdentifiers.Length == 0)
            return [];

        return await dbContext
            .ImportPreNotifications.Where(x => customsDeclarationIdentifiers.Contains(x.CustomsDeclarationIdentifier))
            .ToListWithFallbackAsync(cancellationToken);
    }

    public async Task<List<ImportPreNotificationEntity>> GetAll(
        Expression<Func<ImportPreNotificationEntity, bool>> predicate,
        CancellationToken cancellationToken
    ) => await dbContext.ImportPreNotifications.Where(predicate).ToListWithFallbackAsync(cancellationToken);

    public async Task<string?> GetCustomsDeclarationIdentifier(string id, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(id))
            return null;

        return await dbContext
            .ImportPreNotifications.Where(x => x.Id == id)
            .Select(x => x.CustomsDeclarationIdentifier)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ImportPreNotificationUpdates> GetUpdates(
        ImportPreNotificationUpdateQuery query,
        CancellationToken cancellationToken = default
    )
    {
        if (query.Page < 1)
            throw new ArgumentOutOfRangeException(nameof(query), "Page must be greater than 0");

        if (query.PageSize < 1)
            throw new ArgumentOutOfRangeException(nameof(query), "Page size must be greater than 0");

        // See UpdatesIdx index and field order - any changes should check the query plan used
        // We can expect multiple update entities for the same CHED, which is why we group
        // and then take the last updated value from the source reference

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
            // Group by importPreNotificationId, then take the last updated value
            new BsonDocument(
                "$group",
                new BsonDocument
                {
                    { "_id", "$importPreNotificationId" },
                    { "updated", new BsonDocument("$last", "$source.updated") },
                }
            ),
            // Sort to ensure same order on each query execution
            new BsonDocument("$sort", new BsonDocument { { "updated", 1 }, { "_id", 1 } }),
            new BsonDocument("$skip", (query.Page - 1) * query.PageSize),
            new BsonDocument("$limit", query.PageSize),
        };

        var countPipeline = new[]
        {
            new BsonDocument("$match", where),
            new BsonDocument("$group", new BsonDocument { { "_id", "$importPreNotificationId" } }),
            new BsonDocument("$count", "total"),
        };

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

    public ImportPreNotificationEntity Insert(ImportPreNotificationEntity entity)
    {
        dbContext.ImportPreNotifications.Insert(entity);

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

        dbContext.ImportPreNotifications.Update(entity, etag);

        return (existing, entity);
    }

    public async Task TrackImportPreNotificationUpdate(
        IDataEntity source,
        string[] customsDeclarationIdentifiers,
        CancellationToken cancellationToken
    )
    {
        var notifications = await GetAll(customsDeclarationIdentifiers, cancellationToken);

        TrackImportPreNotificationUpdate(source, notifications);
    }

    public void TrackImportPreNotificationUpdate(ImportPreNotificationEntity entity) =>
        TrackImportPreNotificationUpdate(entity, [entity]);

    public async Task<string?> GetMaxId(CancellationToken cancellationToken)
    {
        var entity = await dbContext
            .ImportPreNotifications.Collection.Find(_ => true)
            .SortByDescending(x => x.CustomsDeclarationIdentifier)
            .Limit(1)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        return entity?.Id;
    }

    private void TrackImportPreNotificationUpdate(IDataEntity source, List<ImportPreNotificationEntity> notifications)
    {
        if (notifications.Count == 0)
            return;

        foreach (
            var update in notifications.Select(importPreNotification => new ImportPreNotificationUpdateEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                ImportPreNotificationId = importPreNotification.Id,
                PointOfEntry = importPreNotification.ImportPreNotification.PartOne?.PointOfEntry,
                ImportNotificationType = importPreNotification.ImportPreNotification.ImportNotificationType,
                Status = importPreNotification.ImportPreNotification.Status,
            })
        )
        {
            update.SetSource(source);

            dbContext.ImportPreNotificationUpdates.Insert(update);
        }
    }
}
