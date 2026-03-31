using AdaskoTheBeAsT.MongoDbMigrations.Abstractions;
using Defra.TradeImportsDataApi.Data.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using Version = AdaskoTheBeAsT.MongoDbMigrations.Abstractions.Version;

namespace Defra.TradeImportsDataApi.Data.Mongo.Migrations;

public class PopulateImportPreNotificationTagsWithExternalReference()
    : BtmsMigration("Populate Import Pre Notification tags field", new Version(1, 0, 8))
{
    public override async Task UpAsync(MigrationContext context)
    {
        var updatePipeline = new[]
        {
            new BsonDocument(
                "$set",
                new BsonDocument
                {
                    {
                        "tags",
                        new BsonDocument(
                            "$setUnion",
                            new BsonArray
                            {
                                new BsonDocument("$ifNull", new BsonArray { "$tags", new BsonArray() }),
                                new BsonDocument(
                                    "$map",
                                    new BsonDocument
                                    {
                                        {
                                            "input",
                                            new BsonDocument(
                                                "$filter",
                                                new BsonDocument
                                                {
                                                    {
                                                        "input",
                                                        new BsonDocument(
                                                            "$ifNull",
                                                            new BsonArray
                                                            {
                                                                "$importPreNotification.externalReferences",
                                                                new BsonArray(),
                                                            }
                                                        )
                                                    },
                                                    { "as", "ref" },
                                                    {
                                                        "cond",
                                                        new BsonDocument(
                                                            "$ne",
                                                            new BsonArray { "$$ref.reference", BsonNull.Value }
                                                        )
                                                    },
                                                }
                                            )
                                        },
                                        { "as", "ref" },
                                        { "in", new BsonDocument("$toLower", "$$ref.reference") },
                                    }
                                ),
                            }
                        )
                    },
                }
            ),
        };

        var collection = context.Database.GetCollection<ImportPreNotificationEntity>(
            typeof(ImportPreNotificationEntity).DataEntityName()
        );

        // Lambda filter (strongly typed)
        await collection.UpdateManyAsync(
            Builders<ImportPreNotificationEntity>.Filter.Empty,
            Builders<ImportPreNotificationEntity>.Update.Pipeline(updatePipeline)
        );
    }

    public override Task DownAsync(MigrationContext context)
    {
        // No down migration needed as this is a data population task
        return Task.CompletedTask;
    }
}
