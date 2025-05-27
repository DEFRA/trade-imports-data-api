using System.Diagnostics.CodeAnalysis;
using Defra.TradeImportsDataApi.Api.Exceptions;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Events;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Defra.TradeImportsDataApi.Api.Services;

[ExcludeFromCodeCoverage] // see integration tests
public class ImportPreNotificationService(
    IDbContext dbContext,
    IResourceEventPublisher resourceEventPublisher,
    ILogger<ImportPreNotificationService> logger
) : IImportPreNotificationService
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
            where.Add(
                "importPreNotification.partOne.pointOfEntry",
                new BsonDocument { { "$in", new BsonArray(pointOfEntry) } }
            );

        if (type is { Length: > 0 })
            where.Add(
                "importPreNotification.importNotificationType",
                new BsonDocument { { "$in", new BsonArray(type) } }
            );

        if (status is { Length: > 0 })
            where.Add("importPreNotification.status", new BsonDocument { { "$in", new BsonArray(status) } });

        var pipeline = new[]
        {
            new BsonDocument("$match", where),
            new BsonDocument(
                "$project",
                new BsonDocument
                {
                    { "referenceNumber", "$_id" },
                    { "updated", "$updated" },
                    { "_id", 0 },
                }
            ),
        };

        var rawQuery = string.Join(",\n", pipeline.Select(x => x.ToString()));
        logger.LogInformation("Updates query: {RawQuery}", rawQuery);

        var aggregate = await dbContext.ImportPreNotifications.Collection.AggregateAsync<ImportPreNotificationUpdate>(
            pipeline,
            cancellationToken: cancellationToken
        );

        return await aggregate.ToListAsync(cancellationToken);
    }
}
