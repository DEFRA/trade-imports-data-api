using Defra.TradeImportsDataApi.Api.Endpoints.Search;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Api.Tests.Utils.InMemoryData;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Api.Tests.Services;

public class RelatedImportDeclarationsServiceTests
{
    [Fact]
    public async Task GivenSearchNothing_ThenShouldReturnEmpty()
    {
        var memoryDbContext = new MemoryDbContext();
        var subject = new RelatedImportDeclarationsService(memoryDbContext);

        var response = await subject.Search(new RelatedImportDeclarationsRequest(), CancellationToken.None);

        response.ImportPreNotifications.Length.Should().Be(0);
        response.CustomsDeclaration.Length.Should().Be(0);
    }

    [Fact]
    public async Task GivenSearchByMrn_WhenMrnExists_AndNoNotificationsExist_ThenShouldReturn()
    {
        const string mrn = "mrn";
        var memoryDbContext = new MemoryDbContext();

        await memoryDbContext.CustomsDeclarations.Insert(
            new()
            {
                Id = mrn,
                ImportPreNotificationIdentifiers = ["123"],
                ClearanceRequest = new ClearanceRequest(),
                Created = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
                Updated = new DateTime(2025, 4, 3, 10, 15, 0, DateTimeKind.Utc),
                ETag = "etag",
            }
        );

        var subject = new RelatedImportDeclarationsService(memoryDbContext);

        var response = await subject.Search(
            new RelatedImportDeclarationsRequest() { Mrn = mrn },
            CancellationToken.None
        );

        response.Should().NotBeNull();
        response.CustomsDeclaration.Length.Should().Be(1);
        response.ImportPreNotifications.Length.Should().Be(0);
    }

    [Fact]
    public async Task GivenSearchByMrn_WhenMrnExists_AndNotificationsExist_ThenShouldReturn()
    {
        const string mrn = "mrn";
        var memoryDbContext = new MemoryDbContext();

        await memoryDbContext.CustomsDeclarations.Insert(
            new()
            {
                Id = mrn,
                ImportPreNotificationIdentifiers = ["123"],
                ClearanceRequest = new ClearanceRequest(),
                Created = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
                Updated = new DateTime(2025, 4, 3, 10, 15, 0, DateTimeKind.Utc),
                ETag = "etag",
            }
        );

        await memoryDbContext.ImportPreNotifications.Insert(
            new()
            {
                Id = mrn,
                CustomsDeclarationIdentifier = "123",
                ImportPreNotification = new ImportPreNotification(),
                Created = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
                Updated = new DateTime(2025, 4, 3, 10, 15, 0, DateTimeKind.Utc),
                ETag = "etag",
            }
        );

        var subject = new RelatedImportDeclarationsService(memoryDbContext);

        var response = await subject.Search(
            new RelatedImportDeclarationsRequest() { Mrn = mrn },
            CancellationToken.None
        );

        response.Should().NotBeNull();
        response.CustomsDeclaration.Length.Should().Be(1);
        response.ImportPreNotifications.Length.Should().Be(1);
    }

    [Fact]
    public async Task GivenSearchByDucr_WhenDucrExists_AndNoNotificationsExist_ThenShouldReturn()
    {
        var memoryDbContext = new MemoryDbContext();

        await memoryDbContext.CustomsDeclarations.Insert(
            new()
            {
                Id = "mrn",
                ImportPreNotificationIdentifiers = ["123"],
                ClearanceRequest = new ClearanceRequest() { DeclarationUcr = "ducr123" },
                Created = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
                Updated = new DateTime(2025, 4, 3, 10, 15, 0, DateTimeKind.Utc),
                ETag = "etag",
            }
        );

        var subject = new RelatedImportDeclarationsService(memoryDbContext);

        var response = await subject.Search(
            new RelatedImportDeclarationsRequest() { Ducr = "ducr123" },
            CancellationToken.None
        );

        response.Should().NotBeNull();
        response.CustomsDeclaration.Length.Should().Be(1);
        response.ImportPreNotifications.Length.Should().Be(0);
    }

    [Fact]
    public async Task GivenSearchByDucr_WhenDucrExists_AndNotificationsExist_ThenShouldReturn()
    {
        var memoryDbContext = new MemoryDbContext();

        await memoryDbContext.CustomsDeclarations.Insert(
            new()
            {
                Id = "mrn",
                ImportPreNotificationIdentifiers = ["123"],
                ClearanceRequest = new ClearanceRequest() { DeclarationUcr = "ducr123" },
                Created = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
                Updated = new DateTime(2025, 4, 3, 10, 15, 0, DateTimeKind.Utc),
                ETag = "etag",
            }
        );

        await memoryDbContext.ImportPreNotifications.Insert(
            new()
            {
                Id = "mrn",
                CustomsDeclarationIdentifier = "123",
                ImportPreNotification = new ImportPreNotification(),
                Created = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
                Updated = new DateTime(2025, 4, 3, 10, 15, 0, DateTimeKind.Utc),
                ETag = "etag",
            }
        );

        var subject = new RelatedImportDeclarationsService(memoryDbContext);

        var response = await subject.Search(
            new RelatedImportDeclarationsRequest() { Ducr = "ducr123" },
            CancellationToken.None
        );

        response.Should().NotBeNull();
        response.CustomsDeclaration.Length.Should().Be(1);
        response.ImportPreNotifications.Length.Should().Be(1);
    }

    [Theory]
    [InlineData("CHEDA.GB.2025.1234567")]
    [InlineData("2025.1234567")]
    [InlineData("20251234567")]
    [InlineData("1234567")]
    public async Task GivenSearchByChedId_WhenExists_AndNoCustomDeclarationsExist_ThenShouldReturn(string searchChedId)
    {
        var memoryDbContext = new MemoryDbContext();

        await memoryDbContext.ImportPreNotifications.Insert(
            new()
            {
                Id = "CHEDA.GB.2025.1234567",
                CustomsDeclarationIdentifier = "1234567",
                ImportPreNotification = new ImportPreNotification(),
                Created = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
                Updated = new DateTime(2025, 4, 3, 10, 15, 0, DateTimeKind.Utc),
                ETag = "etag",
            }
        );

        var subject = new RelatedImportDeclarationsService(memoryDbContext);

        var response = await subject.Search(
            new RelatedImportDeclarationsRequest() { ChedId = searchChedId },
            CancellationToken.None
        );

        response.Should().NotBeNull();
        response.CustomsDeclaration.Length.Should().Be(0);
        response.ImportPreNotifications.Length.Should().Be(1);
    }

    [Fact]
    public async Task GivenSearchByChedId_WhenNotExists_ThenShouldReturnEmpty()
    {
        var memoryDbContext = new MemoryDbContext();

        var subject = new RelatedImportDeclarationsService(memoryDbContext);

        var response = await subject.Search(
            new RelatedImportDeclarationsRequest() { ChedId = "1234510" },
            CancellationToken.None
        );

        response.Should().NotBeNull();
        response.CustomsDeclaration.Length.Should().Be(1);
        response.ImportPreNotifications.Length.Should().Be(2);
    }

    [Fact]
    public async Task GivenSearchByChedId_WhenExists_AndHasIndirectNotification_ThenIndirectNotificationShouldBeReturned()
    {
        var memoryDbContext = new MemoryDbContext();
        await InsertTestData(memoryDbContext);

        var subject = new RelatedImportDeclarationsService(memoryDbContext);

        var response = await subject.Search(
            new RelatedImportDeclarationsRequest() { ChedId = "1234510" },
            CancellationToken.None
        );

        response.Should().NotBeNull();
        response.CustomsDeclaration.Length.Should().Be(3);
        response.ImportPreNotifications.Length.Should().Be(4);
    }

    [Fact]
    public async Task GivenSearchByChedId_WhenExists_AndHasIndirectNotification_AndHasMaxDepth_ThenIndirectNotificationShouldBeReturned()
    {
        var memoryDbContext = new MemoryDbContext();
        await InsertTestData(memoryDbContext);

        var subject = new RelatedImportDeclarationsService(memoryDbContext);

        var response = await subject.Search(
            new RelatedImportDeclarationsRequest() { ChedId = "1234510", MaxLinkDepth = 1 },
            CancellationToken.None
        );

        response.Should().NotBeNull();
        response.CustomsDeclaration.Length.Should().Be(2);
        response.ImportPreNotifications.Length.Should().Be(2);
    }

    private async Task InsertTestData(MemoryDbContext memoryDbContext)
    {
        await memoryDbContext.ImportPreNotifications.Insert(CreateImportPreNotification("CHEDA.GB.2025.1234567"));
        await memoryDbContext.ImportPreNotifications.Insert(CreateImportPreNotification("CHEDA.GB.2025.1234568"));
        await memoryDbContext.ImportPreNotifications.Insert(CreateImportPreNotification("CHEDA.GB.2025.1234569"));
        await memoryDbContext.ImportPreNotifications.Insert(CreateImportPreNotification("CHEDA.GB.2025.1234510"));

        await memoryDbContext.CustomsDeclarations.Insert(CreateCustomsDeclaration("mrn1", ["1234569", "1234510"]));
        await memoryDbContext.CustomsDeclarations.Insert(CreateCustomsDeclaration("mrn2", ["1234568", "1234569"]));
        await memoryDbContext.CustomsDeclarations.Insert(CreateCustomsDeclaration("mrn3", ["1234567", "1234568"]));
    }

    private ImportPreNotificationEntity CreateImportPreNotification(string chedId)
    {
        return new ImportPreNotificationEntity()
        {
            Id = chedId,
            CustomsDeclarationIdentifier = chedId.Split('.')[3],
            ImportPreNotification = new ImportPreNotification(),
            Created = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
            Updated = new DateTime(2025, 4, 3, 10, 15, 0, DateTimeKind.Utc),
            ETag = "etag",
        };
    }

    private CustomsDeclarationEntity CreateCustomsDeclaration(string mrn, List<string> links)
    {
        return new CustomsDeclarationEntity()
        {
            Id = mrn,
            ImportPreNotificationIdentifiers = links,
            Created = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
            Updated = new DateTime(2025, 4, 3, 10, 15, 0, DateTimeKind.Utc),
            ETag = "etag",
        };
    }
}
