using Defra.TradeImportsDataApi.Api.Client;
using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Api.IntegrationTests.Endpoints;

public class RelatedImportDeclarationsTests : IntegrationTestBase
{
    [Fact]
    public async Task GivenSearchByChedId_WhenExists_AndHasIndirectNotification_AndHasMaxDepth_ThenIndirectNotificationShouldBeReturned()
    {
        var client = CreateDataApiClient();

        await InsertTestData();

        var response = await client.RelatedImportDeclarations(
            new RelatedImportDeclarationsRequest() { ChedId = "3333333", MaxLinkDepth = 1 },
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

        await InsertTestData();

        var response = await client.RelatedImportDeclarations(
            new RelatedImportDeclarationsRequest() { ChedId = "3333333" },
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

        await InsertTestData();

        var response = await client.RelatedImportDeclarations(
            new RelatedImportDeclarationsRequest() { Mrn = "mRn1" },
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

        await InsertTestData();

        var response = await client.RelatedImportDeclarations(
            new RelatedImportDeclarationsRequest() { Ducr = "dUCr1" },
            CancellationToken.None
        );

        response.Should().NotBeNull();
        response.CustomsDeclarations.Length.Should().Be(3);
        response.ImportPreNotifications.Length.Should().Be(4);
    }

    private static async Task InsertTestData()
    {
        await Task.WhenAll(
            CreateImportPreNotification("CHEDA.GB.2025.5555555"),
            CreateImportPreNotification("CHEDA.GB.2025.4444444"),
            CreateImportPreNotification("CHEDA.GB.2025.6666666"),
            CreateImportPreNotification("CHEDA.GB.2025.3333333"),
            CreateCustomsDeclaration("mrn1", "ducr1", ["GBCVD2025.6666666", "GBCVD2025.3333333"]),
            CreateCustomsDeclaration("mrn2", "ducr2", ["GBCVD2025.4444444", "GBCVD2025.6666666"]),
            CreateCustomsDeclaration("mrn3", "ducr3", ["GBCVD2025.5555555", "GBCVD2025.4444444"])
        );
    }

    private static async Task CreateCustomsDeclaration(string mrn, string duckr, List<string> links)
    {
        var client = CreateDataApiClient();

        var existing = await client.GetCustomsDeclaration(mrn, CancellationToken.None);

        if (existing is not null)
        {
            return;
        }

        var documents = links
            .Select(x => new ImportDocument()
            {
                DocumentReference = new ImportDocumentReference(x),
                DocumentCode = "C640",
            })
            .ToArray();

        var cd = new CustomsDeclaration()
        {
            ClearanceRequest = new ClearanceRequest()
            {
                DeclarationUcr = duckr,
                Commodities = [new Commodity() { Documents = documents }],
            },
        };

        await client.PutCustomsDeclaration(mrn, cd, null, CancellationToken.None);
    }

    private static async Task CreateImportPreNotification(string chedId)
    {
        var client = CreateDataApiClient();

        var notification = await client.GetImportPreNotification(chedId, CancellationToken.None);

        if (notification is not null)
        {
            return;
        }

        await client.PutImportPreNotification(
            chedId,
            new ImportPreNotification { ReferenceNumber = chedId, Version = 1 },
            null,
            CancellationToken.None
        );
    }
}
