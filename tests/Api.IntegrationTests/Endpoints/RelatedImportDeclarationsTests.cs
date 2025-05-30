using Defra.TradeImportsDataApi.Api.Client;
using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Api.IntegrationTests.Endpoints;

public class RelatedImportDeclarationsTests : IntegrationTestBase
{
    [Fact]
    public async Task GivenSearchByChedId_WhenExists_AndHasIndirectNotification_AndHasMaxDepth_ThenIndirectNotificationShouldBeReturned()
    {
        var client = CreateDataApiClient();
        var (chedRef, _) = await InsertTestData(client);

        var response = await client.RelatedImportDeclarations(
            new RelatedImportDeclarationsRequest { ChedId = chedRef, MaxLinkDepth = 1 },
            CancellationToken.None
        );

        response.Should().NotBeNull();
        response.CustomsDeclarations.Length.Should().Be(2);
        response.ImportPreNotifications.Length.Should().Be(2);
    }

    [Fact]
    public async Task GivenSearchByChedId_WhenExists_AndHasIndirectNotification_AndNoMaxDepth_ThenIndirectNotificationShouldBeReturned()
    {
        var client = CreateDataApiClient();
        var (chedRef, _) = await InsertTestData(client);

        var response = await client.RelatedImportDeclarations(
            new RelatedImportDeclarationsRequest { ChedId = chedRef },
            CancellationToken.None
        );

        response.Should().NotBeNull();
        response.CustomsDeclarations.Length.Should().Be(3);
        response.ImportPreNotifications.Length.Should().Be(4);
    }

    [Fact]
    public async Task GivenSearchByMrn_WhenExists_AndHasIndirectNotification_AndNoMaxDepth_ThenIndirectNotificationShouldBeReturned()
    {
        var client = CreateDataApiClient();
        var (_, random) = await InsertTestData(client);

        var response = await client.RelatedImportDeclarations(
            new RelatedImportDeclarationsRequest { Mrn = $"{random}-mRn1" },
            CancellationToken.None
        );

        response.Should().NotBeNull();
        response.CustomsDeclarations.Length.Should().Be(3);
        response.ImportPreNotifications.Length.Should().Be(4);
    }

    [Fact]
    public async Task GivenSearchByDucr_WhenExists_AndHasIndirectNotification_AndNoMaxDepth_ThenIndirectNotificationShouldBeReturned()
    {
        var client = CreateDataApiClient();
        var (_, random) = await InsertTestData(client);

        var response = await client.RelatedImportDeclarations(
            new RelatedImportDeclarationsRequest { Ducr = $"{random}-dUCr1" },
            CancellationToken.None
        );

        response.Should().NotBeNull();
        response.CustomsDeclarations.Length.Should().Be(3);
        response.ImportPreNotifications.Length.Should().Be(4);
    }

    private static async Task<(string ChedRef, string Random)> InsertTestData(TradeImportsDataApiClient client)
    {
        var chedRef1 = ImportPreNotificationIdGenerator.Generate();
        var chedRef2 = ImportPreNotificationIdGenerator.Generate();
        var chedRef3 = ImportPreNotificationIdGenerator.Generate();
        var chedRef4 = ImportPreNotificationIdGenerator.Generate();
        var random = Guid.NewGuid().ToString("N");

        await CreateImportPreNotification(client, chedRef1);
        await CreateImportPreNotification(client, chedRef2);
        await CreateImportPreNotification(client, chedRef3);
        await CreateImportPreNotification(client, chedRef4);
        await CreateCustomsDeclaration(client, $"{random}-mrn1", $"{random}-ducr1", [chedRef3, chedRef4]);
        await CreateCustomsDeclaration(client, $"{random}-mrn2", $"{random}-ducr2", [chedRef2, chedRef3]);
        await CreateCustomsDeclaration(client, $"{random}-mrn3", $"{random}-ducr3", [chedRef1, chedRef2]);

        return (chedRef4, random);
    }

    private static async Task CreateCustomsDeclaration(
        TradeImportsDataApiClient client,
        string mrn,
        string ducr,
        List<string> links
    )
    {
        var documents = links
            .Select(x => new ImportDocument
            {
                DocumentReference = new ImportDocumentReference(x),
                DocumentCode = "C640",
            })
            .ToArray();

        var cd = new CustomsDeclaration
        {
            ClearanceRequest = new ClearanceRequest
            {
                DeclarationUcr = ducr,
                Commodities = [new Commodity { Documents = documents }],
            },
        };

        await client.PutCustomsDeclaration(mrn, cd, null, CancellationToken.None);
    }

    private static async Task CreateImportPreNotification(TradeImportsDataApiClient client, string chedId)
    {
        await client.PutImportPreNotification(
            chedId,
            new ImportPreNotification { ReferenceNumber = chedId, Version = 1 },
            null,
            CancellationToken.None
        );
    }
}
