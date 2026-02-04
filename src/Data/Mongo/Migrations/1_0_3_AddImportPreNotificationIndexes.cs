using AdaskoTheBeAsT.MongoDbMigrations.Abstractions;
using Defra.TradeImportsDataApi.Data.Entities;
using MongoDB.Driver;
using Version = AdaskoTheBeAsT.MongoDbMigrations.Abstractions.Version;

namespace Defra.TradeImportsDataApi.Data.Mongo.Migrations;

public class AddImportPreNotificationIndexes()
    : BtmsMigration("Add indexes to import pre notification collection", new Version(1, 0, 3))
{
    public override async Task UpAsync(MigrationContext context)
    {
        var collection = context.Database.GetCollection<ImportPreNotificationEntity>(
            typeof(ImportPreNotificationEntity).DataEntityName()
        );

        await CreateIndex(
            collection,
            "CustomDeclarationIdentifierIdx",
            Builders<ImportPreNotificationEntity>.IndexKeys.Ascending(x => x.CustomsDeclarationIdentifier),
            cancellationToken: context.CancellationToken
        );

        await CreateIndex(
            collection,
            "UpdatedIdx",
            Builders<ImportPreNotificationEntity>.IndexKeys.Ascending(x => x.Updated),
            cancellationToken: context.CancellationToken
        );

        await CreateIndex(
            context.Database.GetCollection<ImportPreNotificationUpdateEntity>(
                typeof(ImportPreNotificationUpdateEntity).DataEntityName()
            ),
            "UpdatesIdx",
            Builders<ImportPreNotificationUpdateEntity>
                // Order of fields important - don't change without reason
                .IndexKeys.Ascending(x => x.Updated)
                .Ascending(x => x.PointOfEntry)
                .Ascending(x => x.ImportNotificationType)
                .Ascending(x => x.Status)
                .Ascending(x => x.Source!.Updated)
                .Ascending(x => x.ImportPreNotificationId),
            cancellationToken: context.CancellationToken
        );
    }

    public override async Task DownAsync(MigrationContext context)
    {
        var collection = context.Database.GetCollection<ImportPreNotificationEntity>(
            typeof(ImportPreNotificationEntity).DataEntityName()
        );
        await collection.Indexes.DropOneAsync("CustomDeclarationIdentifierIdx", context.CancellationToken);
        await collection.Indexes.DropOneAsync("UpdatedIdx", context.CancellationToken);
        await context
            .Database.GetCollection<ImportPreNotificationUpdateEntity>(
                typeof(ImportPreNotificationUpdateEntity).DataEntityName()
            )
            .Indexes.DropOneAsync("UpdatesIdx", context.CancellationToken);
    }
}
