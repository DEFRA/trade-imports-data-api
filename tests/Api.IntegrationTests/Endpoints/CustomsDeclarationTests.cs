using System.Net;
using System.Text.Json;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using Defra.TradeImportsDataApi.Domain.Events;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Xunit.Abstractions;

namespace Defra.TradeImportsDataApi.Api.IntegrationTests.Endpoints;

public class CustomsDeclarationTests(ITestOutputHelper testOutputHelper) : SqsTestBase(testOutputHelper)
{
    [Fact]
    public async Task WhenDoesNotExist_ShouldCreateAndRead()
    {
        var client = CreateDataApiClient();
        var httpClient = CreateHttpClient();
        var mrn = Guid.NewGuid().ToString("N");

        var result = await client.GetCustomsDeclaration(mrn, CancellationToken.None);
        result.Should().BeNull();

        await client.PutCustomsDeclaration(
            mrn,
            new CustomsDeclaration { ClearanceRequest = new ClearanceRequest() },
            null,
            CancellationToken.None
        );

        result = await client.GetCustomsDeclaration(mrn, CancellationToken.None);
        result.Should().NotBeNull();

        var allResourceEvents = await httpClient.GetFromJsonAsyncSafe<object[]>(
            Testing.Endpoints.ResourceEvents.GetAll(mrn)
        );
        allResourceEvents.Length.Should().Be(1);
        var unpublishedResourceEvents = await httpClient.GetFromJsonAsyncSafe<object[]>(
            Testing.Endpoints.ResourceEvents.Unpublished(mrn)
        );
        unpublishedResourceEvents.Length.Should().Be(0);
    }

    [Fact]
    public async Task WhenExists_ShouldUpdate()
    {
        var client = CreateDataApiClient();
        var mrn = Guid.NewGuid().ToString("N");

        var result = await client.GetCustomsDeclaration(mrn, CancellationToken.None);
        result.Should().BeNull();

        await client.PutCustomsDeclaration(
            mrn,
            new CustomsDeclaration { ClearanceRequest = new ClearanceRequest { ExternalVersion = 1 } },
            null,
            CancellationToken.None
        );

        result = await client.GetCustomsDeclaration(mrn, CancellationToken.None);
        result.Should().NotBeNull();
        result.Created.Should().BeAfter(DateTime.MinValue);
        result.Updated.Should().BeAfter(DateTime.MinValue);
        result.ClearanceRequest?.ExternalVersion.Should().Be(1);

        await client.PutCustomsDeclaration(
            mrn,
            new CustomsDeclaration { ClearanceRequest = new ClearanceRequest { ExternalVersion = 2 } },
            result.ETag,
            CancellationToken.None
        );

        var result2 = await client.GetCustomsDeclaration(mrn, CancellationToken.None);
        result2.Should().NotBeNull();
        result2.ClearanceRequest?.ExternalVersion.Should().Be(2);
        result2.Created.Should().Be(result.Created);
        result2.Updated.Should().BeAfter(result.Updated);
    }

    [Fact]
    public async Task WhenRelatedImportPreNotificationsExists_ShouldRead()
    {
        var client = CreateDataApiClient();
        var (chedRef, chedId) = ImportPreNotificationIdGenerator.GenerateReturnId();
        var mrn = Guid.NewGuid().ToString("N");

        await client.PutImportPreNotification(
            chedRef,
            new ImportPreNotification { ReferenceNumber = chedRef, Version = 1 },
            null,
            CancellationToken.None
        );
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
                                    DocumentReference = new ImportDocumentReference($"GBCHD2025.{chedId}"),
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

        var actualResult = await client.GetImportPreNotificationsByMrn(mrn, CancellationToken.None);
        actualResult.Should().NotBeNull();
        actualResult.ImportPreNotifications.Count.Should().Be(1);
    }

    [Fact]
    public async Task WhenCreating_ThenUpdating_ShouldEmitResourceEvents()
    {
        var client = CreateDataApiClient();
        var httpClient = CreateHttpClient();
        var mrn = Guid.NewGuid().ToString("N");

        await DrainAllMessages();

        await client.PutCustomsDeclaration(
            mrn,
            new CustomsDeclaration { ClearanceRequest = new ClearanceRequest() },
            null,
            CancellationToken.None
        );

        var customsDeclaration = await client.GetCustomsDeclaration(mrn, CancellationToken.None);
        customsDeclaration.Should().NotBeNull();
        var etag = customsDeclaration.ETag?.Replace("\"", "") ?? throw new InvalidOperationException("No etag");

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
                        .ScrubMember("etag")
                        .ScrubMember("id")
                        .UseStrictJson()
                        .DontIgnoreEmptyCollections()
                        .UseMethodName($"{nameof(WhenCreating_ThenUpdating_ShouldEmitResourceEvents)}_Created");

                    var resourceEvent = JsonSerializer.Deserialize<ResourceEvent<CustomsDeclarationEvent>>(
                        message.Body
                    );

                    resourceEvent.Should().NotBeNull();
                    resourceEvent.ResourceId.Should().Be(mrn);
                    resourceEvent.Resource.Should().NotBeNull();
                    resourceEvent.Resource.Id.Should().Be(mrn);
                    resourceEvent.Resource.ETag.Should().Be(etag);
                    resourceEvent.ETag.Should().Be(etag);

                    var response = await httpClient.GetAsync(Testing.Endpoints.ResourceEvents.GetAll(mrn));
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

                    message.Body.Should().Be(resourceEventEntities[0].Message);
                }

                return expectedMessageCount;
            })
        );

        await client.PutCustomsDeclaration(
            mrn,
            new CustomsDeclaration
            {
                ClearanceRequest = customsDeclaration.ClearanceRequest,
                ClearanceDecision = new ClearanceDecision { Items = [] },
                ExternalErrors = customsDeclaration.ExternalErrors,
                Finalisation = customsDeclaration.Finalisation,
            },
            customsDeclaration.ETag,
            CancellationToken.None
        );

        customsDeclaration = await client.GetCustomsDeclaration(mrn, CancellationToken.None);
        customsDeclaration.Should().NotBeNull();
        etag = customsDeclaration.ETag?.Replace("\"", "") ?? throw new InvalidOperationException("No etag");

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
                        .ScrubMember("etag")
                        .ScrubMember("id")
                        .UseStrictJson()
                        .DontIgnoreEmptyCollections()
                        .UseMethodName($"{nameof(WhenCreating_ThenUpdating_ShouldEmitResourceEvents)}_Updated");

                    var resourceEvent = JsonSerializer.Deserialize<ResourceEvent<CustomsDeclarationEvent>>(
                        message.Body
                    );

                    resourceEvent.Should().NotBeNull();
                    resourceEvent.ResourceId.Should().Be(mrn);
                    resourceEvent.Resource.Should().NotBeNull();
                    resourceEvent.Resource.Id.Should().Be(mrn);
                    resourceEvent.Resource.ETag.Should().Be(etag);
                    resourceEvent.ETag.Should().Be(etag);

                    var response = await httpClient.GetAsync(Testing.Endpoints.ResourceEvents.GetAll(mrn));
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
    public async Task WhenWritingTwoSubResourceProperties_ShouldNotChangeDbState()
    {
        var client = CreateDataApiClient();
        var mrn = Guid.NewGuid().ToString("N");
        const int version = 1;

        var result = await client.GetCustomsDeclaration(mrn, CancellationToken.None);
        result.Should().BeNull();

        await client.PutCustomsDeclaration(
            mrn,
            new CustomsDeclaration { ClearanceRequest = new ClearanceRequest { ExternalVersion = version } },
            null,
            CancellationToken.None
        );

        result = await client.GetCustomsDeclaration(mrn, CancellationToken.None);
        result.Should().NotBeNull();
        result.ClearanceRequest?.ExternalVersion.Should().Be(version);

        HttpRequestException? exception = null;

        try
        {
            await client.PutCustomsDeclaration(
                mrn,
                new CustomsDeclaration
                {
                    ClearanceRequest = new ClearanceRequest { ExternalVersion = 2 },
                    ClearanceDecision = new ClearanceDecision { Items = [] },
                },
                result.ETag,
                CancellationToken.None
            );
        }
        catch (HttpRequestException ex)
        {
            exception = ex;
        }

        exception.Should().NotBeNull();
        exception.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

        var result2 = await client.GetCustomsDeclaration(mrn, CancellationToken.None);
        result2.Should().NotBeNull();
        result2.ClearanceRequest?.ExternalVersion.Should().Be(version);
    }
}
