using System.Reflection;
using System.Threading;
using AdaskoTheBeAsT.MongoDbMigrations;
using Medallion.Threading.MongoDB;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Defra.TradeImportsDataApi.Data.Mongo;

public class MongoMigrationHostedService(ILogger<MongoMigrationHostedService> logger, IMongoDatabase mongoDatabase)
    : IHostedService
{
    private readonly MongoDistributedLock _lock = new("data-api-mongo-migration", mongoDatabase);

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Mongo Migrations starting.");

        await using (await _lock.AcquireAsync(cancellationToken: cancellationToken))
        {
            using var engine = new MigrationEngineBuilder().UseDatabase(
                mongoDatabase.Client,
                mongoDatabase.DatabaseNamespace.DatabaseName
            );

            var result = await engine
                .UseAssembly(Assembly.GetExecutingAssembly())
                .UseSchemeValidation(false)
                .RunAsync(cancellationToken);

            if (!result.Success)
                throw new InvalidOperationException("Mongo Migrations Failed");
        }

        logger.LogInformation("Mongo Migrations completed.");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
