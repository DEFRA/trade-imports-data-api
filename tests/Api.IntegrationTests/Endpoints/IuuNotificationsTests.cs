using System.Text.Json;
using Defra.TradeImportsDataApi.Api.Utils;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Events;
using Defra.TradeImportsDataApi.Domain.Iuu;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Xunit.Abstractions;

namespace Defra.TradeImportsDataApi.Api.IntegrationTests.Endpoints;

public class IuuNotificationsTests(ITestOutputHelper testOutputHelper)
    : SqsTestBase(testOutputHelper, IuuNotificationUpsertedQueueName)
{
    [Fact]
    public async Task WhenDoesNotExist_ShouldCreateAndRead()
    {
        var iuuReference = IuuReferenceGenerator.Generate();
        var body = new IuuNotification
        {
            Reference = iuuReference,
            LastUpdated = DateTime.Today,
            IuuOption = "IUUOK",
        };

        var client = CreateDataApiClient();
        var httpClient = CreateHttpClient();

        var result = await client.GetIuuNotification(iuuReference, CancellationToken.None);
        result.Should().BeNull();

        await client.PutIuuNotification(iuuReference, body, null, CancellationToken.None);

        result = await client.GetIuuNotification(iuuReference, CancellationToken.None);
        result.Should().NotBeNull();

        var allResourceEvents = await httpClient.GetFromJsonAsyncSafe<object[]>(
            Testing.Endpoints.ResourceEvents.GetAll(iuuReference)
        );
        allResourceEvents.Length.Should().Be(1);
        var unpublishedResourceEvents = await httpClient.GetFromJsonAsyncSafe<object[]>(
            Testing.Endpoints.ResourceEvents.Unpublished(iuuReference)
        );
        unpublishedResourceEvents.Length.Should().Be(0);
    }

    [Fact]
    public async Task WhenExists_ShouldUpdate()
    {
        var client = CreateDataApiClient();
        var httpClient = CreateHttpClient();
        var iuuReference = IuuReferenceGenerator.Generate();
        var lastUpdated = new DateTime(2026, 10, 16, 9, 0, 0, DateTimeKind.Utc);

        var result = await client.GetIuuNotification(iuuReference, CancellationToken.None);
        result.Should().BeNull();

        await client.PutIuuNotification(
            iuuReference,
            new IuuNotification
            {
                Reference = iuuReference,
                LastUpdated = lastUpdated,
                IuuOption = "IUUOK",
            },
            null,
            CancellationToken.None
        );

        result = await client.GetIuuNotification(iuuReference, CancellationToken.None);
        result.Should().NotBeNull();
        result.IuuNotification.Reference.Should().Be(iuuReference);
        result.IuuNotification.LastUpdated.Should().Be(lastUpdated);
        result.IuuNotification.IuuOption.Should().Be("IUUOK");
        result.Created.Should().BeAfter(DateTime.MinValue);
        result.Updated.Should().BeAfter(DateTime.MinValue);

        await client.PutIuuNotification(
            iuuReference,
            new IuuNotification
            {
                Reference = iuuReference,
                LastUpdated = lastUpdated.AddMinutes(1),
                IuuOption = "IUUNA",
            },
            result.ETag,
            CancellationToken.None
        );

        var result2 = await client.GetIuuNotification(iuuReference, CancellationToken.None);
        result2.Should().NotBeNull();
        result2.IuuNotification.Reference.Should().Be(iuuReference);
        result2.IuuNotification.LastUpdated.Should().Be(lastUpdated.AddMinutes(1));
        result2.IuuNotification.IuuOption.Should().Be("IUUNA");
        result2.Created.Should().Be(result.Created);
        result2.Updated.Should().BeAfter(result.Updated);

        var allResourceEvents = await httpClient.GetFromJsonAsyncSafe<ResourceEventEntity[]>(
            Testing.Endpoints.ResourceEvents.GetAll(iuuReference)
        );
        allResourceEvents
            .Select(x => x.Operation)
            .Should()
            .Equal(ResourceEventOperations.Created, ResourceEventOperations.Updated);
        var unpublishedResourceEvents = await httpClient.GetFromJsonAsyncSafe<object[]>(
            Testing.Endpoints.ResourceEvents.Unpublished(iuuReference)
        );
        unpublishedResourceEvents.Length.Should().Be(0);
    }

    [Fact]
    public async Task WhenCreating_ThenUpdating_ShouldEmitResourceEvents()
    {
        var client = CreateDataApiClient();
        var httpClient = CreateHttpClient();
        var iuuReference = IuuReferenceGenerator.Generate();

        await DrainAllMessages();

        await client.PutIuuNotification(
            iuuReference,
            new IuuNotification
            {
                Reference = iuuReference,
                LastUpdated = new DateTime(2026, 10, 16, 9, 0, 0, DateTimeKind.Utc),
                IuuOption = "IUUOK",
            },
            null,
            CancellationToken.None
        );

        var notification = await client.GetIuuNotification(iuuReference, CancellationToken.None);
        notification.Should().NotBeNull();
        var etag = notification.ETag?.Replace("\"", "") ?? throw new InvalidOperationException("No etag");

        string? resourceEventId = null;
        DateTime? publishedDate = null;
        string? messageBody = null;

        Assert.True(
            await AsyncWaiter.WaitForAsync(async () =>
            {
                // Expect single resource event to have been emitted
                var expectedMessageCount = (await GetQueueAttributes()).ApproximateNumberOfMessages == 1;

                if (expectedMessageCount)
                {
                    // Get the resource event message
                    var messageResponse = await ReceiveMessage();
                    var message = messageResponse.Messages[0];

                    await VerifyResourceEventMessage(message.Body, iuuReference, etag, "Created");

                    var response = await httpClient.GetAsync(Testing.Endpoints.ResourceEvents.GetAll(iuuReference));
                    var content = await response.Content.ReadAsStringAsync();

                    await VerifyResourceEvents(content, "Created");

                    var resourceEventEntities = JsonSerializer.Deserialize<ResourceEventEntity[]>(content);
                    resourceEventEntities.Should().NotBeNull();

                    var resourceEventEntity = resourceEventEntities[0];

                    // Resource event body should match what was saved
                    message.Body.Should().Be(resourceEventEntity.Message);

                    resourceEventEntity.Updated.Should().BeAfter(resourceEventEntity.Created);

                    resourceEventEntity.Published.Should().NotBeNull();

                    // Store the following for republish checking
                    resourceEventId = resourceEventEntity.Id;
                    publishedDate = resourceEventEntity.Published;
                    messageBody = message.Body;
                }

                return expectedMessageCount;
            })
        );

        resourceEventId.Should().NotBeNull();

        await DrainAllMessages();

        // Republish the already sent resource event
        await httpClient.PutAsync(
            Testing.Endpoints.ResourceEvents.Publish(iuuReference, resourceEventId, force: true),
            null
        );

        Assert.True(
            await AsyncWaiter.WaitForAsync(async () =>
            {
                // Expect single resource event to have been emitted
                var expectedMessageCount = (await GetQueueAttributes()).ApproximateNumberOfMessages == 1;

                if (expectedMessageCount)
                {
                    // Get the resource event message
                    var messageResponse = await ReceiveMessage();
                    var message = messageResponse.Messages[0];

                    // Message should not have changed
                    message.Body.Should().Be(messageBody);

                    var response = await httpClient.GetAsync(Testing.Endpoints.ResourceEvents.GetAll(iuuReference));
                    var content = await response.Content.ReadAsStringAsync();

                    var resourceEventEntities = JsonSerializer.Deserialize<ResourceEventEntity[]>(content);
                    resourceEventEntities.Should().NotBeNull();
                    resourceEventEntities.Length.Should().Be(1);

                    // Published field should now be different as it was republished
                    resourceEventEntities[0].Published.Should().BeAfter(publishedDate.GetValueOrDefault());
                }

                return expectedMessageCount;
            })
        );

        await DrainAllMessages();

        notification.IuuNotification.IuuOption = "IUUNA";
        await client.PutIuuNotification(
            iuuReference,
            notification.IuuNotification,
            notification.ETag,
            CancellationToken.None
        );

        notification = await client.GetIuuNotification(iuuReference, CancellationToken.None);
        notification.Should().NotBeNull();
        etag = notification.ETag?.Replace("\"", "") ?? throw new InvalidOperationException("No etag");

        Assert.True(
            await AsyncWaiter.WaitForAsync(async () =>
            {
                var expectedMessageCount = (await GetQueueAttributes()).ApproximateNumberOfMessages == 1;

                if (expectedMessageCount)
                {
                    var messageResponse = await ReceiveMessage();
                    var message = messageResponse.Messages[0];

                    await VerifyResourceEventMessage(message.Body, iuuReference, etag, "Updated");

                    var response = await httpClient.GetAsync(Testing.Endpoints.ResourceEvents.GetAll(iuuReference));
                    var content = await response.Content.ReadAsStringAsync();

                    await VerifyResourceEvents(content, "Updated");

                    var resourceEventEntities = JsonSerializer.Deserialize<ResourceEventEntity[]>(content);
                    resourceEventEntities.Should().NotBeNull();

                    message.Body.Should().Be(resourceEventEntities[1].Message);
                }

                return expectedMessageCount;
            })
        );
    }

    private static async Task VerifyResourceEventMessage(
        string messageBody,
        string iuuReference,
        string etag,
        string operation
    )
    {
        await VerifyJson(messageBody)
            .ScrubMember("resourceId")
            .ScrubMember("reference")
            .ScrubMember("etag")
            .ScrubMember("id")
            .UseStrictJson()
            .DontIgnoreEmptyCollections()
            .UseMethodName($"{nameof(WhenCreating_ThenUpdating_ShouldEmitResourceEvents)}_{operation}");

        // Scrubbed above, so check they match the IUU Notification that was written
        var resourceEvent = JsonSerializer.Deserialize<ResourceEvent<IuuNotificationEvent>>(
            messageBody,
            JsonSettings.Instance
        );

        resourceEvent.Should().NotBeNull();
        resourceEvent.ResourceId.Should().Be(iuuReference);
        resourceEvent.Etag.Should().Be(etag);
        resourceEvent.Resource.Should().NotBeNull();
        resourceEvent.Resource.Etag.Should().Be(etag);
    }

    private static async Task VerifyResourceEvents(string content, string operation)
    {
        await VerifyJson(content)
            .ScrubMember("id")
            .ScrubMember("resourceId")
            .ScrubMember("etag")
            .ScrubMember("message")
            .ScrubMember("published")
            .UseStrictJson()
            .DontIgnoreEmptyCollections()
            .UseMethodName($"{nameof(WhenCreating_ThenUpdating_ShouldEmitResourceEvents)}_{operation}_ResourceEvents");
    }
}
