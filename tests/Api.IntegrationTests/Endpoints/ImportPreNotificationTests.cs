using Defra.TradeImportsDataApi.Api.Client;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Api.IntegrationTests.Endpoints;

public class ImportPreNotificationTests
{
    [Fact]
    public async Task WhenDoesNotExist_ShouldCreateAndRead()
    {
        var client = new TradeImportsDataApiClient(new HttpClient { BaseAddress = new Uri("http://localhost:8080") });
        var chedRef = Guid.NewGuid().ToString("N");

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
    }

    [Fact]
    public async Task WhenExists_ShouldUpdate()
    {
        var client = new TradeImportsDataApiClient(new HttpClient { BaseAddress = new Uri("http://localhost:8080") });
        var chedRef = Guid.NewGuid().ToString("N");

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

        await client.PutImportPreNotification(
            chedRef,
            new ImportPreNotification { ReferenceNumber = chedRef, Version = 2 },
            result.ETag,
            CancellationToken.None
        );

        result = await client.GetImportPreNotification(chedRef, CancellationToken.None);
        result.Should().NotBeNull();
        result.ImportPreNotification.Version.Should().Be(2);
    }
}
