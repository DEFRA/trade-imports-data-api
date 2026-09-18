using AdaskoTheBeAsT.MongoDbMigrations.Abstractions;
using Defra.TradeImportsDataApi.Data.Entities;
using MongoDB.Driver;
using Version = AdaskoTheBeAsT.MongoDbMigrations.Abstractions.Version;

namespace Defra.TradeImportsDataApi.Data.Mongo.Migrations;

public class AddIndexToChedIdForChedReservations()
    : BtmsMigration("Add ChedId index for reservations", new Version(1, 0, 9))
{
    public override async Task UpAsync(MigrationContext context)
    {
        var collection = context.Database.GetCollection<ChedReservationEntity>(
            typeof(ChedReservationEntity).DataEntityName()
        );

        await CreateIndex(
            collection,
            "ChedIdIdx",
            Builders<ChedReservationEntity>.IndexKeys.Ascending(x => x.Reservation.ChedId),
            cancellationToken: context.CancellationToken
        );
    }

    public override Task DownAsync(MigrationContext context)
    {
        // No down migration needed as this is a data population task
        var collection = context.Database.GetCollection<ChedReservationEntity>(
            typeof(ChedReservationEntity).DataEntityName()
        );
        return collection.Indexes.DropOneAsync("ChedIdIdx", context.CancellationToken);
    }
}
