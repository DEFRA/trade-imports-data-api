using AdaskoTheBeAsT.MongoDbMigrations.Abstractions;
using Defra.TradeImportsDataApi.Data.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using Version = AdaskoTheBeAsT.MongoDbMigrations.Abstractions.Version;

namespace Defra.TradeImportsDataApi.Data.Mongo.Migrations;

public class PopulateGmrTagsWithTrnAndVrn() : BtmsMigration("Populate GMR tags field", new Version(1, 0, 7))
{
    public override async Task UpAsync(MigrationContext context)
    {
        var pipeline = new[]
        {
            new BsonDocument(
                "$set",
                new BsonDocument(
                    "tags",
                    new BsonDocument(
                        "$setUnion",
                        new BsonArray
                        {
                            // existing tags or []
                            new BsonDocument("$ifNull", new BsonArray { "$tags", new BsonArray() }),
                            // _id → string → lowercase
                            new BsonDocument(
                                "$cond",
                                new BsonArray
                                {
                                    new BsonDocument("$ne", new BsonArray { "$_id", BsonNull.Value }),
                                    new BsonArray
                                    {
                                        new BsonDocument("$toLower", new BsonDocument("$toString", "$_id")),
                                    },
                                    new BsonArray(),
                                }
                            ),
                            // vehicleRegistrationNumber → lowercase
                            new BsonDocument(
                                "$cond",
                                new BsonArray
                                {
                                    new BsonDocument(
                                        "$ne",
                                        new BsonArray { "$gmr.vehicleRegistrationNumber", BsonNull.Value }
                                    ),
                                    new BsonArray { new BsonDocument("$toLower", "$gmr.vehicleRegistrationNumber") },
                                    new BsonArray(),
                                }
                            ),
                            // trailerRegistrationNums[] → lowercase
                            new BsonDocument(
                                "$cond",
                                new BsonArray
                                {
                                    new BsonDocument(
                                        "$and",
                                        new BsonArray
                                        {
                                            new BsonDocument(
                                                "$ne",
                                                new BsonArray { "$gmr.trailerRegistrationNums", BsonNull.Value }
                                            ),
                                            new BsonDocument("$isArray", "$gmr.trailerRegistrationNums"),
                                        }
                                    ),
                                    new BsonDocument(
                                        "$map",
                                        new BsonDocument
                                        {
                                            { "input", "$gmr.trailerRegistrationNums" },
                                            { "as", "trailer" },
                                            { "in", new BsonDocument("$toLower", "$$trailer") },
                                        }
                                    ),
                                    new BsonArray(),
                                }
                            ),
                        }
                    )
                )
            ),
        };

        var collection = context.Database.GetCollection<GmrEntity>(typeof(GmrEntity).DataEntityName());

        // Lambda filter (strongly typed)
        await collection.UpdateManyAsync(
            Builders<GmrEntity>.Filter.Empty,
            Builders<GmrEntity>.Update.Pipeline(pipeline)
        );
    }

    public override Task DownAsync(MigrationContext context)
    {
        // No down migration needed as this is a data population task
        return Task.CompletedTask;
    }
}
