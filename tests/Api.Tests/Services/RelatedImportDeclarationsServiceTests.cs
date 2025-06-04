using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Api.Endpoints.RelatedImportDeclarations;
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
        var subject = CreateSubject(memoryDbContext);

        var response = await subject.Search(new RelatedImportDeclarationsRequest(), CancellationToken.None);

        response.ImportPreNotifications.Length.Should().Be(0);
        response.CustomsDeclarations.Length.Should().Be(0);
    }

    [Fact]
    public async Task GivenSearchByMrn_WhenMrnExists_AndNoNotificationsExist_ThenShouldReturn()
    {
        const string mrn = "mrn";
        var memoryDbContext = new MemoryDbContext();

        memoryDbContext.CustomsDeclarations.AddTestData(
            new CustomsDeclarationEntity
            {
                Id = mrn,
                ImportPreNotificationIdentifiers = ["123"],
                ClearanceRequest = new ClearanceRequest(),
                Created = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
                Updated = new DateTime(2025, 4, 3, 10, 15, 0, DateTimeKind.Utc),
                ETag = "etag",
            }
        );

        var subject = CreateSubject(memoryDbContext);

        var response = await subject.Search(new RelatedImportDeclarationsRequest { Mrn = mrn }, CancellationToken.None);

        response.Should().NotBeNull();
        response.CustomsDeclarations.Length.Should().Be(1);
        response.ImportPreNotifications.Length.Should().Be(0);
    }

    [Fact]
    public async Task GivenSearchByMrn_WhenMrnExists_AndIdentifiersMatchesAnotherMrn_ThenShouldReturn()
    {
        const string mrn1 = "mrn1";
        const string mrn2 = "mrn2";
        var memoryDbContext = new MemoryDbContext();

        memoryDbContext.CustomsDeclarations.AddTestData(
            new CustomsDeclarationEntity
            {
                Id = mrn1,
                ImportPreNotificationIdentifiers = [""],
                ClearanceRequest = new ClearanceRequest(),
                Created = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
                Updated = new DateTime(2025, 4, 3, 10, 15, 0, DateTimeKind.Utc),
                ETag = "etag",
            }
        );
        memoryDbContext.CustomsDeclarations.AddTestData(
            new CustomsDeclarationEntity
            {
                Id = mrn2,
                ImportPreNotificationIdentifiers = [""],
                ClearanceRequest = new ClearanceRequest(),
                Created = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
                Updated = new DateTime(2025, 4, 3, 10, 15, 0, DateTimeKind.Utc),
                ETag = "etag",
            }
        );

        var subject = CreateSubject(memoryDbContext);

        var response = await subject.Search(
            new RelatedImportDeclarationsRequest { Mrn = mrn1 },
            CancellationToken.None
        );

        response.Should().NotBeNull();
        response.CustomsDeclarations.Length.Should().Be(1);
        response.ImportPreNotifications.Length.Should().Be(0);
    }

    [Fact]
    public async Task GivenSearchByMrn_WhenMrnExists_AndNotificationsExist_ThenShouldReturn()
    {
        const string mrn = "mrn";
        var memoryDbContext = new MemoryDbContext();

        memoryDbContext.CustomsDeclarations.AddTestData(
            new CustomsDeclarationEntity
            {
                Id = mrn,
                ImportPreNotificationIdentifiers = ["1234567"],
                ClearanceRequest = new ClearanceRequest
                {
                    DeclarationUcr = "ducr123",
                    Commodities =
                    [
                        new Commodity
                        {
                            Documents =
                            [
                                new ImportDocument
                                {
                                    DocumentCode = "C640",
                                    DocumentReference = new ImportDocumentReference("CHEDA.GB.2025.1234567"),
                                },
                            ],
                        },
                    ],
                },
                Created = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
                Updated = new DateTime(2025, 4, 3, 10, 15, 0, DateTimeKind.Utc),
                ETag = "etag",
            }
        );

        memoryDbContext.ImportPreNotifications.AddTestData(
            new ImportPreNotificationEntity
            {
                Id = "CHEDA.GB.2025.1234567",
                CustomsDeclarationIdentifier = "1234567",
                ImportPreNotification = new ImportPreNotification(),
                Created = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
                Updated = new DateTime(2025, 4, 3, 10, 15, 0, DateTimeKind.Utc),
                ETag = "etag",
            }
        );

        var subject = CreateSubject(memoryDbContext);

        var response = await subject.Search(new RelatedImportDeclarationsRequest { Mrn = mrn }, CancellationToken.None);

        response.Should().NotBeNull();
        response.CustomsDeclarations.Length.Should().Be(1);
        response.ImportPreNotifications.Length.Should().Be(1);
    }

    [Fact]
    public async Task GivenSearchByDucr_WhenDucrExists_AndNoNotificationsExist_ThenShouldReturn()
    {
        var memoryDbContext = new MemoryDbContext();

        memoryDbContext.CustomsDeclarations.AddTestData(
            new CustomsDeclarationEntity
            {
                Id = "mrn",
                ImportPreNotificationIdentifiers = ["123"],
                ClearanceRequest = new ClearanceRequest { DeclarationUcr = "ducr123" },
                Created = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
                Updated = new DateTime(2025, 4, 3, 10, 15, 0, DateTimeKind.Utc),
                ETag = "etag",
            }
        );

        var subject = CreateSubject(memoryDbContext);

        var response = await subject.Search(
            new RelatedImportDeclarationsRequest { Ducr = "ducr123" },
            CancellationToken.None
        );

        response.Should().NotBeNull();
        response.CustomsDeclarations.Length.Should().Be(1);
        response.ImportPreNotifications.Length.Should().Be(0);
    }

    [Fact]
    public async Task GivenSearchByDucr_WhenDucrExists_AndNotificationsExist_ThenShouldReturn()
    {
        var memoryDbContext = new MemoryDbContext();

        memoryDbContext.CustomsDeclarations.AddTestData(
            new CustomsDeclarationEntity
            {
                Id = "mrn",
                ImportPreNotificationIdentifiers = ["1234567"],
                ClearanceRequest = new ClearanceRequest
                {
                    DeclarationUcr = "ducr123",
                    Commodities =
                    [
                        new Commodity
                        {
                            Documents =
                            [
                                new ImportDocument
                                {
                                    DocumentCode = "C640",
                                    DocumentReference = new ImportDocumentReference("CHEDA.GB.2025.1234567"),
                                },
                            ],
                        },
                    ],
                },
                Created = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
                Updated = new DateTime(2025, 4, 3, 10, 15, 0, DateTimeKind.Utc),
                ETag = "etag",
            }
        );

        memoryDbContext.ImportPreNotifications.AddTestData(
            new ImportPreNotificationEntity
            {
                Id = "CHEDA.GB.2025.1234567",
                CustomsDeclarationIdentifier = "1234567",
                ImportPreNotification = new ImportPreNotification(),
                Created = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
                Updated = new DateTime(2025, 4, 3, 10, 15, 0, DateTimeKind.Utc),
                ETag = "etag",
            }
        );

        var subject = CreateSubject(memoryDbContext);

        var response = await subject.Search(
            new RelatedImportDeclarationsRequest { Ducr = "ducr123" },
            CancellationToken.None
        );

        response.Should().NotBeNull();
        response.CustomsDeclarations.Length.Should().Be(1);
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

        memoryDbContext.ImportPreNotifications.AddTestData(
            new ImportPreNotificationEntity
            {
                Id = "CHEDA.GB.2025.1234567",
                CustomsDeclarationIdentifier = "1234567",
                ImportPreNotification = new ImportPreNotification(),
                Created = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
                Updated = new DateTime(2025, 4, 3, 10, 15, 0, DateTimeKind.Utc),
                ETag = "etag",
            }
        );

        var subject = CreateSubject(memoryDbContext);

        var response = await subject.Search(
            new RelatedImportDeclarationsRequest { ChedId = searchChedId },
            CancellationToken.None
        );

        response.Should().NotBeNull();
        response.CustomsDeclarations.Length.Should().Be(0);
        response.ImportPreNotifications.Length.Should().Be(1);
    }

    [Fact]
    public async Task GivenSearchByChedId_WhenNotExists_ThenShouldReturnEmpty()
    {
        var memoryDbContext = new MemoryDbContext();

        var subject = CreateSubject(memoryDbContext);

        var response = await subject.Search(
            new RelatedImportDeclarationsRequest { ChedId = "1234567" },
            CancellationToken.None
        );

        response.Should().NotBeNull();
        response.CustomsDeclarations.Length.Should().Be(0);
        response.ImportPreNotifications.Length.Should().Be(0);
    }

    [Fact]
    public async Task GivenSearchByChedId_WhenExists_AndHasIndirectNotification_ThenIndirectNotificationShouldBeReturned()
    {
        var memoryDbContext = new MemoryDbContext();
        InsertTestData(memoryDbContext);

        var subject = CreateSubject(memoryDbContext);

        var response = await subject.Search(
            new RelatedImportDeclarationsRequest { ChedId = "1234510" },
            CancellationToken.None
        );

        response.Should().NotBeNull();
        response.CustomsDeclarations.Length.Should().Be(3);
        response.ImportPreNotifications.Length.Should().Be(4);
    }

    [Fact]
    public async Task GivenSearchByChedId_WhenExists_AndHasIndirectNotification_AndHasMaxDepth_ThenIndirectNotificationShouldBeReturned()
    {
        var memoryDbContext = new MemoryDbContext();
        InsertTestData(memoryDbContext);

        var subject = CreateSubject(memoryDbContext);

        var response = await subject.Search(
            new RelatedImportDeclarationsRequest { ChedId = "1234510", MaxLinkDepth = 1 },
            CancellationToken.None
        );

        response.Should().NotBeNull();
        response.CustomsDeclarations.Length.Should().Be(2);
        response.ImportPreNotifications.Length.Should().Be(2);
    }

    private static void InsertTestData(MemoryDbContext memoryDbContext)
    {
        memoryDbContext.ImportPreNotifications.AddTestData(CreateImportPreNotification("CHEDA.GB.2025.1234567"));
        memoryDbContext.ImportPreNotifications.AddTestData(CreateImportPreNotification("CHEDA.GB.2025.1234568"));
        memoryDbContext.ImportPreNotifications.AddTestData(CreateImportPreNotification("CHEDA.GB.2025.1234569"));
        memoryDbContext.ImportPreNotifications.AddTestData(CreateImportPreNotification("CHEDA.GB.2025.1234510"));

        memoryDbContext.CustomsDeclarations.AddTestData(CreateCustomsDeclaration("mrn1", ["1234569", "1234510"]));
        memoryDbContext.CustomsDeclarations.AddTestData(CreateCustomsDeclaration("mrn2", ["1234568", "1234569"]));
        memoryDbContext.CustomsDeclarations.AddTestData(CreateCustomsDeclaration("mrn3", ["1234567", "1234568"]));
    }

    private static ImportPreNotificationEntity CreateImportPreNotification(string chedId)
    {
        return new ImportPreNotificationEntity
        {
            Id = chedId,
            CustomsDeclarationIdentifier = chedId.Split('.')[3],
            ImportPreNotification = new ImportPreNotification(),
            Created = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
            Updated = new DateTime(2025, 4, 3, 10, 15, 0, DateTimeKind.Utc),
            ETag = "etag",
        };
    }

    private static CustomsDeclarationEntity CreateCustomsDeclaration(string mrn, List<string> links)
    {
        var documents = links
            .Select(x => new ImportDocument
            {
                DocumentReference = new ImportDocumentReference(x),
                DocumentCode = "C640",
            })
            .ToArray();

        return new CustomsDeclarationEntity
        {
            Id = mrn,
            ImportPreNotificationIdentifiers = links,
            ClearanceRequest = new ClearanceRequest { Commodities = [new Commodity { Documents = documents }] },
            Created = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
            Updated = new DateTime(2025, 4, 3, 10, 15, 0, DateTimeKind.Utc),
            ETag = "etag",
        };
    }

    private static RelatedImportDeclarationsService CreateSubject(MemoryDbContext memoryDbContext)
    {
        return new RelatedImportDeclarationsService(
            new CustomsDeclarationRepository(memoryDbContext),
            new ImportPreNotificationRepository(memoryDbContext)
        );
    }
}
