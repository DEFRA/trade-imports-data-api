using AdaskoTheBeAsT.MongoDbMigrations.Abstractions;
using Defra.TradeImportsDataApi.Data.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using Version = AdaskoTheBeAsT.MongoDbMigrations.Abstractions.Version;

namespace Defra.TradeImportsDataApi.Data.Mongo.Migrations;

public class PopulateCustomsDeclarationsTags()
    : BtmsMigration("Populate Customs Declarations tags field", new Version(1, 0, 6))
{
    public override async Task UpAsync(MigrationContext context)
    {
        var pipeline = new[]
        {
            new BsonDocument(
                "$set",
                new BsonDocument
                {
                    {
                        "tags",
                        new BsonDocument(
                            "$let",
                            new BsonDocument
                            {
                                {
                                    "vars",
                                    new BsonDocument
                                    {
                                        {
                                            "existingTags",
                                            new BsonDocument(
                                                "$filter",
                                                new BsonDocument
                                                {
                                                    {
                                                        "input",
                                                        new BsonDocument(
                                                            "$ifNull",
                                                            new BsonArray { "$tags", new BsonArray() }
                                                        )
                                                    },
                                                    { "as", "t" },
                                                    {
                                                        "cond",
                                                        new BsonDocument("$ne", new BsonArray { "$$t", BsonNull.Value })
                                                    },
                                                }
                                            )
                                        },
                                        { "lowerId", new BsonDocument("$toLower", "$_id") },
                                        {
                                            "lowerDucr",
                                            new BsonDocument(
                                                "$cond",
                                                new BsonArray
                                                {
                                                    new BsonDocument(
                                                        "$ne",
                                                        new BsonArray
                                                        {
                                                            "$clearanceRequest.declarationUcr",
                                                            BsonNull.Value,
                                                        }
                                                    ),
                                                    new BsonDocument("$toLower", "$clearanceRequest.declarationUcr"),
                                                    BsonNull.Value,
                                                }
                                            )
                                        },
                                    }
                                },
                                {
                                    "in",
                                    new BsonDocument(
                                        "$setUnion",
                                        new BsonArray
                                        {
                                            "$$existingTags",
                                            new BsonDocument(
                                                "$filter",
                                                new BsonDocument
                                                {
                                                    {
                                                        "input",
                                                        new BsonArray { "$$lowerId", "$$lowerDucr" }
                                                    },
                                                    { "as", "v" },
                                                    {
                                                        "cond",
                                                        new BsonDocument("$ne", new BsonArray { "$$v", BsonNull.Value })
                                                    },
                                                }
                                            ),
                                        }
                                    )
                                },
                            }
                        )
                    },
                }
            ),
        };

        var collection = context.Database.GetCollection<CustomsDeclarationEntity>(
            typeof(CustomsDeclarationEntity).DataEntityName()
        );

        // Lambda filter (strongly typed)
        await collection.UpdateManyAsync(
            Builders<CustomsDeclarationEntity>.Filter.Empty,
            Builders<CustomsDeclarationEntity>.Update.Pipeline(pipeline)
        );
    }

    public override Task DownAsync(MigrationContext context)
    {
        // No down migration needed as this is a data population task
        return Task.CompletedTask;
    }
}
