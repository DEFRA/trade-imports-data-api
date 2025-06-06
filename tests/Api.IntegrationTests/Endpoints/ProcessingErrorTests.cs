using Defra.TradeImportsDataApi.Domain.Errors;
using FluentAssertions;
using Xunit.Abstractions;

namespace Defra.TradeImportsDataApi.Api.IntegrationTests.Endpoints;

public class ProcessingErrorTests(ITestOutputHelper testOutputHelper) : SqsTestBase(testOutputHelper)
{
    [Fact]
    public async Task WhenDoesNotExist_ShouldCreateAndRead()
    {
        var client = CreateDataApiClient();
        var mrn = Guid.NewGuid().ToString("N");

        var result = await client.GetProcessingError(mrn, CancellationToken.None);
        result.Should().BeNull();

        await client.PutProcessingError(mrn, [new ProcessingError()], null, CancellationToken.None);

        result = await client.GetProcessingError(mrn, CancellationToken.None);
        result.Should().NotBeNull();
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
    public async Task WhenCreating_ShouldEmitCreatedMessage()
    {
        var client = CreateDataApiClient();
        var mrn = Guid.NewGuid().ToString("N");
        await DrainAllMessages();

        await client.PutProcessingError(mrn, [new ProcessingError()], null, CancellationToken.None);

        Assert.True(
            await AsyncWaiter.WaitForAsync(async () => (await GetQueueAttributes()).ApproximateNumberOfMessages == 1)
        );
    }
}
