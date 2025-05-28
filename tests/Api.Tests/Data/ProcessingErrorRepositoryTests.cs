using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Api.Exceptions;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.ProcessingErrors;
using FluentAssertions;
using NSubstitute;

namespace Defra.TradeImportsDataApi.Api.Tests.Data;

public class ProcessingErrorRepositoryTests
{
    private IDbContext DbContext { get; }
    private ProcessingErrorRepository Subject { get; }

    public ProcessingErrorRepositoryTests()
    {
        DbContext = Substitute.For<IDbContext>();

        Subject = new ProcessingErrorRepository(DbContext);
    }

    [Fact]
    public async Task Update_WhenNotExists_ShouldThrow()
    {
        var entity = new ProcessingErrorEntity { Id = "id", ProcessingError = new ProcessingError() };

        var act = async () => await Subject.Update(entity, "etag", CancellationToken.None);

        await act.Should().ThrowAsync<EntityNotFoundException>();
    }
}
