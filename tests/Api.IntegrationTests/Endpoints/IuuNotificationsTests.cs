using Defra.TradeImportsDataApi.Domain.Iuu;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Xunit.Abstractions;

namespace Defra.TradeImportsDataApi.Api.IntegrationTests.Endpoints;

public class IuuNotificationsTests(ITestOutputHelper testOutputHelper) : SqsTestBase(testOutputHelper)
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

        var result = await client.GetIuuNotification(iuuReference, CancellationToken.None);
        result.Should().BeNull();

        await client.PutIuuNotification(iuuReference, body, null, CancellationToken.None);

        result = await client.GetIuuNotification(iuuReference, CancellationToken.None);
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task WhenExists_ShouldUpdate()
    {
        var client = CreateDataApiClient();
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
    }
}
