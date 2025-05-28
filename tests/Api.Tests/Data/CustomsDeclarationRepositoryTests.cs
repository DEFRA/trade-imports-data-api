using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Api.Exceptions;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using FluentAssertions;
using NSubstitute;

namespace Defra.TradeImportsDataApi.Api.Tests.Data;

public class CustomsDeclarationRepositoryTests
{
    private IDbContext DbContext { get; }
    private CustomsDeclarationRepository Subject { get; }

    public CustomsDeclarationRepositoryTests()
    {
        DbContext = Substitute.For<IDbContext>();

        Subject = new CustomsDeclarationRepository(DbContext);
    }

    [Fact]
    public async Task Update_WhenNotExists_ShouldThrow()
    {
        var entity = new CustomsDeclarationEntity { Id = "id", ClearanceRequest = new ClearanceRequest() };

        var act = async () => await Subject.Update(entity, "etag", CancellationToken.None);

        await act.Should().ThrowAsync<EntityNotFoundException>();
    }
}
