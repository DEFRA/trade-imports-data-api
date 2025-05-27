using Defra.TradeImportsDataApi.Api.Exceptions;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Events;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;

namespace Defra.TradeImportsDataApi.Api.Tests.Services;

public class ImportPreNotificationServiceTests
{
    [Fact]
    public async Task Insert_ShouldInsertAndPublish()
    {
        var mockDbContext = Substitute.For<IDbContext>();
        var mockResourceEventPublisher = Substitute.For<IResourceEventPublisher>();
        var subject = new ImportPreNotificationService(
            mockDbContext,
            mockResourceEventPublisher,
            NullLogger<ImportPreNotificationService>.Instance
        );
        var entity = new ImportPreNotificationEntity
        {
            Id = "id",
            ImportPreNotification = new ImportPreNotification { Version = 1 },
        };

        await subject.Insert(entity, CancellationToken.None);

        await mockDbContext.ImportPreNotifications.Received().Insert(entity, CancellationToken.None);
        await mockDbContext.Received().SaveChangesAsync(CancellationToken.None);
        await mockResourceEventPublisher
            .Received()
            .Publish(
                Arg.Is<ResourceEvent<ImportPreNotificationEntity>>(x =>
                    x.Operation == "Created" && x.Resource == null && x.ChangeSet.Count == 0
                ),
                CancellationToken.None
            );
    }

    [Fact]
    public async Task Update_WhenNotExists_ShouldThrow()
    {
        var mockDbContext = Substitute.For<IDbContext>();
        var mockResourceEventPublisher = Substitute.For<IResourceEventPublisher>();
        var subject = new ImportPreNotificationService(
            mockDbContext,
            mockResourceEventPublisher,
            NullLogger<ImportPreNotificationService>.Instance
        );
        var entity = new ImportPreNotificationEntity { Id = "id", ImportPreNotification = new ImportPreNotification() };

        var act = async () => await subject.Update(entity, "etag", CancellationToken.None);

        await act.Should().ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async Task Update_ShouldUpdateAndPublish()
    {
        var mockDbContext = Substitute.For<IDbContext>();
        const string id = "id";
        mockDbContext
            .ImportPreNotifications.Find(id)
            .Returns(
                new ImportPreNotificationEntity
                {
                    Id = id,
                    ImportPreNotification = new ImportPreNotification { Version = 1 },
                }
            );
        var mockResourceEventPublisher = Substitute.For<IResourceEventPublisher>();
        var subject = new ImportPreNotificationService(
            mockDbContext,
            mockResourceEventPublisher,
            NullLogger<ImportPreNotificationService>.Instance
        );
        var entity = new ImportPreNotificationEntity
        {
            Id = id,
            ImportPreNotification = new ImportPreNotification { Version = 2 },
        };

        await subject.Update(entity, "etag", CancellationToken.None);

        await mockDbContext.ImportPreNotifications.Received().Update(entity, "etag", CancellationToken.None);
        await mockDbContext.Received().SaveChangesAsync(CancellationToken.None);
        await mockResourceEventPublisher
            .Received()
            .Publish(
                Arg.Is<ResourceEvent<ImportPreNotificationEntity>>(x =>
                    x.Operation == "Updated" && x.Resource == null && x.ChangeSet.Count == 0
                ),
                CancellationToken.None
            );
    }
}
