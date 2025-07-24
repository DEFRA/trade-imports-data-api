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
    private IResourceEventPublisher ResourceEventPublisher { get; }
    private ICustomsDeclarationRepository CustomsDeclarationRepository { get; }
    private IImportPreNotificationRepository ImportPreNotificationRepository { get; }
    private CustomsDeclarationService Subject { get; }

    public CustomsDeclarationServiceTests()
    {
        DbContext = Substitute.For<IDbContext>();
        ResourceEventPublisher = Substitute.For<IResourceEventPublisher>();
        CustomsDeclarationRepository = Substitute.For<ICustomsDeclarationRepository>();
        ImportPreNotificationRepository = Substitute.For<IImportPreNotificationRepository>();

        Subject = new CustomsDeclarationService(
            DbContext,
            ResourceEventPublisher,
            CustomsDeclarationRepository,
            ImportPreNotificationRepository
        );
    }

    [Fact]
    public async Task Insert_ShouldInsertAndPublish()
    {
        var (_, chedId) = ImportPreNotificationIdGenerator.GenerateReturnId();
        var entity = new CustomsDeclarationEntity
        {
            Id = "id",
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
            .Insert(entity, CancellationToken.None)
            .Returns(x =>
            {
                entity.OnSave();
                return entity;
            });

        await Subject.Insert(entity, CancellationToken.None);

        await DbContext.Received().StartTransaction(CancellationToken.None);
        await CustomsDeclarationRepository.Received().Insert(entity, CancellationToken.None);
        await ImportPreNotificationRepository
            .Received()
            .TrackImportPreNotificationUpdate(
                entity,
                Arg.Is<string[]>(x => x.SequenceEqual(entity.ImportPreNotificationIdentifiers)),
                CancellationToken.None
            );
        await DbContext.Received().SaveChanges(CancellationToken.None);
        await ResourceEventPublisher
            .Received()
            .Publish(
                Arg.Is<ResourceEvent<CustomsDeclarationEntity>>(x =>
                    x.Operation == "Created" && x.ChangeSet.Count > 0 && x.SubResourceType != null
                ),
                CancellationToken.None
            );
        await DbContext.Received().CommitTransaction(CancellationToken.None);
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

        await Subject.Update(entity, "etag", CancellationToken.None);

        await DbContext.Received().StartTransaction(CancellationToken.None);
        await CustomsDeclarationRepository.Received().Update(entity, "etag", CancellationToken.None);
        await DbContext.Received().SaveChanges(CancellationToken.None);
        await ResourceEventPublisher
            .Received()
            .Publish(
                Arg.Is<ResourceEvent<CustomsDeclarationEntity>>(x =>
                    x.Operation == "Updated" && x.ChangeSet.Count > 0 && x.SubResourceType != null
                ),
                CancellationToken.None
            );
        await DbContext.Received().CommitTransaction(CancellationToken.None);
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
