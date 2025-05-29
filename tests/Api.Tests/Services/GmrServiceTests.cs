using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Gvms;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using FluentAssertions;
using NSubstitute;

namespace Defra.TradeImportsDataApi.Api.Tests.Services;

public class GmrServiceTests
{
    private IDbContext DbContext { get; }
    private IGmrRepository GmrRepository { get; }
    private IImportPreNotificationRepository ImportPreNotificationRepository { get; }
    private ICustomsDeclarationRepository CustomsDeclarationRepository { get; }
    private GmrService Subject { get; }

    public GmrServiceTests()
    {
        DbContext = Substitute.For<IDbContext>();
        GmrRepository = Substitute.For<IGmrRepository>();
        ImportPreNotificationRepository = Substitute.For<IImportPreNotificationRepository>();
        CustomsDeclarationRepository = Substitute.For<ICustomsDeclarationRepository>();

        Subject = new GmrService(
            DbContext,
            GmrRepository,
            ImportPreNotificationRepository,
            CustomsDeclarationRepository
        );
    }

    [Fact]
    public async Task GetGmrByChedId_WhenChedDoesNotExist_ReturnsEmptyList()
    {
        const string chedRef = "CHEDDY";
        ImportPreNotificationRepository
            .GetCustomsDeclarationIdentifier(chedRef, CancellationToken.None)
            .Returns((string?)null);

        var result = await Subject.GetGmrByChedId(chedRef, CancellationToken.None);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetGmrByChedId_WhenChedExists_ButHasNoLinkedCustomsDeclarations_ReturnsEmptyList()
    {
        const string chedRef = "CHEDDY";
        const string mrn = "mrn123";

        var importNotification = new ImportPreNotificationEntity
        {
            Id = chedRef,
            CustomsDeclarationIdentifier = mrn,
            ImportPreNotification = new ImportPreNotification(),
        };

        ImportPreNotificationRepository
            .GetCustomsDeclarationIdentifier(chedRef, CancellationToken.None)
            .Returns(importNotification.CustomsDeclarationIdentifier);

        CustomsDeclarationRepository
            .GetAllIds(importNotification.CustomsDeclarationIdentifier, CancellationToken.None)
            .Returns([]);

        var result = await Subject.GetGmrByChedId(chedRef, CancellationToken.None);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetGmrByChedId_WhenChedExists_AndHasLinkedCustomsDeclarations_ReturnsListOfGmrs()
    {
        const string chedRef = "CHEDDY";
        const string mrn = "mrn123";
        const string gmr = "gmr123";

        var importNotification = new ImportPreNotificationEntity
        {
            Id = chedRef,
            CustomsDeclarationIdentifier = mrn,
            ImportPreNotification = new ImportPreNotification(),
        };

        ImportPreNotificationRepository
            .GetCustomsDeclarationIdentifier(chedRef, CancellationToken.None)
            .Returns(importNotification.CustomsDeclarationIdentifier);

        CustomsDeclarationRepository
            .GetAllIds(importNotification.CustomsDeclarationIdentifier, CancellationToken.None)
            .Returns([mrn]);

        GmrRepository
            .GetAll(Arg.Is<string[]>(x => x.SequenceEqual(new[] { mrn })), CancellationToken.None)
            .Returns([new GmrEntity { Id = gmr, Gmr = new Gmr() }]);

        var result = await Subject.GetGmrByChedId(chedRef, CancellationToken.None);

        result.Count.Should().Be(1);
        result[0].Id.Should().Be(gmr);
    }

    [Fact]
    public async Task Insert_ShouldInsert()
    {
        var entity = new GmrEntity { Id = "id", Gmr = new Gmr() };

        await Subject.Insert(entity, CancellationToken.None);

        await GmrRepository.Received().Insert(entity, CancellationToken.None);
        await DbContext.Received().SaveChangesAsync(CancellationToken.None);
    }

    [Fact]
    public async Task Update_ShouldUpdate()
    {
        const string id = "id";
        GmrRepository
            .Get(id, CancellationToken.None)
            .Returns(
                new GmrEntity
                {
                    Id = id,
                    Gmr = new Gmr { State = "OPEN" },
                }
            );
        var entity = new GmrEntity
        {
            Id = id,
            Gmr = new Gmr { State = "COMPLETED" },
        };

        await Subject.Update(entity, "etag", CancellationToken.None);

        await GmrRepository.Received().Update(entity, "etag", CancellationToken.None);
        await DbContext.Received().SaveChangesAsync(CancellationToken.None);
    }

    [Fact]
    public async Task GetGmr_ShouldReturn()
    {
        const string id = "id";
        GmrRepository.Get(id, CancellationToken.None).Returns(new GmrEntity { Id = id, Gmr = new Gmr() });

        var result = await Subject.GetGmr(id, CancellationToken.None);

        result.Should().NotBeNull();
    }
}
