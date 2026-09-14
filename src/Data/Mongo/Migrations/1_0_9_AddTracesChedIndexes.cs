using AdaskoTheBeAsT.MongoDbMigrations.Abstractions;
using Defra.TradeImportsDataApi.Data.Entities;
using MongoDB.Driver;
using Version = AdaskoTheBeAsT.MongoDbMigrations.Abstractions.Version;

namespace Defra.TradeImportsDataApi.Data.Mongo.Migrations;

public class AddTracesChedIndexes() : BtmsMigration("Add indexes to traces ched collection", new Version(1, 0, 9))
{
    public override async Task UpAsync(MigrationContext context)
    {
        await CreateIndex(
            context.Database.GetCollection<TracesChedEntity>(typeof(TracesChedEntity).DataEntityName()),
            "UpdatedIdx",
            Builders<TracesChedEntity>
                // Id is included so the updates query can sort on updated then id without an in memory sort
                .IndexKeys.Ascending(x => x.Updated)
                .Ascending(x => x.Id),
            cancellationToken: context.CancellationToken
        );
    }

    public override async Task DownAsync(MigrationContext context)
    {
        await context
            .Database.GetCollection<TracesChedEntity>(typeof(TracesChedEntity).DataEntityName())
            .Indexes.DropOneAsync("UpdatedIdx", context.CancellationToken);
    }
}
