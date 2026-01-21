using AdaskoTheBeAsT.MongoDbMigrations.Abstractions;
using Defra.TradeImportsDataApi.Data.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using Version = AdaskoTheBeAsT.MongoDbMigrations.Abstractions.Version;

namespace Defra.TradeImportsDataApi.Data.Mongo.Migrations;

public class PopulateGmrTags() : BtmsMigration("Populate GMR tags field", new Version(1, 0, 5))
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
                        "$cond",
                        new BsonArray
                        {
                            new BsonDocument("$in", new BsonArray { new BsonDocument("$toLower", "$_id"), "$tags" }),
                            "$tags",
                            new BsonDocument(
                                "$concatArrays",
                                new BsonArray
                                {
                                    "$tags",
                                    new BsonArray { new BsonDocument("$toLower", "$_id") },
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
