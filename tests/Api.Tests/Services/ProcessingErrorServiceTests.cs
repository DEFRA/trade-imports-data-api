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
    private IResourceEventPublisher ResourceEventPublisher { get; }
    private IProcessingErrorRepository ProcessingErrorRepository { get; }
    private ProcessingErrorService Subject { get; }

    public ProcessingErrorServiceTests()
    {
        DbContext = Substitute.For<IDbContext>();
        ResourceEventPublisher = Substitute.For<IResourceEventPublisher>();
        ProcessingErrorRepository = Substitute.For<IProcessingErrorRepository>();

        Subject = new ProcessingErrorService(DbContext, ResourceEventPublisher, ProcessingErrorRepository);
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

        await Subject.Insert(entity, CancellationToken.None);

        await DbContext.Received().StartTransaction(CancellationToken.None);
        ProcessingErrorRepository.Received().Insert(entity);
        await DbContext.Received().SaveChanges(CancellationToken.None);
        await ResourceEventPublisher
            .Received()
            .Publish(
                Arg.Is<ResourceEvent<ProcessingErrorEntity>>(x => x.Operation == "Created" && x.ChangeSet.Count > 0),
                CancellationToken.None
            );
        await DbContext.Received().CommitTransaction(CancellationToken.None);
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

        await Subject.Update(entity, "etag", CancellationToken.None);

        await DbContext.Received().StartTransaction(CancellationToken.None);
        await ProcessingErrorRepository.Received().Update(entity, "etag", CancellationToken.None);
        await DbContext.Received().SaveChanges(CancellationToken.None);
        await ResourceEventPublisher
            .Received()
            .Publish(
                Arg.Is<ResourceEvent<ProcessingErrorEntity>>(x => x.Operation == "Updated" && x.ChangeSet.Count > 0),
                CancellationToken.None
            );
        await DbContext.Received().CommitTransaction(CancellationToken.None);
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
