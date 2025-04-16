using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Api.IntegrationTests.Endpoints;

public class CustomsDeclarationTests : SqsTestBase
{
    [Fact]
    public async Task WhenDoesNotExist_ShouldCreateAndRead()
    {
        var client = CreateDataApiClient();
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
        result.ClearanceRequest?.ExternalVersion.Should().Be(1);

        await client.PutCustomsDeclaration(
            mrn,
            new CustomsDeclaration { ClearanceRequest = new ClearanceRequest { ExternalVersion = 2 } },
            result.ETag,
            CancellationToken.None
        );

        result = await client.GetCustomsDeclaration(mrn, CancellationToken.None);
        result.Should().NotBeNull();
        result.ClearanceRequest?.ExternalVersion.Should().Be(2);
    }

    [Fact]
    public async Task WhenRelatedImportPreNotificationsExists_ShouldRead()
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

        var actualResult = await client.GetImportPreNotificationsByMrn(mrn, CancellationToken.None);
        actualResult.Should().NotBeNull();
        actualResult.Count.Should().Be(1);
    }

    [Fact]
    public async Task WhenCreating_ShouldEmitCreatedMessage()
    {
        var client = CreateDataApiClient();
        var mrn = Guid.NewGuid().ToString("N");
        await DrainAllMessages();

        await client.PutCustomsDeclaration(
            mrn,
            new CustomsDeclaration { ClearanceRequest = new ClearanceRequest() },
            null,
            CancellationToken.None
        );

        Assert.True(
            await AsyncWaiter.WaitForAsync(async () => (await GetQueueAttributes()).ApproximateNumberOfMessages == 1)
        );
    }
}
