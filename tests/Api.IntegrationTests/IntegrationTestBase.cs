using System.Net.Http.Headers;
using Defra.TradeImportsDataApi.Api.Client;
using Defra.TradeImportsDataApi.Data.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Defra.TradeImportsDataApi.Api.IntegrationTests;

[Trait("Category", "IntegrationTest")]
[Collection("Integration Tests")]
public abstract class IntegrationTestBase
{
    protected const int DefaultDataApiPort = 8080;
    protected const int DataApiWithInvalidSnsTopic = 8081;

    protected static TradeImportsDataApiClient CreateDataApiClient(int port = DefaultDataApiPort) =>
        new(CreateHttpClient(port));

    protected static HttpClient CreateHttpClient(int port = DefaultDataApiPort)
    {
        var httpClient = new HttpClient { BaseAddress = new Uri($"http://localhost:{port}") };
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Basic",
            // See compose.yml for username, password and scope configuration
            Convert.ToBase64String("IntegrationTests:integration-tests-pwd"u8.ToArray())
        );

        return httpClient;
    }

    protected static IMongoDatabase GetMongoDatabase()
    {
        var settings = MongoClientSettings.FromConnectionString("mongodb://127.0.0.1:27017/?directConnection=true");
        settings.ServerSelectionTimeout = TimeSpan.FromSeconds(5);
        settings.ConnectTimeout = TimeSpan.FromSeconds(5);
        settings.SocketTimeout = TimeSpan.FromSeconds(5);

        return new MongoClient(settings).GetDatabase("trade-imports-data-api");
    }

    protected static IMongoCollection<T> GetMongoCollection<T>()
        where T : IDataEntity
    {
        var db = GetMongoDatabase();

        return db.GetCollection<T>(typeof(T).DataEntityName());
    }

    protected IntegrationTestBase()
    {
        var conventionPack = new ConventionPack
        {
            new CamelCaseElementNameConvention(),
            new EnumRepresentationConvention(BsonType.String),
        };

        ConventionRegistry.Register(nameof(conventionPack), conventionPack, _ => true);
    }
}
