using System.Text.Json;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Errors;
using Defra.TradeImportsDataApi.Domain.Events;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Xunit.Abstractions;

namespace Defra.TradeImportsDataApi.Api.IntegrationTests.Endpoints;

public class ProcessingErrorTests(ITestOutputHelper testOutputHelper) : SqsTestBase(testOutputHelper)
{
    [Fact]
    public async Task WhenDoesNotExist_ShouldCreateAndRead()
    {
        var client = CreateDataApiClient();
        var httpClient = CreateHttpClient();
        var mrn = Guid.NewGuid().ToString("N");

        var result = await client.GetProcessingError(mrn, CancellationToken.None);
        result.Should().BeNull();

        await client.PutProcessingError(mrn, [new ProcessingError()], null, CancellationToken.None);

        result = await client.GetProcessingError(mrn, CancellationToken.None);
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

        var result = await client.GetProcessingError(mrn, CancellationToken.None);
        result.Should().BeNull();

        await client.PutProcessingError(mrn, [], null, CancellationToken.None);

        result = await client.GetProcessingError(mrn, CancellationToken.None);
        result.Should().NotBeNull();
        result.Created.Should().BeAfter(DateTime.MinValue);
        result.Updated.Should().BeAfter(DateTime.MinValue);
        result.ProcessingErrors.Should().BeEmpty();

        await client.PutProcessingError(mrn, [new ProcessingError()], result.ETag, CancellationToken.None);

        var result2 = await client.GetProcessingError(mrn, CancellationToken.None);
        result2.Should().NotBeNull();
        result2.ProcessingErrors.Should().NotBeEmpty();
        result2.Created.Should().Be(result.Created);
        result2.Updated.Should().BeAfter(result.Updated);
    }

    [Fact]
    public async Task WhenCreating_ThenUpdating_ShouldEmitResourceEvents()
    {
        var client = CreateDataApiClient();
        var httpClient = CreateHttpClient();
        var mrn = Guid.NewGuid().ToString("N");

        await DrainAllMessages();

        await client.PutProcessingError(mrn, [new ProcessingError()], null, CancellationToken.None);

        var processingError = await client.GetProcessingError(mrn, CancellationToken.None);
        processingError.Should().NotBeNull();
        var etag = processingError.ETag?.Replace("\"", "") ?? throw new InvalidOperationException("No etag");

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

                    var resourceEvent = JsonSerializer.Deserialize<ResourceEvent<ProcessingErrorEvent>>(message.Body);

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

        await client.PutProcessingError(
            mrn,
            [.. processingError.ProcessingErrors, new ProcessingError { Message = "New error" }],
            processingError.ETag,
            CancellationToken.None
        );

        processingError = await client.GetProcessingError(mrn, CancellationToken.None);
        processingError.Should().NotBeNull();
        etag = processingError.ETag?.Replace("\"", "") ?? throw new InvalidOperationException("No etag");

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
                        .ScrubMember("ETag")
                        .ScrubMember("etag")
                        .ScrubMember("Id")
                        .UseStrictJson()
                        .DontIgnoreEmptyCollections()
                        .UseMethodName($"{nameof(WhenCreating_ThenUpdating_ShouldEmitResourceEvents)}_Updated");

                    var resourceEvent = JsonSerializer.Deserialize<ResourceEvent<ProcessingErrorEvent>>(message.Body);

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
}
