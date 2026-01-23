using AdaskoTheBeAsT.MongoDbMigrations.Abstractions;
using Defra.TradeImportsDataApi.Data.Entities;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using Version = AdaskoTheBeAsT.MongoDbMigrations.Abstractions.Version;

namespace Defra.TradeImportsDataApi.Data.Mongo.Migrations;

public abstract class BtmsMigration(string name, Version version) : IMigration
{
    public abstract Task UpAsync(MigrationContext context);

    public abstract Task DownAsync(MigrationContext context);

    public Version Version { get; } = version;
    public string Name { get; } = name;

    protected async Task CreateIndex<T>(
        IMongoCollection<T> collection,
        string name,
        IndexKeysDefinition<T> keys,
        bool unique = false,
        CancellationToken cancellationToken = default
    )
    {
        var indexModel = new CreateIndexModel<T>(
            keys,
            new CreateIndexOptions
            {
                Name = name,
                Background = true,
                Unique = unique,
            }
        );
        await collection.Indexes.CreateOneAsync(indexModel, cancellationToken: cancellationToken);
    }

    protected async Task CreateTtlIndex<T>(
        IMongoCollection<T> collection,
        string name,
        IndexKeysDefinition<T> keys,
        TimeSpan? expireAfter = null,
        CancellationToken cancellationToken = default
    )
    {
        var indexModel = new CreateIndexModel<T>(
            keys,
            new CreateIndexOptions
            {
                Name = name,
                Background = true,
                ExpireAfter = expireAfter ?? TimeSpan.Zero,
            }
        );
        await collection.Indexes.CreateOneAsync(indexModel, cancellationToken: cancellationToken);
    }
}
