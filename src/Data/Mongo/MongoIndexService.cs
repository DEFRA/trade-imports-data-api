using Defra.TradeImportsDataApi.Data.Entities;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Defra.TradeImportsDataApi.Data.Mongo;

public class MongoIndexService(IMongoDatabase database, ILogger<MongoIndexService> logger) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return CreateIndex(
            "CustomDeclarationIdentifierIdx",
            Builders<ImportNotificationEntity>.IndexKeys.Ascending(n => n.CustomDeclarationIdentifier),
            cancellationToken: cancellationToken
        );
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task CreateIndex<T>(
        string name,
        IndexKeysDefinition<T> keys,
        bool unique = false,
        CancellationToken cancellationToken = default
    )
    {
        try
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
            await database
                .GetCollection<T>(typeof(T).Name)
                .Indexes.CreateOneAsync(indexModel, cancellationToken: cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to Create index {Name} on {Collection}", name, typeof(T).Name);
        }
    }
}
