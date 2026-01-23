using AdaskoTheBeAsT.MongoDbMigrations.Abstractions;
using Defra.TradeImportsDataApi.Data.Entities;
using MongoDB.Driver;
using Version = AdaskoTheBeAsT.MongoDbMigrations.Abstractions.Version;

namespace Defra.TradeImportsDataApi.Data.Mongo.Migrations;

public class AddGmrIndexes() : BtmsMigration("Add indexes to gmr collection", new Version(1, 0, 2))
{
    public override async Task UpAsync(MigrationContext context)
    {
        var collection = context.Database.GetCollection<GmrEntity>(typeof(GmrEntity).DataEntityName());

        await CreateIndex(
            collection,
            "CustomsDeclarationIdentifierIdx",
            Builders<GmrEntity>.IndexKeys.Ascending(x => x.CustomsDeclarationIdentifiers),
            cancellationToken: context.CancellationToken
        );

        await CreateIndex(
            collection,
            "UpdatedIdx",
            Builders<GmrEntity>.IndexKeys.Ascending(x => x.Updated),
            cancellationToken: context.CancellationToken
        );

        await CreateIndex(
            collection,
            "TagsIdx",
            Builders<GmrEntity>.IndexKeys.Ascending(x => x.Tags),
            cancellationToken: context.CancellationToken
        );
    }

    public override async Task DownAsync(MigrationContext context)
    {
        var collection = context.Database.GetCollection<GmrEntity>(typeof(GmrEntity).DataEntityName());
        await collection.Indexes.DropOneAsync("CustomsDeclarationIdentifierIdx", context.CancellationToken);
        await collection.Indexes.DropOneAsync("UpdatedIdx", context.CancellationToken);
        await collection.Indexes.DropOneAsync("TagsIdx", context.CancellationToken);
    }
}
