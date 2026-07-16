using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using FluentAssertions;
using NSubstitute;
using Trade.Gateway.Api.Contract.Certificate;

namespace Defra.TradeImportsDataApi.Api.Tests.Services;

public class TracesChedServiceTests
{
    private IDbContext DbContext { get; }
    private ITracesChedRepository TracesChedRepository { get; }
    private IResourceEventRepository ResourceEventRepository { get; }
    private IResourceEventService ResourceEventService { get; }
    private TracesChedService Subject { get; }

    public TracesChedServiceTests()
    {
        DbContext = Substitute.For<IDbContext>();
        TracesChedRepository = Substitute.For<ITracesChedRepository>();
        ResourceEventRepository = Substitute.For<IResourceEventRepository>();
        ResourceEventService = Substitute.For<IResourceEventService>();

        Subject = new TracesChedService(DbContext, TracesChedRepository, ResourceEventRepository,
            ResourceEventService);
    }

    [Fact]
    public async Task Insert_ShouldInsertAndPublish()
    {
        var entity = new TracesChedEntity()
        {
            Id = "id",
            Ched = new DefraUNVTDCHEDProfile
            {
                ExchangedDocument = new ExchangedDocument() { Identifier = "Test" },
                SpecifiedConsignment = new Consignment(),
            },
        };
        TracesChedRepository.Insert(entity).Returns(entity);

        await Subject.Insert(entity, CancellationToken.None);

        await DbContext.Received(1).StartTransaction(CancellationToken.None);
        await DbContext.Received(1).SaveChanges(CancellationToken.None);
        await DbContext.Received(1).CommitTransaction(CancellationToken.None);

        TracesChedRepository.Received().Insert(entity);
    }

    [Fact]
    public async Task Update_ShouldUpdateAndPublish()
    {
        const string id = "id";
        var existing = new TracesChedEntity
        {
            Id = id,
            Ched = new DefraUNVTDCHEDProfile
            {
                Model = "Model1",
                ExchangedDocument = new ExchangedDocument() { Identifier = "Test" },
                SpecifiedConsignment = new Consignment(),
            },
        };
        TracesChedRepository.Get(id, CancellationToken.None).Returns(existing);
        var entity = new TracesChedEntity
        {
            Id = id,
            Ched = new DefraUNVTDCHEDProfile
            {
                Model = "Model2",
                ExchangedDocument = new ExchangedDocument() { Identifier = "Test" },
                SpecifiedConsignment = new Consignment(),
            },
        };
        TracesChedRepository.Update(entity, "etag", CancellationToken.None).Returns((existing, entity));

        await Subject.Update(entity, "etag", CancellationToken.None);

        await DbContext.Received(1).StartTransaction(CancellationToken.None);
        await DbContext.Received(1).SaveChanges(CancellationToken.None);
        await DbContext.Received(1).CommitTransaction(CancellationToken.None);

        await TracesChedRepository.Received().Update(entity, "etag", CancellationToken.None);
    }

    [Fact]
    public async Task GetImportPreNotification_ShouldReturn()
    {
        const string id = "id";
        TracesChedRepository
            .Get(id, CancellationToken.None)
            .Returns(
                new TracesChedEntity()
                {
                    Id = id,
                    Ched = new DefraUNVTDCHEDProfile
                    {
                        Model = "Model1",
                        ExchangedDocument = new ExchangedDocument() { Identifier = "Test" },
                        SpecifiedConsignment = new Consignment(),
                    },
                }
            );

        var result = await Subject.Get(id, CancellationToken.None);

        result.Should().NotBeNull();
    }
}
