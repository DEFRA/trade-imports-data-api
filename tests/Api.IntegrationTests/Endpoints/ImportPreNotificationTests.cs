using System.Text.Json;
using Defra.TradeImportsDataApi.Api.Utils;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using Defra.TradeImportsDataApi.Domain.Events;
using Defra.TradeImportsDataApi.Domain.Gvms;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Xunit.Abstractions;

namespace Defra.TradeImportsDataApi.Api.IntegrationTests.Endpoints;

public class ImportPreNotificationTests(ITestOutputHelper testOutputHelper) : SqsTestBase(testOutputHelper)
{
    [Fact]
    public async Task WhenDoesNotExist_ShouldCreateAndRead()
    {
        var body = ImportPreNotificationFixtures.CreateFromSample(GetType(), "ImportPreNotificationTests_Sample.json");
        var chedRef = ImportPreNotificationIdGenerator.Generate();
        var client = CreateDataApiClient();
        var httpClient = CreateHttpClient();

        var result = await client.GetImportPreNotification(chedRef, CancellationToken.None);
        result.Should().BeNull();

        await client.PutImportPreNotification(chedRef, body, null, CancellationToken.None);

        result = await client.GetImportPreNotification(chedRef, CancellationToken.None);
        result.Should().NotBeNull();

        var allResourceEvents = await httpClient.GetFromJsonAsyncSafe<object[]>(
            Testing.Endpoints.ResourceEvents.GetAll(chedRef)
        );
        allResourceEvents.Length.Should().Be(1);
        var unpublishedResourceEvents = await httpClient.GetFromJsonAsyncSafe<object[]>(
            Testing.Endpoints.ResourceEvents.Unpublished(chedRef)
        );
        unpublishedResourceEvents.Length.Should().Be(0);
    }

    [Fact]
    public async Task WhenRelatedCustomsDeclarationsDoNotExist_ShouldReturnAnEmptyList()
    {
        var client = CreateDataApiClient();
        var chedRef = ImportPreNotificationIdGenerator.Generate();

        var result = await client.GetImportPreNotification(chedRef, CancellationToken.None);
        result.Should().BeNull();

        await client.PutImportPreNotification(
            chedRef,
            new ImportPreNotification { ReferenceNumber = chedRef, Version = 1 },
            null,
            CancellationToken.None
        );

        var cdResult = await client.GetCustomsDeclarationsByChedId(chedRef, CancellationToken.None);
        cdResult.Should().NotBeNull();
        cdResult.CustomsDeclarations.Count.Should().Be(0);
    }

    [Fact]
    public async Task WhenRelatedGmrsDoNotExist_ShouldReturnAnEmptyList()
    {
        var client = CreateDataApiClient();
        var chedRef = ImportPreNotificationIdGenerator.Generate();

        var result = await client.GetImportPreNotification(chedRef, CancellationToken.None);
        result.Should().BeNull();

        await client.PutImportPreNotification(
            chedRef,
            new ImportPreNotification { ReferenceNumber = chedRef, Version = 1 },
            null,
            CancellationToken.None
        );

        var gmrsResult = await client.GetGmrsByChedId(chedRef, CancellationToken.None);
        gmrsResult.Should().NotBeNull();
        gmrsResult.Gmrs.Count.Should().Be(0);
    }

    [Fact]
    public async Task WhenExists_ShouldUpdate()
    {
        var client = CreateDataApiClient();
        var chedRef = ImportPreNotificationIdGenerator.Generate();

        var result = await client.GetImportPreNotification(chedRef, CancellationToken.None);
        result.Should().BeNull();

        await client.PutImportPreNotification(
            chedRef,
            new ImportPreNotification { ReferenceNumber = chedRef, Version = 1 },
            null,
            CancellationToken.None
        );

        result = await client.GetImportPreNotification(chedRef, CancellationToken.None);
        result.Should().NotBeNull();
        result.ImportPreNotification.Version.Should().Be(1);
        result.Created.Should().BeAfter(DateTime.MinValue);
        result.Updated.Should().BeAfter(DateTime.MinValue);

        await client.PutImportPreNotification(
            chedRef,
            new ImportPreNotification { ReferenceNumber = chedRef, Version = 2 },
            result.ETag,
            CancellationToken.None
        );

        var result2 = await client.GetImportPreNotification(chedRef, CancellationToken.None);
        result2.Should().NotBeNull();
        result2.ImportPreNotification.Version.Should().Be(2);
        result2.Created.Should().Be(result.Created);
        result2.Updated.Should().BeAfter(result.Updated);
    }

    [Fact]
    public async Task WhenRelatedCustomsDeclarationsExists_ShouldRead()
    {
        var client = CreateDataApiClient();
        var chedRef = "CHEDA.GB.2025.1234567";
        var mrn = "testmrn123";

        var result = await client.GetImportPreNotification(chedRef, CancellationToken.None);

        if (result is null)
        {
            await client.PutImportPreNotification(
                chedRef,
                new ImportPreNotification { ReferenceNumber = chedRef, Version = 1 },
                null,
                CancellationToken.None
            );
        }

        var cdResult = await client.GetCustomsDeclaration(mrn, CancellationToken.None);

        if (cdResult is null)
        {
            await client.PutCustomsDeclaration(
                mrn,
                new CustomsDeclaration
                {
                    ClearanceRequest = new ClearanceRequest
                    {
                        ExternalVersion = 1,
                        Commodities =
                        [
                            new Commodity
                            {
                                Documents =
                                [
                                    new ImportDocument
                                    {
                                        DocumentReference = new ImportDocumentReference("GBCHD2025.1234567"),
                                        DocumentCode = "C640",
                                    },
                                ],
                            },
                        ],
                    },
                },
                null,
                CancellationToken.None
            );
        }

        var actualResult = await client.GetCustomsDeclarationsByChedId(chedRef, CancellationToken.None);
        actualResult.Should().NotBeNull();
        actualResult.CustomsDeclarations.Count.Should().Be(1);
    }

    [Fact]
    public async Task WhenRelatedGmrsExists_ShouldReturnThem()
    {
        var client = CreateDataApiClient();
        var mrn = Guid.NewGuid().ToString("N");
        var gmrId = Guid.NewGuid().ToString("N");
        var gmrIdTwo = Guid.NewGuid().ToString("N");
        var (chedRef, chedId) = ImportPreNotificationIdGenerator.GenerateReturnId();

        var importPreNotification = ImportPreNotificationFixtures.CreateFromSample(
            GetType(),
            "ImportPreNotificationTests_Sample.json"
        );
        importPreNotification.ReferenceNumber = chedRef;

        var customsDeclaration = new CustomsDeclaration
        {
            ClearanceRequest = new ClearanceRequest
            {
                ExternalVersion = 1,
                Commodities =
                [
                    new Commodity
                    {
                        Documents =
                        [
                            new ImportDocument
                            {
                                DocumentReference = new ImportDocumentReference($"GBCHD2025.{chedId}"),
                                DocumentCode = "C640",
                            },
                        ],
                    },
                ],
            },
        };

        await client.PutImportPreNotification(chedRef, importPreNotification, null, CancellationToken.None);
        await client.PutCustomsDeclaration(mrn, customsDeclaration, null, CancellationToken.None);
        await client.PutGmr(
            gmrId,
            new Gmr
            {
                Id = gmrId,
                Declarations = new Declarations { Customs = [new Customs { Id = mrn }] },
            },
            null,
            CancellationToken.None
        );
        await client.PutGmr(
            gmrIdTwo,
            new Gmr
            {
                Id = gmrIdTwo,
                Declarations = new Declarations { Transits = [new Transits { Id = mrn }] },
            },
            null,
            CancellationToken.None
        );

        var actualResult = await client.GetGmrsByChedId(chedRef, CancellationToken.None);
        actualResult.Should().NotBeNull();
        actualResult.Gmrs.Count.Should().Be(2);
        actualResult.Gmrs.Should().Contain(g => g.Gmr.Id == gmrId);
        actualResult.Gmrs.Should().Contain(g => g.Gmr.Id == gmrIdTwo);
    }

    [Fact]
    public async Task WhenCreating_ThenUpdating_ShouldEmitResourceEvents()
    {
        var client = CreateDataApiClient();
        var httpClient = CreateHttpClient();
        var chedRef = ImportPreNotificationIdGenerator.Generate();

        await DrainAllMessages();

        await client.PutImportPreNotification(
            chedRef,
            new ImportPreNotification { ReferenceNumber = chedRef, Version = 1 },
            null,
            CancellationToken.None
        );

        var notification = await client.GetImportPreNotification(chedRef, CancellationToken.None);
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

                    await VerifyJson(message.Body)
                        .ScrubMember("resourceId")
                        .ScrubMember("referenceNumber")
                        .ScrubMember("etag")
                        .ScrubMember("id")
                        .ScrubMember("value")
                        .UseStrictJson()
                        .DontIgnoreEmptyCollections()
                        .UseMethodName($"{nameof(WhenCreating_ThenUpdating_ShouldEmitResourceEvents)}_Created");

                    var resourceEvent = JsonSerializer.Deserialize<ResourceEvent<ImportPreNotificationEvent>>(
                        message.Body,
                        JsonSettings.Instance
                    );

                    resourceEvent.Should().NotBeNull();
                    resourceEvent.ResourceId.Should().Be(chedRef);
                    resourceEvent.Resource.Should().NotBeNull();
                    resourceEvent.Resource.Id.Should().Be(chedRef);
                    resourceEvent.Resource.Etag.Should().Be(etag);
                    resourceEvent.Etag.Should().Be(etag);

                    var response = await httpClient.GetAsync(Testing.Endpoints.ResourceEvents.GetAll(chedRef));
                    var content = await response.Content.ReadAsStringAsync();

                    await VerifyJson(content)
                        .ScrubMember("id")
                        .ScrubMember("resourceId")
                        .ScrubMember("etag")
                        .ScrubMember("message")
                        .UseStrictJson()
                        .DontIgnoreEmptyCollections()
                        .UseMethodName(
                            $"{nameof(WhenCreating_ThenUpdating_ShouldEmitResourceEvents)}_Created_ResourceEvents"
                        );

                    var resourceEventEntities = JsonSerializer.Deserialize<ResourceEventEntity[]>(content);
                    resourceEventEntities.Should().NotBeNull();
                    resourceEventEntities.Length.Should().Be(1);

                    var resourceEventEntity = resourceEventEntities[0];

                    // Resource event body should match what was saved
                    message.Body.Should().Be(resourceEventEntity.Message);

                    // Updated should be greater than created as resource event is created first as part of main
                    // save, then updated once SNS write is complete
                    resourceEventEntity.Updated.Should().BeAfter(resourceEventEntity.Created);

                    // Updated should match Published due to above comment
                    resourceEventEntity.Published.Should().Be(resourceEventEntity.Updated);

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
            Testing.Endpoints.ResourceEvents.Publish(chedRef, resourceEventId, force: true),
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

                    var response = await httpClient.GetAsync(Testing.Endpoints.ResourceEvents.GetAll(chedRef));
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

        notification.ImportPreNotification.Version = 2;
        await client.PutImportPreNotification(
            chedRef,
            notification.ImportPreNotification,
            notification.ETag,
            CancellationToken.None
        );

        notification = await client.GetImportPreNotification(chedRef, CancellationToken.None);
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

                    await VerifyJson(message.Body)
                        .ScrubMember("resourceId")
                        .ScrubMember("referenceNumber")
                        .ScrubMember("etag")
                        .ScrubMember("id")
                        .UseStrictJson()
                        .DontIgnoreEmptyCollections()
                        .UseMethodName($"{nameof(WhenCreating_ThenUpdating_ShouldEmitResourceEvents)}_Updated");

                    var resourceEvent = JsonSerializer.Deserialize<ResourceEvent<ImportPreNotificationEvent>>(
                        message.Body,
                        JsonSettings.Instance
                    );

                    resourceEvent.Should().NotBeNull();
                    resourceEvent.ResourceId.Should().Be(chedRef);
                    resourceEvent.Resource.Should().NotBeNull();
                    resourceEvent.Resource.Id.Should().Be(chedRef);
                    resourceEvent.Resource.Etag.Should().Be(etag);
                    resourceEvent.Etag.Should().Be(etag);

                    var response = await httpClient.GetAsync(Testing.Endpoints.ResourceEvents.GetAll(chedRef));
                    var content = await response.Content.ReadAsStringAsync();

                    await VerifyJson(content)
                        .ScrubMember("id")
                        .ScrubMember("resourceId")
                        .ScrubMember("etag")
                        .ScrubMember("message")
                        .UseStrictJson()
                        .DontIgnoreEmptyCollections()
                        .UseMethodName(
                            $"{nameof(WhenCreating_ThenUpdating_ShouldEmitResourceEvents)}_Updated_ResourceEvents"
                        );

                    var resourceEventEntities = JsonSerializer.Deserialize<ResourceEventEntity[]>(content);
                    resourceEventEntities.Should().NotBeNull();
                    resourceEventEntities.Length.Should().Be(2);

                    message.Body.Should().Be(resourceEventEntities[1].Message);
                }

                return expectedMessageCount;
            })
        );
    }

    [Fact]
    public async Task WhenCreating_AndCannotWriteToSns_ResourceEventShouldBeSavedButNotPublished()
    {
        var client = CreateDataApiClient(DataApiWithInvalidSnsTopic);
        var httpClient = CreateHttpClient(DataApiWithInvalidSnsTopic);
        var chedRef = ImportPreNotificationIdGenerator.Generate();

        await DrainAllMessages();

        await client.PutImportPreNotification(
            chedRef,
            new ImportPreNotification { ReferenceNumber = chedRef, Version = 1 },
            null,
            CancellationToken.None
        );

        var notification = await client.GetImportPreNotification(chedRef, CancellationToken.None);
        notification.Should().NotBeNull();

        var allResourceEvents = await httpClient.GetFromJsonAsyncSafe<object[]>(
            Testing.Endpoints.ResourceEvents.GetAll(chedRef)
        );
        allResourceEvents.Length.Should().Be(1);
        var unpublishedResourceEvents = await httpClient.GetFromJsonAsyncSafe<ResourceEventEntity[]>(
            Testing.Endpoints.ResourceEvents.Unpublished(chedRef)
        );
        unpublishedResourceEvents.Length.Should().Be(1);

        // Move to the data API we know works against SNS
        httpClient = CreateHttpClient();

        await httpClient.PutAsync(
            Testing.Endpoints.ResourceEvents.Publish(chedRef, unpublishedResourceEvents[0].Id, force: false),
            null
        );

        allResourceEvents = await httpClient.GetFromJsonAsyncSafe<object[]>(
            Testing.Endpoints.ResourceEvents.GetAll(chedRef)
        );
        allResourceEvents.Length.Should().Be(1);
        unpublishedResourceEvents = await httpClient.GetFromJsonAsyncSafe<ResourceEventEntity[]>(
            Testing.Endpoints.ResourceEvents.Unpublished(chedRef)
        );

        // Should not be no unpublished resource events
        unpublishedResourceEvents.Length.Should().Be(0);
    }
}
