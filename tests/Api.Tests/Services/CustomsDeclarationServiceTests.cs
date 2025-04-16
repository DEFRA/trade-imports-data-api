using Defra.TradeImportsDataApi.Api.Exceptions;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using Defra.TradeImportsDataApi.Domain.Events;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using FluentAssertions;
using NSubstitute;

namespace Defra.TradeImportsDataApi.Api.Tests.Services;

public class CustomsDeclarationServiceTests
{
    [Fact]
    public async Task Insert_ShouldInsertAndPublish()
    {
        var mockDbContext = Substitute.For<IDbContext>();
        var mockResourceEventPublisher = Substitute.For<IResourceEventPublisher>();
        var subject = new CustomsDeclarationService(mockDbContext, mockResourceEventPublisher);
        var entity = new CustomsDeclarationEntity { Id = "id", ClearanceRequest = new ClearanceRequest() };

        await subject.Insert(entity, CancellationToken.None);

        await mockDbContext.CustomsDeclarations.Received().Insert(entity, CancellationToken.None);
        await mockDbContext.Received().SaveChangesAsync(CancellationToken.None);
        await mockResourceEventPublisher
            .Received()
            .Publish(
                Arg.Is<ResourceEvent<CustomsDeclarationEntity>>(x => x.Operation == "Created"),
                CancellationToken.None
            );
    }

    [Fact]
    public async Task Update_WhenNotExists_ShouldThrow()
    {
        var mockDbContext = Substitute.For<IDbContext>();
        var mockResourceEventPublisher = Substitute.For<IResourceEventPublisher>();
        var subject = new CustomsDeclarationService(mockDbContext, mockResourceEventPublisher);
        var entity = new CustomsDeclarationEntity { Id = "id", ClearanceRequest = new ClearanceRequest() };

        var act = async () => await subject.Update(entity, "etag", CancellationToken.None);

        await act.Should().ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async Task Update_ShouldUpdateAndPublish()
    {
        var mockDbContext = Substitute.For<IDbContext>();
        const string id = "id";
        mockDbContext
            .CustomsDeclarations.Find(id)
            .Returns(
                new CustomsDeclarationEntity
                {
                    Id = id,
                    ClearanceRequest = new ClearanceRequest { ExternalVersion = 1 },
                }
            );
        var mockResourceEventPublisher = Substitute.For<IResourceEventPublisher>();
        var subject = new CustomsDeclarationService(mockDbContext, mockResourceEventPublisher);
        var entity = new CustomsDeclarationEntity
        {
            Id = id,
            ClearanceRequest = new ClearanceRequest { ExternalVersion = 2 },
        };

        await subject.Update(entity, "etag", CancellationToken.None);

        await mockDbContext.CustomsDeclarations.Received().Update(entity, "etag", CancellationToken.None);
        await mockDbContext.Received().SaveChangesAsync(CancellationToken.None);
        await mockResourceEventPublisher
            .Received()
            .Publish(
                Arg.Is<ResourceEvent<CustomsDeclarationEntity>>(x => x.Operation == "Updated" && x.ChangeSet.Count > 0),
                CancellationToken.None
            );
    }
}
