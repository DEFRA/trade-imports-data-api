using System.Collections.Concurrent;
using Defra.TradeImportsDataApi.Data.Configuration;
using Defra.TradeImportsDataApi.Data.Mongo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.Driver.Authentication.AWS;
using MongoDB.Driver.Core.Events;

namespace Defra.TradeImportsDataApi.Data.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDbContext(
        this IServiceCollection services,
        IConfiguration configuration,
        bool integrationTest
    )
    {
        services
            .AddOptions<MongoDbOptions>()
            .Bind(configuration.GetSection(MongoDbOptions.SectionName))
            .ValidateDataAnnotations();

        if (integrationTest)
            return services;

        services.AddHostedService<MongoIndexService>();
        services.AddScoped<IDbContext, MongoDbContext>();
        services.AddSingleton<MongoCommandTracker>();
        services.AddSingleton(sp =>
        {
            MongoClientSettings.Extensions.AddAWSAuthentication();

            var options =
                sp.GetService<IOptions<MongoDbOptions>>() ?? throw new InvalidOperationException("Options not found");
            var settings = MongoClientSettings.FromConnectionString(options.Value.DatabaseUri);

            if (options.Value.QueryLogging)
            {
                var commandTracker = sp.GetRequiredService<MongoCommandTracker>();

                settings.ClusterConfigurator = cb =>
                {
                    cb.Subscribe<CommandStartedEvent>(commandTracker.OnCommandStarted);
                    cb.Subscribe<CommandSucceededEvent>(commandTracker.OnCommandSucceeded);
                    cb.Subscribe<CommandFailedEvent>(commandTracker.OnCommandFailed);
                };
            }

            var client = new MongoClient(settings);
            var conventionPack = new ConventionPack
            {
                new CamelCaseElementNameConvention(),
                new EnumRepresentationConvention(BsonType.String),
            };

            ConventionRegistry.Register(nameof(conventionPack), conventionPack, _ => true);
            DomainClassMapConfiguration.Register();

            return client.GetDatabase(options.Value.DatabaseName);
        });

        return services;
    }

    private sealed class MongoCommandTracker(ILogger<MongoCommandTracker> logger)
    {
        private readonly ConcurrentDictionary<long, Command> _commands = new();

        private sealed record Command(string CommandName, string Query, long Timestamp);

        public void OnCommandStarted(CommandStartedEvent @event)
        {
            if (@event.OperationId is not null && ShouldTrack(@event))
                _commands.TryAdd(
                    @event.OperationId.Value,
                    new Command(@event.CommandName, @event.Command.ToJson(), TimeProvider.System.GetTimestamp())
                );
        }

        public void OnCommandSucceeded(CommandSucceededEvent @event)
        {
            if (@event.OperationId is not null && _commands.TryRemove(@event.OperationId.Value, out var commandInfo))
                Log(LogLevel.Information, commandInfo);
        }

        public void OnCommandFailed(CommandFailedEvent @event)
        {
            if (@event.OperationId is not null && _commands.TryRemove(@event.OperationId.Value, out var commandInfo))
                Log(LogLevel.Warning, commandInfo);
        }

        private void Log(LogLevel level, Command command) =>
            logger.Log(
                level,
                "Mongo query {Result} {CommandName} {Query} took {Duration}ms",
                level == LogLevel.Information ? "succeeded" : "failed",
                command.CommandName,
                command.Query,
                TimeProvider.System.GetElapsedTime(command.Timestamp).TotalMilliseconds
            );

        private static bool ShouldTrack(CommandStartedEvent @event) =>
            @event.CommandName is "find" or "aggregate" or "count" or "distinct";
    }
}
