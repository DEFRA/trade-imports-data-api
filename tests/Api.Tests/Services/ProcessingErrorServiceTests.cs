using Defra.TradeImportsDataApi.Api.Exceptions;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Errors;
using Defra.TradeImportsDataApi.Domain.Events;
using Defra.TradeImportsDataApi.Domain.ProcessingErrors;
using FluentAssertions;
using NSubstitute;

namespace Defra.TradeImportsDataApi.Api.Tests.Services;

public class ProcessingErrorServiceTests
{
    [Fact]
    public async Task Insert_ShouldInsertAndPublish()
    {
        var mockDbContext = Substitute.For<IDbContext>();
        var mockResourceEventPublisher = Substitute.For<IResourceEventPublisher>();
        var subject = new ProcessingErrorService(mockDbContext, mockResourceEventPublisher);
        var entity = new ProcessingErrorEntity
        {
            Id = "id",
            ProcessingError = new ProcessingError { Notifications = [new ErrorNotification { ExternalVersion = 1 }] },
        };

        await subject.Insert(entity, CancellationToken.None);

        await mockDbContext.ProcessingErrors.Received().Insert(entity, CancellationToken.None);
        await mockDbContext.Received().SaveChangesAsync(CancellationToken.None);
        await mockResourceEventPublisher
            .Received()
            .Publish(
                Arg.Is<ResourceEvent<ProcessingErrorEntity>>(x => x.Operation == "Created" && x.ChangeSet.Count > 0),
                CancellationToken.None
            );
    }

    [Fact]
    public async Task Update_WhenNotExists_ShouldThrow()
    {
        var mockDbContext = Substitute.For<IDbContext>();
        var mockResourceEventPublisher = Substitute.For<IResourceEventPublisher>();
        var subject = new ProcessingErrorService(mockDbContext, mockResourceEventPublisher);
        var entity = new ProcessingErrorEntity { Id = "id", ProcessingError = new ProcessingError() };

        var act = async () => await subject.Update(entity, "etag", CancellationToken.None);

        await act.Should().ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async Task Update_ShouldUpdateAndPublish()
    {
        var mockDbContext = Substitute.For<IDbContext>();
        const string id = "id";
        mockDbContext
            .ProcessingErrors.Find(id)
            .Returns(
                new ProcessingErrorEntity
                {
                    Id = "id",
                    ProcessingError = new ProcessingError
                    {
                        Notifications = [new ErrorNotification { ExternalVersion = 1 }],
                    },
                }
            );
        var mockResourceEventPublisher = Substitute.For<IResourceEventPublisher>();
        var subject = new ProcessingErrorService(mockDbContext, mockResourceEventPublisher);
        var entity = new ProcessingErrorEntity
        {
            Id = "id",
            ProcessingError = new ProcessingError
            {
                Notifications =
                [
                    new ErrorNotification { ExternalVersion = 1 },
                    new ErrorNotification { ExternalVersion = 2 },
                ],
            },
        };

        await subject.Update(entity, "etag", CancellationToken.None);

        await mockDbContext.ProcessingErrors.Received().Update(entity, "etag", CancellationToken.None);
        await mockDbContext.Received().SaveChangesAsync(CancellationToken.None);
        await mockResourceEventPublisher
            .Received()
            .Publish(
                Arg.Is<ResourceEvent<ProcessingErrorEntity>>(x => x.Operation == "Updated" && x.ChangeSet.Count > 0),
                CancellationToken.None
            );
    }
}
