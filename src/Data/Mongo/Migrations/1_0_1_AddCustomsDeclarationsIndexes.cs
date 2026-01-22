using AdaskoTheBeAsT.MongoDbMigrations.Abstractions;
using Defra.TradeImportsDataApi.Data.Entities;
using MongoDB.Driver;
using Version = AdaskoTheBeAsT.MongoDbMigrations.Abstractions.Version;

namespace Defra.TradeImportsDataApi.Data.Mongo.Migrations;

public class AddCustomsDeclarationsIndexes()
    : BtmsMigration("Add indexes to CustomsDeclarations collection", new Version(1, 0, 1))
{
    public override async Task UpAsync(MigrationContext context)
    {
        var collection = context.Database.GetCollection<CustomsDeclarationEntity>(
            typeof(CustomsDeclarationEntity).DataEntityName()
        );

        await CreateIndex(
            collection,
            "ImportPreNotificationIdentifierIdx",
            Builders<CustomsDeclarationEntity>.IndexKeys.Ascending(x => x.ImportPreNotificationIdentifiers),
            cancellationToken: context.CancellationToken
        );
        await CreateIndex(
            collection,
            "DeclarationUcrIdx",
            Builders<CustomsDeclarationEntity>.IndexKeys.Ascending(x => x.ClearanceRequest!.DeclarationUcr),
            cancellationToken: context.CancellationToken
        );
        await CreateIndex(
            collection,
            "UpdatedIdx",
            Builders<CustomsDeclarationEntity>.IndexKeys.Ascending(x => x.Updated),
            cancellationToken: context.CancellationToken
        );
        await CreateIndex(
            collection,
            "FinalisationMessageSentAtIdx",
            Builders<CustomsDeclarationEntity>.IndexKeys.Ascending(x => x.Finalisation!.MessageSentAt),
            cancellationToken: context.CancellationToken
        );

        await CreateIndex(
            collection,
            "TagsIdx",
            Builders<CustomsDeclarationEntity>.IndexKeys.Ascending(x => x.Tags),
            cancellationToken: context.CancellationToken
        );
    }

    public override async Task DownAsync(MigrationContext context)
    {
        var collection = context.Database.GetCollection<CustomsDeclarationEntity>(
            typeof(CustomsDeclarationEntity).DataEntityName()
        );
        await collection.Indexes.DropOneAsync("ImportPreNotificationIdentifierIdx", context.CancellationToken);
        await collection.Indexes.DropOneAsync("DeclarationUcrIdx", context.CancellationToken);
        await collection.Indexes.DropOneAsync("UpdatedIdx", context.CancellationToken);
        await collection.Indexes.DropOneAsync("FinalisationMessageSentAtIdx", context.CancellationToken);
        await collection.Indexes.DropOneAsync("TagsIdx", context.CancellationToken);
    }
}
