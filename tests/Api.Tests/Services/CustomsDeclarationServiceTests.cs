using System.Linq.Expressions;
using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using Defra.TradeImportsDataApi.Domain.Events;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Defra.TradeImportsDataApi.Api.Tests.Services;

public class CustomsDeclarationServiceTests
{
    private IDbContext DbContext { get; }
    private IResourceEventPublisher ResourceEventPublisher { get; }
    private ICustomsDeclarationRepository CustomsDeclarationRepository { get; }
    private IImportPreNotificationRepository ImportPreNotificationRepository { get; }
    private IResourceEventRepository ResourceEventRepository { get; }
    private CustomsDeclarationService Subject { get; }

    public CustomsDeclarationServiceTests()
    {
        DbContext = Substitute.For<IDbContext>();
        ResourceEventPublisher = Substitute.For<IResourceEventPublisher>();
        CustomsDeclarationRepository = Substitute.For<ICustomsDeclarationRepository>();
        ImportPreNotificationRepository = Substitute.For<IImportPreNotificationRepository>();
        ResourceEventRepository = Substitute.For<IResourceEventRepository>();

        Subject = new CustomsDeclarationService(
            DbContext,
            ResourceEventPublisher,
            CustomsDeclarationRepository,
            ImportPreNotificationRepository,
            ResourceEventRepository,
            NullLogger<CustomsDeclarationService>.Instance
        );
    }

    [Fact]
    public async Task Insert_ShouldInsertAndPublish()
    {
        var (_, chedId) = ImportPreNotificationIdGenerator.GenerateReturnId();
        const string id = "id";
        var entity = new CustomsDeclarationEntity
        {
            Id = id,
            ClearanceRequest = new ClearanceRequest
            {
                ExternalVersion = 1,
                Commodities =
                [
                    new Commodity
                    {
                        Documents =
                        [
                            new ImportDocument
                            {
                                DocumentReference = new ImportDocumentReference($"GBCHD2025.{chedId}"),
                                DocumentCode = "C640",
                            },
                        ],
                    },
                ],
            },
        };
        CustomsDeclarationRepository
            .Insert(entity)
            .Returns(_ =>
            {
                entity.OnSave();
                return entity;
            });
        var resourceEventEntityId = Guid.NewGuid().ToString();
        ResourceEventRepository
            .Insert(Arg.Any<ResourceEvent<CustomsDeclarationEntity>>())
            .Returns(call =>
            {
                var resourceEvent = call.Arg<ResourceEvent<CustomsDeclarationEntity>>();

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

        CustomsDeclarationRepository.Received().Insert(entity);
        await ImportPreNotificationRepository
            .Received()
            .TrackImportPreNotificationUpdate(
                entity,
                Arg.Is<string[]>(x => x.SequenceEqual(entity.ImportPreNotificationIdentifiers)),
                CancellationToken.None
            );
        Expression<Predicate<ResourceEvent<CustomsDeclarationEntity>>> assertion = x =>
            x.Operation == "Created" && x.ChangeSet.Count > 0 && x.SubResourceType != null;
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
        var existing = new CustomsDeclarationEntity
        {
            Id = id,
            ClearanceRequest = new ClearanceRequest { ExternalVersion = 1 },
        };
        CustomsDeclarationRepository.Get(id, CancellationToken.None).Returns(existing);
        var entity = new CustomsDeclarationEntity
        {
            Id = id,
            ClearanceRequest = new ClearanceRequest { ExternalVersion = 2 },
        };
        CustomsDeclarationRepository.Update(entity, "etag", CancellationToken.None).Returns((existing, entity));
        var resourceEventEntityId = Guid.NewGuid().ToString();
        ResourceEventRepository
            .Insert(Arg.Any<ResourceEvent<CustomsDeclarationEntity>>())
            .Returns(call =>
            {
                var resourceEvent = call.Arg<ResourceEvent<CustomsDeclarationEntity>>();

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

        await CustomsDeclarationRepository.Received().Update(entity, "etag", CancellationToken.None);
        Expression<Predicate<ResourceEvent<CustomsDeclarationEntity>>> assertion = x =>
            x.Operation == "Updated" && x.ChangeSet.Count > 0 && x.SubResourceType != null;
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
        var existing = new CustomsDeclarationEntity
        {
            Id = id,
            ClearanceRequest = new ClearanceRequest { ExternalVersion = 1 },
        };
        CustomsDeclarationRepository.Get(id, CancellationToken.None).Returns(existing);
        var entity = new CustomsDeclarationEntity
        {
            Id = id,
            ClearanceRequest = new ClearanceRequest { ExternalVersion = 2 },
        };
        CustomsDeclarationRepository.Update(entity, "etag", CancellationToken.None).Returns((existing, entity));
        var resourceEventEntityId = Guid.NewGuid().ToString();
        ResourceEventRepository
            .Insert(Arg.Any<ResourceEvent<CustomsDeclarationEntity>>())
            .Returns(call =>
            {
                var resourceEvent = call.Arg<ResourceEvent<CustomsDeclarationEntity>>();

                return new ResourceEventEntity
                {
                    Id = resourceEventEntityId,
                    ResourceId = resourceEvent.ResourceId,
                    ResourceType = resourceEvent.ResourceType,
                    Message = "message body",
                };
            });
        ResourceEventPublisher
            .Publish(Arg.Any<ResourceEvent<CustomsDeclarationEntity>>(), CancellationToken.None)
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
        CustomsDeclarationRepository.Get(id, CancellationToken.None).Returns(new CustomsDeclarationEntity { Id = id });

        var result = await Subject.GetCustomsDeclaration(id, CancellationToken.None);

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetCustomsDeclarationsByChedId_ShouldReturn()
    {
        const string id = "id";
        var (chedRef, chedId) = ImportPreNotificationIdGenerator.GenerateReturnId();
        CustomsDeclarationRepository
            .GetAll(chedId, CancellationToken.None)
            .Returns([new CustomsDeclarationEntity { Id = id }]);

        var result = await Subject.GetCustomsDeclarationsByChedId(chedRef, CancellationToken.None);

        result.Should().NotBeEmpty();
    }
}
