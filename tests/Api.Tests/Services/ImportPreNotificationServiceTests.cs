using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Events;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using FluentAssertions;
using NSubstitute;

namespace Defra.TradeImportsDataApi.Api.Tests.Services;

public class ImportPreNotificationServiceTests
{
    private IDbContext DbContext { get; }
    private IResourceEventPublisher ResourceEventPublisher { get; }
    private IImportPreNotificationRepository ImportPreNotificationRepository { get; }
    private ICustomsDeclarationRepository CustomsDeclarationRepository { get; }
    private ImportPreNotificationService Subject { get; }

    public ImportPreNotificationServiceTests()
    {
        DbContext = Substitute.For<IDbContext>();
        ResourceEventPublisher = Substitute.For<IResourceEventPublisher>();
        ImportPreNotificationRepository = Substitute.For<IImportPreNotificationRepository>();
        CustomsDeclarationRepository = Substitute.For<ICustomsDeclarationRepository>();

        Subject = new ImportPreNotificationService(
            DbContext,
            ResourceEventPublisher,
            ImportPreNotificationRepository,
            CustomsDeclarationRepository
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
        ImportPreNotificationRepository.Insert(entity, CancellationToken.None).Returns(entity);

        await Subject.Insert(entity, CancellationToken.None);

        await ImportPreNotificationRepository.Received().Insert(entity, CancellationToken.None);
        await DbContext.Received().SaveChangesAsync(CancellationToken.None);
        await ResourceEventPublisher
            .Received()
            .Publish(
                Arg.Is<ResourceEvent<ImportPreNotificationEntity>>(x =>
                    x.Operation == "Created" && x.ChangeSet.Count > 0
                ),
                CancellationToken.None
            );
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

        await Subject.Update(entity, "etag", CancellationToken.None);

        await ImportPreNotificationRepository.Received().Update(entity, "etag", CancellationToken.None);
        await DbContext.Received().SaveChangesAsync(CancellationToken.None);
        await ResourceEventPublisher
            .Received()
            .Publish(
                Arg.Is<ResourceEvent<ImportPreNotificationEntity>>(x =>
                    x.Operation == "Updated" && x.ChangeSet.Count > 0
                ),
                CancellationToken.None
            );
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
