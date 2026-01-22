using System.Threading;
using AdaskoTheBeAsT.MongoDbMigrations.Abstractions;
using Defra.TradeImportsDataApi.Data.Entities;
using MongoDB.Driver;
using Version = AdaskoTheBeAsT.MongoDbMigrations.Abstractions.Version;

namespace Defra.TradeImportsDataApi.Data.Mongo.Migrations;

public class AddResourceEventIndexes() : BtmsMigration("Add indexes to ResourceEvent collection", new Version(1, 0, 4))
{
    public override async Task UpAsync(MigrationContext context)
    {
        var collection = context.Database.GetCollection<ResourceEventEntity>(
            typeof(ResourceEventEntity).DataEntityName()
        );

        await CreateIndex(
            collection,
            "ResourceIdIdx",
            Builders<ResourceEventEntity>.IndexKeys.Ascending(x => x.ResourceId),
            cancellationToken: context.CancellationToken
        );
        await CreateTtlIndex(
            collection,
            "ExpiresAtTtlIdx",
            Builders<ResourceEventEntity>.IndexKeys.Ascending(x => x.ExpiresAt),
            cancellationToken: context.CancellationToken
        );
    }

    public override async Task DownAsync(MigrationContext context)
    {
        var collection = context.Database.GetCollection<ResourceEventEntity>(
            typeof(ResourceEventEntity).DataEntityName()
        );
        await collection.Indexes.DropOneAsync("ResourceIdIdx", context.CancellationToken);
        await collection.Indexes.DropOneAsync("ExpiresAtTtlIdx", context.CancellationToken);
    }
}
