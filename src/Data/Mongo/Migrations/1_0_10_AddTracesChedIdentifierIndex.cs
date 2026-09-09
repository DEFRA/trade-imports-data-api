using AdaskoTheBeAsT.MongoDbMigrations.Abstractions;
using Defra.TradeImportsDataApi.Data.Entities;
using MongoDB.Driver;
using Version = AdaskoTheBeAsT.MongoDbMigrations.Abstractions.Version;

namespace Defra.TradeImportsDataApi.Data.Mongo.Migrations;

public class AddTracesChedIdentifierIndex()
    : BtmsMigration("Add identifier index to traces ched collection", new Version(1, 0, 10))
{
    public override async Task UpAsync(MigrationContext context)
    {
        var collection = context.Database.GetCollection<TracesChedEntity>(typeof(TracesChedEntity).DataEntityName());

        await CreateIndex(
            collection,
            "CustomsDeclarationIdentifierIdx",
            Builders<TracesChedEntity>.IndexKeys.Ascending(x => x.CustomsDeclarationIdentifier),
            cancellationToken: context.CancellationToken
        );
    }

    public override async Task DownAsync(MigrationContext context)
    {
        var collection = context.Database.GetCollection<TracesChedEntity>(typeof(TracesChedEntity).DataEntityName());
        await collection.Indexes.DropOneAsync("CustomsDeclarationIdentifierIdx", context.CancellationToken);
    }
}
