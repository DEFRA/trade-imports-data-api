using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Api.Exceptions;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using FluentAssertions;
using NSubstitute;
using Trade.Gateway.Api.Contract.Certificate;

namespace Defra.TradeImportsDataApi.Api.Tests.Data;

public class TracesChedRepositoryTests
{
    private IDbContext DbContext { get; }
    private TracesChedRepository Subject { get; }

    public TracesChedRepositoryTests()
    {
        DbContext = Substitute.For<IDbContext>();

        Subject = new TracesChedRepository(DbContext);
    }

    [Fact]
    public async Task Update_WhenNotExists_ShouldThrow()
    {
        var entity = new TracesChedEntity()
        {
            Id = "id",
            Ched = new DefraUNVTDCHEDProfile
            {
                Model = "Model1",
                ExchangedDocument = new ExchangedDocument() { Identifier = "Test" },
                SpecifiedConsignment = new Consignment(),
            },
        };

        var act = async () => await Subject.Update(entity, "etag", CancellationToken.None);

        await act.Should().ThrowAsync<EntityNotFoundException>();
    }
}
