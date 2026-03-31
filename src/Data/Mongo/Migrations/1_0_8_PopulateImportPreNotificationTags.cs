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
                                                            "$and",
                                                            new BsonArray
                                                            {
                                                                // reference != null
                                                                new BsonDocument(
                                                                    "$ne",
                                                                    new BsonArray { "$$ref.reference", BsonNull.Value }
                                                                ),
                                                                // system == "NCTS"
                                                                new BsonDocument(
                                                                    "$eq",
                                                                    new BsonArray { "$$ref.system", "NCTS" }
                                                                ),
                                                                // regex validation
                                                                new BsonDocument(
                                                                    "$regexMatch",
                                                                    new BsonDocument
                                                                    {
                                                                        { "input", "$$ref.reference" },
                                                                        { "regex", "^\\d{2}[A-Z]{2}[A-Z0-9]{14}$" },
                                                                    }
                                                                ),
                                                            }
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

        await CreateIndex(
            collection,
            "TagsIdx",
            Builders<ImportPreNotificationEntity>.IndexKeys.Ascending(x => x.Tags),
            cancellationToken: context.CancellationToken
        );
    }

    public override Task DownAsync(MigrationContext context)
    {
        // No down migration needed as this is a data population task
        var collection = context.Database.GetCollection<ImportPreNotificationEntity>(
            typeof(ImportPreNotificationEntity).DataEntityName()
        );
        return collection.Indexes.DropOneAsync("TagsIdx", context.CancellationToken);
    }
}
