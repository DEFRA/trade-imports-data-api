using System.Linq.Expressions;
using Defra.TradeImportsDataApi.Api.Exceptions;
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
    [Fact]
    public async Task GetGmrByChedId_WhenChedDoesNotExist_ReturnsEmptyList()
    {
        var mockDbContext = Substitute.For<IDbContext>();
        var subject = new GmrService(mockDbContext);

        const string chedRef = "CHEDDY";

        var result = await subject.GetGmrByChedId(chedRef, CancellationToken.None);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetGmrByChedId_WhenChedExists_ButHasNoLinkedCustomsDeclarations_ReturnsEmptyList()
    {
        var mockDbContext = Substitute.For<IDbContext>();
        var subject = new GmrService(mockDbContext);

        const string chedRef = "CHEDDY";
        const string mrn = "mrn123";

        var importNotification = new ImportPreNotificationEntity
        {
            Id = chedRef,
            CustomsDeclarationIdentifier = mrn,
            ImportPreNotification = new ImportPreNotification(),
        };

        mockDbContext
            .ImportPreNotifications.Find(
                Arg.Any<Expression<Func<ImportPreNotificationEntity, bool>>>(),
                CancellationToken.None
            )
            .Returns(importNotification);
        mockDbContext
            .CustomsDeclarations.FindMany(
                Arg.Any<Expression<Func<CustomsDeclarationEntity, bool>>>(),
                CancellationToken.None
            )
            .Returns([]);
        var result = await subject.GetGmrByChedId(chedRef, CancellationToken.None);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetGmrByChedId_WhenChedExists_AndHasLinkedCustomsDeclarations_ReturnsListOfGmrs()
    {
        var mockDbContext = Substitute.For<IDbContext>();
        var subject = new GmrService(mockDbContext);

        const string chedRef = "CHEDDY";
        const string mrn = "mrn123";
        const string gmr = "gmr123";

        var importNotification = new ImportPreNotificationEntity
        {
            Id = chedRef,
            CustomsDeclarationIdentifier = mrn,
            ImportPreNotification = new ImportPreNotification(),
        };

        var customsDeclaration = new CustomsDeclarationEntity
        {
            Id = mrn,
            ImportPreNotificationIdentifiers = [importNotification.Id],
        };

        mockDbContext
            .ImportPreNotifications.Find(
                Arg.Any<Expression<Func<ImportPreNotificationEntity, bool>>>(),
                CancellationToken.None
            )
            .Returns(importNotification);
        mockDbContext
            .CustomsDeclarations.FindMany(
                Arg.Any<Expression<Func<CustomsDeclarationEntity, bool>>>(),
                CancellationToken.None
            )
            .Returns([customsDeclaration]);
        mockDbContext
            .Gmrs.FindMany(Arg.Any<Expression<Func<GmrEntity, bool>>>(), CancellationToken.None)
            .Returns([new GmrEntity { Id = gmr, Gmr = new Gmr() }]);
        var result = await subject.GetGmrByChedId(chedRef, CancellationToken.None);

        result.Count.Should().Be(1);
        result[0].Id.Should().Be(gmr);
    }

    [Fact]
    public async Task Insert_ShouldInsertAndPublish()
    {
        var mockDbContext = Substitute.For<IDbContext>();
        var subject = new GmrService(mockDbContext);
        var entity = new GmrEntity { Id = "id", Gmr = new Gmr() };

        await subject.Insert(entity, CancellationToken.None);

        await mockDbContext.Gmrs.Received().Insert(entity, CancellationToken.None);
        await mockDbContext.Received().SaveChangesAsync(CancellationToken.None);
    }

    [Fact]
    public async Task Update_WhenNotExists_ShouldThrow()
    {
        var mockDbContext = Substitute.For<IDbContext>();
        var subject = new GmrService(mockDbContext);
        var entity = new GmrEntity { Id = "id", Gmr = new Gmr() };

        var act = async () => await subject.Update(entity, "etag", CancellationToken.None);

        await act.Should().ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async Task Update_ShouldUpdateAndPublish()
    {
        var mockDbContext = Substitute.For<IDbContext>();
        const string id = "id";
        mockDbContext
            .Gmrs.Find(id)
            .Returns(
                new GmrEntity
                {
                    Id = id,
                    Gmr = new Gmr { State = "OPEN" },
                }
            );
        var subject = new GmrService(mockDbContext);
        var entity = new GmrEntity
        {
            Id = id,
            Gmr = new Gmr { State = "COMPLETED" },
        };

        await subject.Update(entity, "etag", CancellationToken.None);

        await mockDbContext.Gmrs.Received().Update(entity, "etag", CancellationToken.None);
        await mockDbContext.Received().SaveChangesAsync(CancellationToken.None);
    }
}
