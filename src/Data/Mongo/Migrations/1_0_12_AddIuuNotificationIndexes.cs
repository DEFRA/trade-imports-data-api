using AdaskoTheBeAsT.MongoDbMigrations.Abstractions;
using Defra.TradeImportsDataApi.Data.Entities;
using MongoDB.Driver;
using Version = AdaskoTheBeAsT.MongoDbMigrations.Abstractions.Version;

namespace Defra.TradeImportsDataApi.Data.Mongo.Migrations;

public class AddIuuNotificationIndexes()
    : BtmsMigration(
        "Add indexes to IUU Notification collection and CustomsDeclaration IuuNotificationIdentifiers",
        new Version(1, 0, 12)
    )
{
    public override async Task UpAsync(MigrationContext context)
    {
        await CreateIndex(
            context.Database.GetCollection<CustomsDeclarationEntity>(typeof(CustomsDeclarationEntity).DataEntityName()),
            "IuuNotificationIdentifiersIdx",
            Builders<CustomsDeclarationEntity>.IndexKeys.Ascending(x => x.IuuNotificationIdentifiers),
            cancellationToken: context.CancellationToken
        );

        var iuuNotificationCollection = context.Database.GetCollection<IuuNotificationEntity>(
            typeof(IuuNotificationEntity).DataEntityName()
        );

        await CreateIndex(
            iuuNotificationCollection,
            "TagsIdx",
            Builders<IuuNotificationEntity>.IndexKeys.Ascending(x => x.Tags),
            cancellationToken: context.CancellationToken
        );

        await CreateIndex(
            iuuNotificationCollection,
            "UpdatedIdx",
            Builders<IuuNotificationEntity>.IndexKeys.Ascending(x => x.Updated),
            cancellationToken: context.CancellationToken
        );
    }

    public override async Task DownAsync(MigrationContext context)
    {
        await context
            .Database.GetCollection<CustomsDeclarationEntity>(typeof(CustomsDeclarationEntity).DataEntityName())
            .Indexes.DropOneAsync("IuuNotificationIdentifiersIdx", context.CancellationToken);

        var iuuNotificationCollection = context.Database.GetCollection<IuuNotificationEntity>(
            typeof(IuuNotificationEntity).DataEntityName()
        );
        await iuuNotificationCollection.Indexes.DropOneAsync("TagsIdx", context.CancellationToken);
        await iuuNotificationCollection.Indexes.DropOneAsync("UpdatedIdx", context.CancellationToken);
    }
}
