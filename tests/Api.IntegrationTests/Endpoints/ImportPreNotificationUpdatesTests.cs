using System.Text.Json;
using Defra.TradeImportsDataApi.Api.Client;
using Defra.TradeImportsDataApi.Api.Endpoints.ImportPreNotifications;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using MongoDB.Driver;

namespace Defra.TradeImportsDataApi.Api.IntegrationTests.Endpoints;

public class ImportPreNotificationUpdatesTests : IntegrationTestBase, IAsyncLifetime
{
    public required IMongoCollection<ImportPreNotificationEntity> Notifications { get; set; }

    public required TradeImportsDataApiClient Client { get; set; }

    public async Task InitializeAsync()
    {
        Notifications = GetMongoCollection<ImportPreNotificationEntity>();

        await Notifications.DeleteManyAsync(FilterDefinition<ImportPreNotificationEntity>.Empty);

        Client = CreateDataApiClient();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    private static DateTime Updated => new(2025, 5, 20, 16, 0, 0, DateTimeKind.Utc);

    [Fact]
    public async Task WhenUpdatesExist_ShouldReturnExpected()
    {
        var httpClient = CreateHttpClient();
        var chedRef = ImportPreNotificationIdGenerator.Generate();
        await CreateNotification(chedRef);
        await OverrideUpdated(chedRef, Updated);

        var uri = Testing.Endpoints.ImportPreNotifications.GetUpdates(
            EndpointQuery
                .New.Where(EndpointFilter.From(Updated.AddHours(-1)))
                .Where(EndpointFilter.To(Updated.AddHours(1)))
        );
        var json = await httpClient.GetStringAsync(uri);
        var result = JsonSerializer.Deserialize<ImportPreNotificationUpdatesResponse>(json);

        result.Should().NotBeNull();
        result.ImportPreNotificationUpdates.Count.Should().Be(1);
        result.ImportPreNotificationUpdates.Should().Contain(x => x.ReferenceNumber == chedRef && x.Updated == Updated);
    }

    private async Task CreateNotification(string chedRef)
    {
        await Client.PutImportPreNotification(
            chedRef,
            new ImportPreNotification { ReferenceNumber = chedRef, Version = 1 },
            null,
            CancellationToken.None
        );
    }

    private async Task OverrideUpdated(string chedRef, DateTime updated)
    {
        var filter = Builders<ImportPreNotificationEntity>.Filter.Eq(x => x.Id, chedRef);
        var update = Builders<ImportPreNotificationEntity>.Update.Set(x => x.Updated, updated);

        await Notifications.UpdateOneAsync(filter, update);
    }
}
