using Defra.TradeImportsDataApi.Api.Exceptions;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Gvms;
using FluentAssertions;
using NSubstitute;

namespace Defra.TradeImportsDataApi.Api.Tests.Services;

public class GmrServiceTests
{
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
                    Gmr = new Gmr { State = State.Open },
                }
            );
        var subject = new GmrService(mockDbContext);
        var entity = new GmrEntity
        {
            Id = id,
            Gmr = new Gmr { State = State.Completed },
        };

        await subject.Update(entity, "etag", CancellationToken.None);

        await mockDbContext.Gmrs.Received().Update(entity, "etag", CancellationToken.None);
        await mockDbContext.Received().SaveChangesAsync(CancellationToken.None);
    }
}
