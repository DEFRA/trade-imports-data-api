using System.Linq.Expressions;
using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Errors;
using Defra.TradeImportsDataApi.Domain.Events;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Defra.TradeImportsDataApi.Api.Tests.Services;

public class ProcessingErrorServiceTests
{
    private IDbContext DbContext { get; }
    private IResourceEventPublisher ResourceEventPublisher { get; }
    private IProcessingErrorRepository ProcessingErrorRepository { get; }
    private IResourceEventRepository ResourceEventRepository { get; }
    private ProcessingErrorService Subject { get; }

    public ProcessingErrorServiceTests()
    {
        DbContext = Substitute.For<IDbContext>();
        ResourceEventPublisher = Substitute.For<IResourceEventPublisher>();
        ProcessingErrorRepository = Substitute.For<IProcessingErrorRepository>();
        ResourceEventRepository = Substitute.For<IResourceEventRepository>();

        Subject = new ProcessingErrorService(
            DbContext,
            ResourceEventPublisher,
            ProcessingErrorRepository,
            ResourceEventRepository,
            NullLogger<ProcessingErrorService>.Instance
        );
    }

    [Fact]
    public async Task Insert_ShouldInsertAndPublish()
    {
        var entity = new ProcessingErrorEntity
        {
            Id = "id",
            ProcessingErrors = [new ProcessingError { ExternalVersion = 1 }],
        };
        ProcessingErrorRepository.Insert(entity).Returns(entity);
        var resourceEventEntityId = Guid.NewGuid().ToString();
        ResourceEventRepository
            .Insert(Arg.Any<ResourceEvent<ProcessingErrorEntity>>())
            .Returns(call =>
            {
                var resourceEvent = call.Arg<ResourceEvent<ProcessingErrorEntity>>();

                return new ResourceEventEntity
                {
                    Id = resourceEventEntityId,
                    ResourceId = resourceEvent.ResourceId,
                    ResourceType = resourceEvent.ResourceType,
                    Message = "message body",
                };
            });

        await Subject.Insert(entity, CancellationToken.None);

        await DbContext.Received(2).StartTransaction(CancellationToken.None);
        await DbContext.Received(2).SaveChanges(CancellationToken.None);
        await DbContext.Received(2).CommitTransaction(CancellationToken.None);

        ProcessingErrorRepository.Received().Insert(entity);
        Expression<Predicate<ResourceEvent<ProcessingErrorEntity>>> assertion = x =>
            x.Operation == "Created" && x.ChangeSet.Count > 0;
        await ResourceEventPublisher.Received().Publish(Arg.Is(assertion), CancellationToken.None);
        ResourceEventRepository.Received().Insert(Arg.Is(assertion));
        ResourceEventRepository
            .Received()
            .Update(Arg.Is<ResourceEventEntity>(x => x.Id == resourceEventEntityId && x.Published != null));
    }

    [Fact]
    public async Task Update_ShouldUpdateAndPublish()
    {
        const string id = "id";
        var existing = new ProcessingErrorEntity
        {
            Id = "id",
            ProcessingErrors = [new ProcessingError { ExternalVersion = 1 }],
        };
        ProcessingErrorRepository.Get(id, CancellationToken.None).Returns(existing);
        var entity = new ProcessingErrorEntity
        {
            Id = "id",
            ProcessingErrors =
            [
                new ProcessingError { ExternalVersion = 1 },
                new ProcessingError { ExternalVersion = 2 },
            ],
        };
        ProcessingErrorRepository.Update(entity, "etag", CancellationToken.None).Returns((existing, entity));
        var resourceEventEntityId = Guid.NewGuid().ToString();
        ResourceEventRepository
            .Insert(Arg.Any<ResourceEvent<ProcessingErrorEntity>>())
            .Returns(call =>
            {
                var resourceEvent = call.Arg<ResourceEvent<ProcessingErrorEntity>>();

                return new ResourceEventEntity
                {
                    Id = resourceEventEntityId,
                    ResourceId = resourceEvent.ResourceId,
                    ResourceType = resourceEvent.ResourceType,
                    Message = "message body",
                };
            });

        await Subject.Update(entity, "etag", CancellationToken.None);

        await DbContext.Received(2).StartTransaction(CancellationToken.None);
        await DbContext.Received(2).SaveChanges(CancellationToken.None);
        await DbContext.Received(2).CommitTransaction(CancellationToken.None);

        await ProcessingErrorRepository.Received().Update(entity, "etag", CancellationToken.None);
        Expression<Predicate<ResourceEvent<ProcessingErrorEntity>>> assertion = x =>
            x.Operation == "Updated" && x.ChangeSet.Count > 0;
        await ResourceEventPublisher.Received().Publish(Arg.Is(assertion), CancellationToken.None);
        ResourceEventRepository.Received().Insert(Arg.Is(assertion));
        ResourceEventRepository
            .Received()
            .Update(Arg.Is<ResourceEventEntity>(x => x.Id == resourceEventEntityId && x.Published != null));
    }

    [Fact]
    public async Task Update_PublishThrows_ShouldNotError()
    {
        const string id = "id";
        var existing = new ProcessingErrorEntity
        {
            Id = "id",
            ProcessingErrors = [new ProcessingError { ExternalVersion = 1 }],
        };
        ProcessingErrorRepository.Get(id, CancellationToken.None).Returns(existing);
        var entity = new ProcessingErrorEntity
        {
            Id = "id",
            ProcessingErrors =
            [
                new ProcessingError { ExternalVersion = 1 },
                new ProcessingError { ExternalVersion = 2 },
            ],
        };
        ProcessingErrorRepository.Update(entity, "etag", CancellationToken.None).Returns((existing, entity));
        var resourceEventEntityId = Guid.NewGuid().ToString();
        ResourceEventRepository
            .Insert(Arg.Any<ResourceEvent<ProcessingErrorEntity>>())
            .Returns(call =>
            {
                var resourceEvent = call.Arg<ResourceEvent<ProcessingErrorEntity>>();

                return new ResourceEventEntity
                {
                    Id = resourceEventEntityId,
                    ResourceId = resourceEvent.ResourceId,
                    ResourceType = resourceEvent.ResourceType,
                    Message = "message body",
                };
            });
        ResourceEventPublisher
            .Publish(Arg.Any<ResourceEvent<ProcessingErrorEntity>>(), CancellationToken.None)
            .Throws(new Exception());

        await Subject.Update(entity, "etag", CancellationToken.None);

        await DbContext.Received(2).StartTransaction(CancellationToken.None);

        // Transaction should be started but it will never be committed
        await DbContext.Received(1).SaveChanges(CancellationToken.None);
        await DbContext.Received(1).CommitTransaction(CancellationToken.None);

        ResourceEventRepository.DidNotReceiveWithAnyArgs().Update(Arg.Any<ResourceEventEntity>());
    }

    [Fact]
    public async Task GetCustomsDeclaration_ShouldReturn()
    {
        const string id = "id";
        ProcessingErrorRepository
            .Get(id, CancellationToken.None)
            .Returns(new ProcessingErrorEntity { Id = id, ProcessingErrors = [] });

        var result = await Subject.GetProcessingError(id, CancellationToken.None);

        result.Should().NotBeNull();
    }
}
