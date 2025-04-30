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
            new RelatedImportDeclarationsRequest() { ChedId = "1234510", MaxLinkDepth = 1 },
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
            new RelatedImportDeclarationsRequest() { ChedId = "1234510" },
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
            new RelatedImportDeclarationsRequest() { Mrn = "mrn1" },
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
            new RelatedImportDeclarationsRequest() { Ducr = "ducr1" },
            CancellationToken.None
        );

        response.Should().NotBeNull();
        response.CustomsDeclarations.Length.Should().Be(3);
        response.ImportPreNotifications.Length.Should().Be(4);
    }

    private async Task InsertTestData()
    {
        await Task.WhenAll(
            CreateImportPreNotification("CHEDA.GB.2025.1234567"),
            CreateImportPreNotification("CHEDA.GB.2025.1234568"),
            CreateImportPreNotification("CHEDA.GB.2025.1234569"),
            CreateImportPreNotification("CHEDA.GB.2025.1234510"),
            CreateCustomsDeclaration("mrn1", "ducr1", ["GBCVD2025.1234569", "GBCVD2025.1234510"]),
            CreateCustomsDeclaration("mrn2", "ducr2", ["GBCVD2025.1234568", "GBCVD2025.1234569"]),
            CreateCustomsDeclaration("mrn3", "ducr3", ["GBCVD2025.1234567", "GBCVD2025.1234568"])
        );
    }

    private async Task CreateCustomsDeclaration(string mrn, string duckr, List<string> links)
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

    private async Task CreateImportPreNotification(string chedId)
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
