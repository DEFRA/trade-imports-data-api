using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Errors;
using Defra.TradeImportsDataApi.Domain.Events;
using FluentAssertions;
using NSubstitute;

namespace Defra.TradeImportsDataApi.Api.Tests.Services;

public class ProcessingErrorServiceTests
{
    private IDbContext DbContext { get; }
    private IProcessingErrorRepository ProcessingErrorRepository { get; }
    private IResourceEventRepository ResourceEventRepository { get; }
    private IResourceEventService ResourceEventService { get; }
    private ProcessingErrorService Subject { get; }

    public ProcessingErrorServiceTests()
    {
        DbContext = Substitute.For<IDbContext>();
        ProcessingErrorRepository = Substitute.For<IProcessingErrorRepository>();
        ResourceEventRepository = Substitute.For<IResourceEventRepository>();
        ResourceEventService = Substitute.For<IResourceEventService>();

        Subject = new ProcessingErrorService(
            DbContext,
            ProcessingErrorRepository,
            ResourceEventRepository,
            ResourceEventService
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
                    SubResourceType = resourceEvent.SubResourceType,
                    Operation = resourceEvent.Operation,
                    Message = "message body",
                };
            });

        await Subject.Insert(entity, CancellationToken.None);

        await DbContext.Received(1).StartTransaction(CancellationToken.None);
        await DbContext.Received(1).SaveChanges(CancellationToken.None);
        await DbContext.Received(1).CommitTransaction(CancellationToken.None);

        ProcessingErrorRepository.Received().Insert(entity);
        ResourceEventRepository
            .Received()
            .Insert(
                Arg.Is<ResourceEvent<ProcessingErrorEntity>>(x => x.Operation == "Created" && x.ChangeSet.Count > 0)
            );
        await ResourceEventService
            .Received()
            .Publish(Arg.Is<ResourceEventEntity>(x => x.Id == resourceEventEntityId), CancellationToken.None);
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
                    SubResourceType = resourceEvent.SubResourceType,
                    Operation = resourceEvent.Operation,
                    Message = "message body",
                };
            });

        await Subject.Update(entity, "etag", CancellationToken.None);

        await DbContext.Received(1).StartTransaction(CancellationToken.None);
        await DbContext.Received(1).SaveChanges(CancellationToken.None);
        await DbContext.Received(1).CommitTransaction(CancellationToken.None);

        await ProcessingErrorRepository.Received().Update(entity, "etag", CancellationToken.None);
        ResourceEventRepository
            .Received()
            .Insert(
                Arg.Is<ResourceEvent<ProcessingErrorEntity>>(x => x.Operation == "Updated" && x.ChangeSet.Count > 0)
            );
        await ResourceEventService
            .Received()
            .Publish(Arg.Is<ResourceEventEntity>(x => x.Id == resourceEventEntityId), CancellationToken.None);
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
