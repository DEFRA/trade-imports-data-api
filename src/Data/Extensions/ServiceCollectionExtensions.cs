using Defra.TradeImportsDataApi.Data.Configuration;
using Defra.TradeImportsDataApi.Data.Mongo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.Driver.Authentication.AWS;
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

        ////services.AddHostedService<MongoIndexService>();

        services.AddScoped<IDbContext, MongoDbContext>();
        services.AddSingleton(sp =>
        {
            MongoClientSettings.Extensions.AddAWSAuthentication();
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
            DomainClassMapConfiguration.Register();

            return client.GetDatabase(options?.Value.DatabaseName);
        });

        return services;
    }
}
