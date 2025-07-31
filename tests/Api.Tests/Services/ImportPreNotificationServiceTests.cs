using System.Linq.Expressions;
using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Events;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Defra.TradeImportsDataApi.Api.Tests.Services;

public class ImportPreNotificationServiceTests
{
    private IDbContext DbContext { get; }
    private IResourceEventPublisher ResourceEventPublisher { get; }
    private IImportPreNotificationRepository ImportPreNotificationRepository { get; }
    private ICustomsDeclarationRepository CustomsDeclarationRepository { get; }
    private IResourceEventRepository ResourceEventRepository { get; }
    private ImportPreNotificationService Subject { get; }

    public ImportPreNotificationServiceTests()
    {
        DbContext = Substitute.For<IDbContext>();
        ResourceEventPublisher = Substitute.For<IResourceEventPublisher>();
        ImportPreNotificationRepository = Substitute.For<IImportPreNotificationRepository>();
        CustomsDeclarationRepository = Substitute.For<ICustomsDeclarationRepository>();
        ResourceEventRepository = Substitute.For<IResourceEventRepository>();

        Subject = new ImportPreNotificationService(
            DbContext,
            ResourceEventPublisher,
            ImportPreNotificationRepository,
            CustomsDeclarationRepository,
            ResourceEventRepository,
            NullLogger<ImportPreNotificationService>.Instance
        );
    }

    [Fact]
    public async Task Insert_ShouldInsertAndPublish()
    {
        var entity = new ImportPreNotificationEntity
        {
            Id = "id",
            ImportPreNotification = new ImportPreNotification { Version = 1 },
        };
        ImportPreNotificationRepository.Insert(entity).Returns(entity);
        var resourceEventEntityId = Guid.NewGuid().ToString();
        ResourceEventRepository
            .Insert(Arg.Any<ResourceEvent<ImportPreNotificationEntity>>())
            .Returns(call =>
            {
                var resourceEvent = call.Arg<ResourceEvent<ImportPreNotificationEntity>>();

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

        ImportPreNotificationRepository.Received().Insert(entity);
        Expression<Predicate<ResourceEvent<ImportPreNotificationEntity>>> assertion = x =>
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
        var existing = new ImportPreNotificationEntity
        {
            Id = id,
            ImportPreNotification = new ImportPreNotification { Version = 1 },
        };
        ImportPreNotificationRepository.Get(id, CancellationToken.None).Returns(existing);
        var entity = new ImportPreNotificationEntity
        {
            Id = id,
            ImportPreNotification = new ImportPreNotification { Version = 2 },
        };
        ImportPreNotificationRepository.Update(entity, "etag", CancellationToken.None).Returns((existing, entity));
        var resourceEventEntityId = Guid.NewGuid().ToString();
        ResourceEventRepository
            .Insert(Arg.Any<ResourceEvent<ImportPreNotificationEntity>>())
            .Returns(call =>
            {
                var resourceEvent = call.Arg<ResourceEvent<ImportPreNotificationEntity>>();

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

        await ImportPreNotificationRepository.Received().Update(entity, "etag", CancellationToken.None);
        Expression<Predicate<ResourceEvent<ImportPreNotificationEntity>>> assertion = x =>
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
        var existing = new ImportPreNotificationEntity
        {
            Id = id,
            ImportPreNotification = new ImportPreNotification { Version = 1 },
        };
        ImportPreNotificationRepository.Get(id, CancellationToken.None).Returns(existing);
        var entity = new ImportPreNotificationEntity
        {
            Id = id,
            ImportPreNotification = new ImportPreNotification { Version = 2 },
        };
        ImportPreNotificationRepository.Update(entity, "etag", CancellationToken.None).Returns((existing, entity));
        var resourceEventEntityId = Guid.NewGuid().ToString();
        ResourceEventRepository
            .Insert(Arg.Any<ResourceEvent<ImportPreNotificationEntity>>())
            .Returns(call =>
            {
                var resourceEvent = call.Arg<ResourceEvent<ImportPreNotificationEntity>>();

                return new ResourceEventEntity
                {
                    Id = resourceEventEntityId,
                    ResourceId = resourceEvent.ResourceId,
                    ResourceType = resourceEvent.ResourceType,
                    Message = "message body",
                };
            });
        ResourceEventPublisher
            .Publish(Arg.Any<ResourceEvent<ImportPreNotificationEntity>>(), CancellationToken.None)
            .Throws(new Exception());

        await Subject.Update(entity, "etag", CancellationToken.None);

        await DbContext.Received(2).StartTransaction(CancellationToken.None);

        // Transaction should be started but it will never be committed
        await DbContext.Received(1).SaveChanges(CancellationToken.None);
        await DbContext.Received(1).CommitTransaction(CancellationToken.None);

        ResourceEventRepository.DidNotReceiveWithAnyArgs().Update(Arg.Any<ResourceEventEntity>());
    }

    [Fact]
    public async Task GetImportPreNotification_ShouldReturn()
    {
        const string id = "id";
        ImportPreNotificationRepository
            .Get(id, CancellationToken.None)
            .Returns(new ImportPreNotificationEntity { Id = id, ImportPreNotification = new ImportPreNotification() });

        var result = await Subject.GetImportPreNotification(id, CancellationToken.None);

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetImportPreNotificationsByMrn_ShouldReturn()
    {
        const string id = "id";
        const string mrn = "mrn";
        var identifiers = new List<string> { "identifier" };
        CustomsDeclarationRepository
            .GetAllImportPreNotificationIdentifiers(mrn, CancellationToken.None)
            .Returns(identifiers);
        ImportPreNotificationRepository
            .GetAll(Arg.Is<string[]>(x => x.SequenceEqual(identifiers)), CancellationToken.None)
            .Returns(
                [new ImportPreNotificationEntity { Id = id, ImportPreNotification = new ImportPreNotification() }]
            );

        var result = await Subject.GetImportPreNotificationsByMrn(mrn, CancellationToken.None);

        result.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetImportPreNotificationUpdates_ShouldReturn()
    {
        const string id = "id";
        var from = DateTime.UtcNow;
        var to = DateTime.UtcNow.AddDays(1);
        ImportPreNotificationRepository
            .GetUpdates(new ImportPreNotificationUpdateQuery(from, to), cancellationToken: CancellationToken.None)
            .Returns(
                new ImportPreNotificationUpdates([new ImportPreNotificationUpdate(id, DateTime.UtcNow)], Total: 1)
            );

        var result = await Subject.GetImportPreNotificationUpdates(
            new ImportPreNotificationUpdateQuery(from, to),
            cancellationToken: CancellationToken.None
        );

        result.Should().NotBeNull();
        result.Updates.Should().NotBeEmpty();
        result.Total.Should().Be(1);
    }
}
