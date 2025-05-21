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
    public required IMongoCollection<ImportPreNotificationEntity> Collection { get; set; }

    // ReSharper disable once MemberCanBePrivate.Global
    public class NotificationUpdatedBetweenFromAndToTestCases : TheoryData<DateTime, DateTime, DateTime, int>
    {
        public NotificationUpdatedBetweenFromAndToTestCases()
        {
            // Updated equal to "from" and less than "to"
            Add(
                new DateTime(2025, 5, 20, 16, 0, 0, DateTimeKind.Utc),
                new DateTime(2025, 5, 20, 16, 0, 0, DateTimeKind.Utc),
                new DateTime(2025, 5, 20, 17, 0, 0, DateTimeKind.Utc),
                1
            );

            // Updated greater than "from" and less than "to"
            Add(
                new DateTime(2025, 5, 20, 16, 0, 1, DateTimeKind.Utc),
                new DateTime(2025, 5, 20, 16, 0, 0, DateTimeKind.Utc),
                new DateTime(2025, 5, 20, 17, 0, 0, DateTimeKind.Utc),
                1
            );

            // Updated greater than "from" and less than "to"
            Add(
                new DateTime(2025, 5, 20, 16, 59, 59, DateTimeKind.Utc),
                new DateTime(2025, 5, 20, 16, 0, 0, DateTimeKind.Utc),
                new DateTime(2025, 5, 20, 17, 0, 0, DateTimeKind.Utc),
                1
            );

            // Updated greater than "from" and equal to "to" - should not be returned
            Add(
                new DateTime(2025, 5, 20, 17, 0, 0, DateTimeKind.Utc),
                new DateTime(2025, 5, 20, 16, 0, 0, DateTimeKind.Utc),
                new DateTime(2025, 5, 20, 17, 0, 0, DateTimeKind.Utc),
                0
            );
        }
    }

    [Theory, ClassData(typeof(NotificationUpdatedBetweenFromAndToTestCases))]
    public async Task WhenNotificationUpdated_ShouldReturnExpected(
        DateTime updated,
        DateTime from,
        DateTime to,
        int expectedCount
    )
    {
        var client = CreateDataApiClient();
        var httpClient = CreateHttpClient();
        var chedRef = ImportPreNotificationIdGenerator.Generate();
        await CreateNotification(client, chedRef);
        await OverrideUpdated(chedRef, updated);

        var uri = Testing.Endpoints.ImportPreNotifications.GetUpdates(
            EndpointQuery.New.Where(EndpointFilter.From(from)).Where(EndpointFilter.To(to))
        );
        var json = await httpClient.GetStringAsync(uri);
        var result = JsonSerializer.Deserialize<ImportPreNotificationUpdatesResponse>(json);

        result.Should().NotBeNull();
        result.ImportPreNotificationUpdates.Count.Should().Be(expectedCount);
        if (expectedCount > 0)
            result.ImportPreNotificationUpdates.Should().Contain(x => x.ReferenceNumber == chedRef);
    }

    private static async Task CreateNotification(TradeImportsDataApiClient client, string chedRef)
    {
        await client.PutImportPreNotification(
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

        await Collection.UpdateOneAsync(filter, update);
    }

    public async Task InitializeAsync()
    {
        Collection = GetMongoCollection<ImportPreNotificationEntity>();

        await Collection.DeleteManyAsync(FilterDefinition<ImportPreNotificationEntity>.Empty);
    }

    public Task DisposeAsync() => Task.CompletedTask;
}
