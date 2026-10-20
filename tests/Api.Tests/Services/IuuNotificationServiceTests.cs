using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Events;
using Defra.TradeImportsDataApi.Domain.Iuu;
using FluentAssertions;
using NSubstitute;

namespace Defra.TradeImportsDataApi.Api.Tests.Services;

public class IuuNotificationServiceTests
{
    private IDbContext DbContext { get; }
    private IIuuNotificationRepository IuuNotificationRepository { get; }
    private IResourceEventRepository ResourceEventRepository { get; }
    private IResourceEventService ResourceEventService { get; }
    private IuuNotificationService Subject { get; }

    public IuuNotificationServiceTests()
    {
        DbContext = Substitute.For<IDbContext>();
        IuuNotificationRepository = Substitute.For<IIuuNotificationRepository>();
        ResourceEventRepository = Substitute.For<IResourceEventRepository>();
        ResourceEventService = Substitute.For<IResourceEventService>();

        Subject = new IuuNotificationService(
            DbContext,
            IuuNotificationRepository,
            ResourceEventRepository,
            ResourceEventService
        );
    }

    private static IuuNotification CreateIuuNotification(string iuuOption = "IUUOK", DateTime? lastUpdated = null) =>
        new()
        {
            Reference = "IUU.Reference",
            LastUpdated = lastUpdated ?? new DateTime(2026, 10, 16, 9, 0, 0, DateTimeKind.Utc),
            IuuOption = iuuOption,
        };

    private string SetupResourceEventRepository()
    {
        var resourceEventEntityId = Guid.NewGuid().ToString();
        ResourceEventRepository
            .Insert(Arg.Any<ResourceEvent<IuuNotificationEvent>>())
            .Returns(call =>
            {
                var resourceEvent = call.Arg<ResourceEvent<IuuNotificationEvent>>();

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

        return resourceEventEntityId;
    }

    [Fact]
    public async Task Insert_ShouldInsertAndPublish()
    {
        var entity = new IuuNotificationEntity { Id = "IUU.Reference", IuuNotification = CreateIuuNotification() };
        IuuNotificationRepository.Insert(entity).Returns(entity);
        var resourceEventEntityId = SetupResourceEventRepository();

        var result = await Subject.Insert(entity, CancellationToken.None);

        result.Should().BeSameAs(entity);

        await DbContext.Received(1).StartTransaction(CancellationToken.None);
        await DbContext.Received(1).SaveChanges(CancellationToken.None);
        await DbContext.Received(1).CommitTransaction(CancellationToken.None);

        IuuNotificationRepository.Received().Insert(entity);
        ResourceEventRepository
            .Received()
            .Insert(
                Arg.Is<ResourceEvent<IuuNotificationEvent>>(x =>
                    x!.Operation == ResourceEventOperations.Created
                    && x.ResourceId == entity.Id
                    && x.ResourceType == ResourceEventResourceTypes.IuuNotification
                    && x.ChangeSet.Count == 0
                    && x.Resource!.IuuNotification == entity.IuuNotification
                )
            );
        await ResourceEventService
            .Received()
            .Publish(Arg.Is<ResourceEventEntity>(x => x!.Id == resourceEventEntityId), CancellationToken.None);
    }

    [Fact]
    public async Task Update_ShouldUpdateAndPublish()
    {
        const string iuuReference = "IUU.Reference";
        var existing = new IuuNotificationEntity { Id = iuuReference, IuuNotification = CreateIuuNotification() };
        var entity = new IuuNotificationEntity
        {
            Id = iuuReference,
            IuuNotification = CreateIuuNotification("IUUNA", new DateTime(2026, 10, 16, 9, 15, 0, DateTimeKind.Utc)),
        };
        IuuNotificationRepository.Update(entity, "etag", CancellationToken.None).Returns((existing, entity));
        var resourceEventEntityId = SetupResourceEventRepository();

        var result = await Subject.Update(entity, "etag", CancellationToken.None);

        result.Should().BeSameAs(entity);

        await DbContext.Received(1).StartTransaction(CancellationToken.None);
        await DbContext.Received(1).SaveChanges(CancellationToken.None);
        await DbContext.Received(1).CommitTransaction(CancellationToken.None);

        await IuuNotificationRepository.Received().Update(entity, "etag", CancellationToken.None);
        ResourceEventRepository
            .Received()
            .Insert(
                Arg.Is<ResourceEvent<IuuNotificationEvent>>(x =>
                    x!.Operation == ResourceEventOperations.Updated
                    && x.ResourceId == iuuReference
                    && x.ResourceType == ResourceEventResourceTypes.IuuNotification
                    && x.Resource!.IuuNotification.IuuOption == "IUUNA"
                )
            );
        await ResourceEventService
            .Received()
            .Publish(Arg.Is<ResourceEventEntity>(x => x!.Id == resourceEventEntityId), CancellationToken.None);
    }

    [Fact]
    public async Task Update_WhenRepositoryThrows_ShouldNotPublish()
    {
        var entity = new IuuNotificationEntity { Id = "IUU.Reference", IuuNotification = CreateIuuNotification() };
        IuuNotificationRepository
            .Update(entity, "etag", CancellationToken.None)
            .Returns<(IuuNotificationEntity, IuuNotificationEntity)>(_ => throw new InvalidOperationException());

        var act = async () => await Subject.Update(entity, "etag", CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>();

        await DbContext.DidNotReceive().CommitTransaction(Arg.Any<CancellationToken>());
        ResourceEventRepository.DidNotReceive().Insert(Arg.Any<ResourceEvent<IuuNotificationEvent>>());
        await ResourceEventService
            .DidNotReceive()
            .Publish(Arg.Any<ResourceEventEntity>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetIuuNotification_ShouldGet()
    {
        const string iuuReference = "IUU.Reference";
        IuuNotificationRepository
            .Get(iuuReference, CancellationToken.None)
            .Returns(new IuuNotificationEntity { Id = iuuReference, IuuNotification = CreateIuuNotification() });

        var result = await Subject.Get(iuuReference, CancellationToken.None);

        result.Should().NotBeNull();

        await IuuNotificationRepository.Received().Get(iuuReference, CancellationToken.None);
    }

    [Fact]
    public async Task GetIuuNotification_WhenNotFound_ShouldReturnNull()
    {
        const string iuuReference = "IUU.Reference";
        IuuNotificationRepository.Get(iuuReference, CancellationToken.None).Returns((IuuNotificationEntity?)null);

        var result = await Subject.Get(iuuReference, CancellationToken.None);

        result.Should().BeNull();
    }
}
