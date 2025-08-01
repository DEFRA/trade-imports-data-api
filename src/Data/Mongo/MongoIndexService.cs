using System.Diagnostics.CodeAnalysis;
using Defra.TradeImportsDataApi.Data.Entities;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Defra.TradeImportsDataApi.Data.Mongo;

[ExcludeFromCodeCoverage]
public class MongoIndexService(IMongoDatabase database, ILogger<MongoIndexService> logger) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await CreateImportPreNotificationIndexes(cancellationToken);
        await CreateCustomsDeclarationIndexes(cancellationToken);
        await CreateGmrIndexes(cancellationToken);
        await CreateResourceEventIndexes(cancellationToken);
    }

    private async Task CreateGmrIndexes(CancellationToken cancellationToken)
    {
        await CreateIndex(
            "CustomsDeclarationIdentifierIdx",
            Builders<GmrEntity>.IndexKeys.Ascending(x => x.CustomsDeclarationIdentifiers),
            cancellationToken: cancellationToken
        );
        await CreateIndex(
            "UpdatedIdx",
            Builders<GmrEntity>.IndexKeys.Ascending(x => x.Updated),
            cancellationToken: cancellationToken
        );
    }

    private async Task CreateCustomsDeclarationIndexes(CancellationToken cancellationToken)
    {
        await CreateIndex(
            "ImportPreNotificationIdentifierIdx",
            Builders<CustomsDeclarationEntity>.IndexKeys.Ascending(x => x.ImportPreNotificationIdentifiers),
            cancellationToken: cancellationToken
        );
        await CreateIndex(
            "DeclarationUcrIdx",
            Builders<CustomsDeclarationEntity>.IndexKeys.Ascending(x => x.ClearanceRequest!.DeclarationUcr),
            cancellationToken: cancellationToken
        );
        await CreateIndex(
            "UpdatedIdx",
            Builders<CustomsDeclarationEntity>.IndexKeys.Ascending(x => x.Updated),
            cancellationToken: cancellationToken
        );
    }

    private async Task CreateImportPreNotificationIndexes(CancellationToken cancellationToken)
    {
        await CreateIndex(
            "CustomDeclarationIdentifierIdx",
            Builders<ImportPreNotificationEntity>.IndexKeys.Ascending(x => x.CustomsDeclarationIdentifier),
            cancellationToken: cancellationToken
        );
        await CreateIndex(
            "UpdatedIdx",
            Builders<ImportPreNotificationEntity>.IndexKeys.Ascending(x => x.Updated),
            cancellationToken: cancellationToken
        );
        await CreateIndex(
            "UpdatesIdx",
            Builders<ImportPreNotificationUpdateEntity>
                // Order of fields important - don't change without reason
                .IndexKeys.Ascending(x => x.Updated)
                .Ascending(x => x.PointOfEntry)
                .Ascending(x => x.ImportNotificationType)
                .Ascending(x => x.Status)
                .Ascending(x => x.Source!.Updated)
                .Ascending(x => x.ImportPreNotificationId),
            cancellationToken: cancellationToken
        );
    }

    private async Task CreateResourceEventIndexes(CancellationToken cancellationToken)
    {
        await CreateIndex(
            "ResourceIdIdx",
            Builders<ResourceEventEntity>.IndexKeys.Ascending(x => x.ResourceId),
            cancellationToken: cancellationToken
        );
        await CreateTtlIndex(
            "ExpiresAtTtlIdx",
            Builders<ResourceEventEntity>.IndexKeys.Ascending(x => x.ExpiresAt),
            cancellationToken: cancellationToken
        );
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task CreateIndex<T>(
        string name,
        IndexKeysDefinition<T> keys,
        bool unique = false,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var indexModel = new CreateIndexModel<T>(
                keys,
                new CreateIndexOptions
                {
                    Name = name,
                    Background = true,
                    Unique = unique,
                }
            );
            await database
                .GetCollection<T>(typeof(T).Name.Replace("Entity", ""))
                .Indexes.CreateOneAsync(indexModel, cancellationToken: cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(
                e,
                "Failed to Create index {Name} on {Collection}",
                name,
                typeof(T).Name.Replace("Entity", "")
            );
        }
    }

    private async Task CreateTtlIndex<T>(
        string name,
        IndexKeysDefinition<T> keys,
        TimeSpan? expireAfter = null,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var indexModel = new CreateIndexModel<T>(
                keys,
                new CreateIndexOptions
                {
                    Name = name,
                    Background = true,
                    ExpireAfter = expireAfter ?? TimeSpan.Zero,
                }
            );
            await database
                .GetCollection<T>(typeof(T).Name.Replace("Entity", ""))
                .Indexes.CreateOneAsync(indexModel, cancellationToken: cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(
                e,
                "Failed to Create TTL index {Name} on {Collection}",
                name,
                typeof(T).Name.Replace("Entity", "")
            );
        }
    }
}
