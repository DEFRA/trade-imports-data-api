using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Api.Exceptions;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Gvms;
using FluentAssertions;
using NSubstitute;

namespace Defra.TradeImportsDataApi.Api.Tests.Data;

public class GmrRepositoryTests
{
    private IDbContext DbContext { get; }
    private GmrRepository Subject { get; }

    public GmrRepositoryTests()
    {
        DbContext = Substitute.For<IDbContext>();

        Subject = new GmrRepository(DbContext);
    }

    [Fact]
    public async Task Update_WhenNotExists_ShouldThrow()
    {
        var entity = new GmrEntity { Id = "id", Gmr = new Gmr() };

        var act = async () => await Subject.Update(entity, "etag", CancellationToken.None);

        await act.Should().ThrowAsync<EntityNotFoundException>();
    }
}
