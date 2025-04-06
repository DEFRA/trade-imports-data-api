using Defra.TradeImportsDataApi.Data.Mongo;
using Defra.TradeImportsDataApi.Domain.MongoDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.Driver.Core.Extensions.DiagnosticSources;

namespace Defra.TradeImportsDataApi.Data.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<MongoDbOptions>()
            .Bind(configuration.GetSection(MongoDbOptions.SectionName))
            .ValidateDataAnnotations();

        //// services.AddHostedService<MongoIndexService>();

        services.AddScoped<IDbContext, MongoDbContext>();
        services.AddSingleton(sp =>
        {
            var options = sp.GetService<IOptions<MongoDbOptions>>();
            var settings = MongoClientSettings.FromConnectionString(options?.Value.DatabaseUri);

            settings.ClusterConfigurator = cb =>
                cb.Subscribe(
                    new DiagnosticsActivityEventSubscriber(new InstrumentationOptions { CaptureCommandText = true })
                );

            var client = new MongoClient(settings);
            var conventionPack = new ConventionPack
            {
                new CamelCaseElementNameConvention(),
                new EnumRepresentationConvention(BsonType.String),
            };

            ConventionRegistry.Register(nameof(conventionPack), conventionPack, _ => true);
            ClassMapConfiguration.Register();

            return client.GetDatabase(options?.Value.DatabaseName);
        });

        return services;
    }
}
