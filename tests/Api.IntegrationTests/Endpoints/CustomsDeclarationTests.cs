using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
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
