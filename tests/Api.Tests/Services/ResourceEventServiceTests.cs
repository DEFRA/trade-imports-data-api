using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Defra.TradeImportsDataApi.Api.Tests.Services;

public class ResourceEventServiceTests
{
    private IDbContext DbContext { get; }
    private IResourceEventRepository ResourceEventRepository { get; }
    private IResourceEventPublisher ResourceEventPublisher { get; }
    private ResourceEventService Subject { get; }

    public ResourceEventServiceTests()
    {
        DbContext = Substitute.For<IDbContext>();
        ResourceEventRepository = Substitute.For<IResourceEventRepository>();
        ResourceEventPublisher = Substitute.For<IResourceEventPublisher>();

        Subject = new ResourceEventService(
            DbContext,
            ResourceEventRepository,
            ResourceEventPublisher,
            NullLogger<ResourceEventService>.Instance
        );
    }

    [Fact]
    public async Task Publish_ShouldPublish()
    {
        var entity = new ResourceEventEntity
        {
            Id = "id",
            ResourceId = "resourceId",
            ResourceType = "resourceType",
            Operation = "operation",
            Message = "message",
        };

        await Subject.Publish(entity, CancellationToken.None);

        await DbContext.Received(1).StartTransaction(CancellationToken.None);
        await ResourceEventPublisher.Received(1).Publish(entity, CancellationToken.None);
        entity.Published.Should().NotBeNull();
        ResourceEventRepository.Received(1).Update(entity);
        await DbContext.Received(1).SaveChanges(CancellationToken.None);
        await DbContext.Received(1).CommitTransaction(CancellationToken.None);
    }

    [Fact]
    public async Task PublishAllowException_ShouldPublish()
    {
        var entity = new ResourceEventEntity
        {
            Id = "id",
            ResourceId = "resourceId",
            ResourceType = "resourceType",
            Operation = "operation",
            Message = "message",
        };

        await Subject.PublishAllowException(entity, CancellationToken.None);

        await DbContext.Received(1).StartTransaction(CancellationToken.None);
        await ResourceEventPublisher.Received(1).Publish(entity, CancellationToken.None);
        entity.Published.Should().NotBeNull();
        ResourceEventRepository.Received(1).Update(entity);
        await DbContext.Received(1).SaveChanges(CancellationToken.None);
        await DbContext.Received(1).CommitTransaction(CancellationToken.None);
    }

    [Fact]
    public async Task Publish_WhenCancelled_Throws()
    {
        var entity = new ResourceEventEntity
        {
            Id = "id",
            ResourceId = "resourceId",
            ResourceType = "resourceType",
            Operation = "operation",
            Message = "message",
        };
        ResourceEventPublisher
            .Publish(Arg.Any<ResourceEventEntity>(), CancellationToken.None)
            .Throws(new OperationCanceledException());

        var act = async () => await Subject.Publish(entity, CancellationToken.None);

        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task Publish_WhenAnyOtherException_DoesNotThrow()
    {
        var entity = new ResourceEventEntity
        {
            Id = "id",
            ResourceId = "resourceId",
            ResourceType = "resourceType",
            Operation = "operation",
            Message = "message",
        };
        ResourceEventPublisher.Publish(Arg.Any<ResourceEventEntity>(), CancellationToken.None).Throws(new Exception());

        var act = async () => await Subject.Publish(entity, CancellationToken.None);

        await act.Should().NotThrowAsync<Exception>();
    }

    [Fact]
    public async Task PublishAllowException_WhenException_Throws()
    {
        var entity = new ResourceEventEntity
        {
            Id = "id",
            ResourceId = "resourceId",
            ResourceType = "resourceType",
            Operation = "operation",
            Message = "message",
        };
        ResourceEventPublisher.Publish(Arg.Any<ResourceEventEntity>(), CancellationToken.None).Throws(new Exception());

        var act = async () => await Subject.PublishAllowException(entity, CancellationToken.None);

        await act.Should().ThrowAsync<Exception>();
    }
}
