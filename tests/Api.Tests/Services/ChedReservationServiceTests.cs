using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Events;
using Defra.TradeImportsDataApi.Domain.Traces;
using FluentAssertions;
using NSubstitute;

namespace Defra.TradeImportsDataApi.Api.Tests.Services;

public class ChedReservationServiceTests
{
    private IDbContext DbContext { get; }
    private IChedReservationRepository ChedReservationRepository { get; }
    private IResourceEventRepository ResourceEventRepository { get; }
    private IResourceEventService ResourceEventService { get; }
    private ChedReservationService Subject { get; }

    public ChedReservationServiceTests()
    {
        DbContext = Substitute.For<IDbContext>();
        ChedReservationRepository = Substitute.For<IChedReservationRepository>();
        ResourceEventRepository = Substitute.For<IResourceEventRepository>();
        ResourceEventService = Substitute.For<IResourceEventService>();

        Subject = new ChedReservationService(
            DbContext,
            ChedReservationRepository,
            ResourceEventRepository,
            ResourceEventService
        );
    }

    private static Reservation CreateReservation(
        string chedId = "chedId",
        string mrn = "mrn",
        string status = "Reserved"
    ) =>
        new()
        {
            ChedId = chedId,
            Mrn = mrn,
            Status = status,
            Timestamp = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
            Commodities =
            [
                new ReservationCommodity
                {
                    GoodsItemNumber = 1,
                    CommodityCode = "123",
                    CertificateLineNumber = 1,
                    UnitOfMeasure = "KGM",
                    Quantity = 100,
                },
            ],
        };

    [Fact]
    public async Task Upsert_WhenNoEtag_ShouldInsertAndPublish()
    {
        var entity = new ChedReservationEntity { Id = "chedId_mrn", Reservation = CreateReservation() };
        ChedReservationRepository.Insert(entity).Returns(entity);

        var resourceEventEntityId = Guid.NewGuid().ToString();
        ResourceEventRepository
            .Insert(Arg.Any<ResourceEvent<ChedReservationEvent>>())
            .Returns(call =>
            {
                var resourceEvent = call.Arg<ResourceEvent<ChedReservationEvent>>();

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

        await Subject.Upsert(entity, null, CancellationToken.None);

        await DbContext.Received(1).StartTransaction(CancellationToken.None);
        await DbContext.Received(1).SaveChanges(CancellationToken.None);
        await DbContext.Received(1).CommitTransaction(CancellationToken.None);

        ChedReservationRepository.Received().Insert(entity);
        ResourceEventRepository
            .Received()
            .Insert(
                Arg.Is<ResourceEvent<ChedReservationEvent>>(x => x!.Operation == "Created" && x.ChangeSet.Count == 0)
            );
        await ResourceEventService
            .Received()
            .Publish(Arg.Is<ResourceEventEntity>(x => x!.Id == resourceEventEntityId), CancellationToken.None);
    }

    [Fact]
    public async Task Upsert_WhenEtagProvided_ShouldUpdateAndPublish()
    {
        const string id = "chedId_mrn";
        var existing = new ChedReservationEntity { Id = id, Reservation = CreateReservation(status: "Reserved") };
        var entity = new ChedReservationEntity { Id = id, Reservation = CreateReservation(status: "Cancelled") };
        ChedReservationRepository.Update(entity, "etag", CancellationToken.None).Returns((existing, entity));

        var resourceEventEntityId = Guid.NewGuid().ToString();
        ResourceEventRepository
            .Insert(Arg.Any<ResourceEvent<ChedReservationEvent>>())
            .Returns(call =>
            {
                var resourceEvent = call.Arg<ResourceEvent<ChedReservationEvent>>();

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

        await Subject.Upsert(entity, "etag", CancellationToken.None);

        await DbContext.Received(1).StartTransaction(CancellationToken.None);
        await DbContext.Received(1).SaveChanges(CancellationToken.None);
        await DbContext.Received(1).CommitTransaction(CancellationToken.None);

        await ChedReservationRepository.Received().Update(entity, "etag", CancellationToken.None);
        ResourceEventRepository
            .Received()
            .Insert(Arg.Is<ResourceEvent<ChedReservationEvent>>(x => x!.Operation == "Updated"));
        await ResourceEventService
            .Received()
            .Publish(Arg.Is<ResourceEventEntity>(x => x!.Id == resourceEventEntityId), CancellationToken.None);
    }

    [Fact]
    public async Task Get_ShouldReturn()
    {
        const string id = "chedId_mrn";
        ChedReservationRepository
            .GetAll(Arg.Is<string[]>(x => x.SequenceEqual(new[] { id })), CancellationToken.None)
            .Returns([new ChedReservationEntity { Id = id, Reservation = CreateReservation() }]);

        var result = await Subject.Get(id, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
    }

    [Fact]
    public async Task Get_WhenNotFound_ShouldReturnNull()
    {
        const string id = "chedId_mrn";
        ChedReservationRepository
            .GetAll(Arg.Is<string[]>(x => x.SequenceEqual(new[] { id })), CancellationToken.None)
            .Returns([]);

        var result = await Subject.Get(id, CancellationToken.None);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByChedId_ShouldReturn()
    {
        const string chedId = "chedId";
        ChedReservationRepository
            .GetByChedIds(Arg.Is<string[]>(x => x.SequenceEqual(new[] { chedId })), CancellationToken.None)
            .Returns([
                new ChedReservationEntity { Id = "chedId_mrn", Reservation = CreateReservation(chedId: chedId) },
            ]);

        var result = await Subject.GetByChedId(chedId, CancellationToken.None);

        result.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Delete_ShouldDeleteById()
    {
        const string chedId = "chedId";
        const string mrn = "mrn";

        await Subject.Delete(chedId, mrn, CancellationToken.None);

        await ChedReservationRepository.Received().DeleteById($"{chedId}_{mrn}", CancellationToken.None);
    }
}
