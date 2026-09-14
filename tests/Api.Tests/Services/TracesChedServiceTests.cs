using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Events;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using FluentAssertions;
using NSubstitute;
using Trade.Gateway.Api.Contract.Certificate;

namespace Defra.TradeImportsDataApi.Api.Tests.Services;

public class TracesChedServiceTests
{
    private IDbContext DbContext { get; }
    private ITracesChedRepository TracesChedRepository { get; }
    private ICustomsDeclarationRepository CustomsDeclarationRepository { get; }
    private IResourceEventRepository ResourceEventRepository { get; }
    private IResourceEventService ResourceEventService { get; }
    private TracesChedService Subject { get; }

    public TracesChedServiceTests()
    {
        DbContext = Substitute.For<IDbContext>();
        TracesChedRepository = Substitute.For<ITracesChedRepository>();
        CustomsDeclarationRepository = Substitute.For<ICustomsDeclarationRepository>();
        ResourceEventRepository = Substitute.For<IResourceEventRepository>();
        ResourceEventService = Substitute.For<IResourceEventService>();

        Subject = new TracesChedService(
            DbContext,
            TracesChedRepository,
            CustomsDeclarationRepository,
            ResourceEventRepository,
            ResourceEventService
        );
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

        var resourceEventEntityId = Guid.NewGuid().ToString();
        ResourceEventRepository
            .Insert(Arg.Any<ResourceEvent<TracesChedEvent>>())
            .Returns(call =>
            {
                var resourceEvent = call.Arg<ResourceEvent<TracesChedEvent>>();

                return new ResourceEventEntity
                {
                    Id = resourceEventEntityId,
                    ResourceId = resourceEvent!.ResourceId,
                    ResourceType = resourceEvent.ResourceType,
                    SubResourceType = resourceEvent.SubResourceType,
                    Operation = resourceEvent.Operation,
                    Message = "message body",
                };
            });

        await Subject.Insert(entity, CancellationToken.None);

        await DbContext.Received(1).StartTransaction(CancellationToken.None);
        await DbContext.Received(1).SaveChanges(CancellationToken.None);
        await DbContext.Received(1).CommitTransaction(CancellationToken.None);

        TracesChedRepository.Received().Insert(entity);
        ResourceEventRepository
            .Received()
            .Insert(Arg.Is<ResourceEvent<TracesChedEvent>>(x => x!.Operation == "Created" && x.ChangeSet.Count == 0));
        await ResourceEventService
            .Received()
            .Publish(Arg.Is<ResourceEventEntity>(x => x!.Id == resourceEventEntityId), CancellationToken.None);
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
        var resourceEventEntityId = Guid.NewGuid().ToString();
        ResourceEventRepository
            .Insert(Arg.Any<ResourceEvent<TracesChedEvent>>())
            .Returns(call =>
            {
                var resourceEvent = call.Arg<ResourceEvent<TracesChedEvent>>();

                return new ResourceEventEntity
                {
                    Id = resourceEventEntityId,
                    ResourceId = resourceEvent!.ResourceId,
                    ResourceType = resourceEvent.ResourceType,
                    SubResourceType = resourceEvent.SubResourceType,
                    Operation = resourceEvent.Operation,
                    Message = "message body",
                };
            });
        await Subject.Update(entity, "etag", CancellationToken.None);

        await DbContext.Received(1).StartTransaction(CancellationToken.None);
        await DbContext.Received(1).SaveChanges(CancellationToken.None);
        await DbContext.Received(1).CommitTransaction(CancellationToken.None);

        await TracesChedRepository.Received().Update(entity, "etag", CancellationToken.None);

        ResourceEventRepository
            .Received()
            .Insert(Arg.Is<ResourceEvent<TracesChedEvent>>(x => x!.Operation == "Updated"));
        await ResourceEventService
            .Received()
            .Publish(Arg.Is<ResourceEventEntity>(x => x!.Id == resourceEventEntityId), CancellationToken.None);
    }

    [Fact]
    public async Task GetChed_ShouldReturn()
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

    [Fact]
    public async Task GetUpdates_ShouldReturn()
    {
        var query = new TracesChedUpdateQuery(
            new DateTime(2025, 5, 21, 8, 0, 0, DateTimeKind.Utc),
            new DateTime(2025, 5, 21, 9, 0, 0, DateTimeKind.Utc)
        );
        var updates = new TracesChedUpdates(
            [new TracesChedUpdate("CHEDA.GB.2026.1234567", new DateTime(2025, 5, 21, 8, 51, 0, DateTimeKind.Utc))],
            Total: 1
        );
        TracesChedRepository.GetUpdates(query, CancellationToken.None).Returns(updates);

        var result = await Subject.GetUpdates(query, CancellationToken.None);

        result.Should().BeSameAs(updates);
    }

    [Fact]
    public async Task GetChedByMrn_ShouldReturn()
    {
        const string id = "id";
        const string mrn = "mrn";
        var identifiers = new List<string> { "CHEDA.GB.2026.1234567" };
        CustomsDeclarationRepository
            .GetAllImportPreNotificationIdentifiers(mrn, CancellationToken.None)
            .Returns(identifiers);
        TracesChedRepository
            .GetAll(Arg.Is<string[]>(x => x!.SequenceEqual(identifiers)), CancellationToken.None)
            .Returns([
                new TracesChedEntity()
                {
                    Id = id,
                    Ched = new DefraUNVTDCHEDProfile
                    {
                        Model = "Model1",
                        ExchangedDocument = new ExchangedDocument() { Identifier = "CHEDA.GB.2026.1234567" },
                        SpecifiedConsignment = new Consignment(),
                    },
                },
            ]);

        var result = await Subject.GetByMrn(mrn, CancellationToken.None);

        result.Should().NotBeEmpty();
    }
}
