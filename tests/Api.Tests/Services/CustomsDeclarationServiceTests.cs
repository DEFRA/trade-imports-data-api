using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using Defra.TradeImportsDataApi.Domain.Events;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using NSubstitute;

namespace Defra.TradeImportsDataApi.Api.Tests.Services;

public class CustomsDeclarationServiceTests
{
    private IDbContext DbContext { get; }
    private ICustomsDeclarationRepository CustomsDeclarationRepository { get; }
    private IImportPreNotificationRepository ImportPreNotificationRepository { get; }
    private IResourceEventRepository ResourceEventRepository { get; }
    private IResourceEventService ResourceEventService { get; }
    private CustomsDeclarationService Subject { get; }

    public CustomsDeclarationServiceTests()
    {
        DbContext = Substitute.For<IDbContext>();
        CustomsDeclarationRepository = Substitute.For<ICustomsDeclarationRepository>();
        ImportPreNotificationRepository = Substitute.For<IImportPreNotificationRepository>();
        ResourceEventRepository = Substitute.For<IResourceEventRepository>();
        ResourceEventService = Substitute.For<IResourceEventService>();

        Subject = new CustomsDeclarationService(
            DbContext,
            CustomsDeclarationRepository,
            ImportPreNotificationRepository,
            ResourceEventRepository,
            ResourceEventService
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
                    SubResourceType = resourceEvent.SubResourceType,
                    Operation = resourceEvent.Operation,
                    Message = "message body",
                };
            });

        await Subject.Insert(entity, CancellationToken.None);

        await DbContext.Received(1).StartTransaction(CancellationToken.None);
        await DbContext.Received(1).SaveChanges(CancellationToken.None);
        await DbContext.Received(1).CommitTransaction(CancellationToken.None);

        CustomsDeclarationRepository.Received().Insert(entity);
        await ImportPreNotificationRepository
            .Received()
            .TrackImportPreNotificationUpdate(
                entity,
                Arg.Is<string[]>(x => x.SequenceEqual(entity.ImportPreNotificationIdentifiers)),
                CancellationToken.None
            );
        ResourceEventRepository
            .Received()
            .Insert(
                Arg.Is<ResourceEvent<CustomsDeclarationEntity>>(x =>
                    x.Operation == "Created" && x.ChangeSet.Count == 0 && x.SubResourceType != null
                )
            );
        await ResourceEventService
            .Received()
            .Publish(Arg.Is<ResourceEventEntity>(x => x.Id == resourceEventEntityId), CancellationToken.None);
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
                    SubResourceType = resourceEvent.SubResourceType,
                    Operation = resourceEvent.Operation,
                    Message = "message body",
                };
            });

        await Subject.Update(entity, "etag", CancellationToken.None);

        await DbContext.Received(1).StartTransaction(CancellationToken.None);
        await DbContext.Received(1).SaveChanges(CancellationToken.None);
        await DbContext.Received(1).CommitTransaction(CancellationToken.None);

        await CustomsDeclarationRepository.Received().Update(entity, "etag", CancellationToken.None);
        ResourceEventRepository
            .Received()
            .Insert(
                Arg.Is<ResourceEvent<CustomsDeclarationEntity>>(x =>
                    x.Operation == "Updated" && x.ChangeSet.Count > 0 && x.SubResourceType != null
                )
            );
        await ResourceEventService
            .Received()
            .Publish(Arg.Is<ResourceEventEntity>(x => x.Id == resourceEventEntityId), CancellationToken.None);
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
