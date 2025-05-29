using Defra.TradeImportsDataApi.Api.Client;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Data.Mongo;
using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using Defra.TradeImportsDataApi.Domain.Gvms;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using MongoDB.Driver;
using NSubstitute;

namespace Defra.TradeImportsDataApi.Api.IntegrationTests.Services;

public class ImportPreNotificationServiceTests : IntegrationTestBase, IAsyncLifetime
{
    public required IMongoCollection<ImportPreNotificationEntity> Notifications { get; set; }
    public required IMongoCollection<GmrEntity> Gmrs { get; set; }
    public required IMongoCollection<CustomsDeclarationEntity> CustomsDeclarations { get; set; }
    public required TradeImportsDataApiClient Client { get; set; }
    public required MongoDbContext MongoDbContext { get; set; }
    public required ImportPreNotificationService Subject { get; set; }

    private static DateTime Updated => new(2025, 5, 20, 16, 0, 0, DateTimeKind.Utc);

    public async Task InitializeAsync()
    {
        Notifications = GetMongoCollection<ImportPreNotificationEntity>();
        Gmrs = GetMongoCollection<GmrEntity>();
        CustomsDeclarations = GetMongoCollection<CustomsDeclarationEntity>();

        await Notifications.DeleteManyAsync(FilterDefinition<ImportPreNotificationEntity>.Empty);
        await Gmrs.DeleteManyAsync(FilterDefinition<GmrEntity>.Empty);
        await CustomsDeclarations.DeleteManyAsync(FilterDefinition<CustomsDeclarationEntity>.Empty);

        Client = CreateDataApiClient();
        MongoDbContext = new MongoDbContext(GetMongoDatabase());
        Subject = new ImportPreNotificationService(
            MongoDbContext,
            Substitute.For<IResourceEventPublisher>(),
            NullLogger<ImportPreNotificationService>.Instance
        );
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Theory]
    [InlineData(-1, 1, true)]
    [InlineData(0, 1, true)]
    [InlineData(-1, -1, false)]
    public async Task WhenImportPreNotificationUpdated_ThenNotificationIdAndUpdateAsExpected(
        int fromSeconds,
        int toSeconds,
        bool expectResult
    )
    {
        var (chedRef, _) = ImportPreNotificationIdGenerator.GenerateReturnId();

        // Set up CHED where the CHED has been updated within the query period
        await CreateNotification(chedRef);
        await OverrideUpdated(Notifications, chedRef, Updated);

        var result = await Subject.GetImportPreNotificationUpdates(
            Updated.AddSeconds(fromSeconds),
            Updated.AddSeconds(toSeconds),
            pointOfEntry: null,
            type: null,
            status: null,
            CancellationToken.None
        );

        AssertResult(expectResult, result, chedRef);
    }

    [Fact]
    public async Task WhenImportPreNotificationPointOfEntryMatch_ThenNotificationIdAndUpdateAsExpected()
    {
        var (chedRef, _) = ImportPreNotificationIdGenerator.GenerateReturnId();
        var (chedRefNoMatch, _) = ImportPreNotificationIdGenerator.GenerateReturnId();

        await CreateNotification(chedRef, pointOfEntry: "BCP1");
        await OverrideUpdated(Notifications, chedRef, Updated);

        await CreateNotification(chedRefNoMatch, pointOfEntry: "NoMatch");
        await OverrideUpdated(Notifications, chedRefNoMatch, Updated);

        var result = await Subject.GetImportPreNotificationUpdates(
            Updated.AddSeconds(-1),
            Updated.AddSeconds(1),
            pointOfEntry: ["BCP1"],
            type: null,
            status: null,
            cancellationToken: CancellationToken.None
        );

        AssertResult(expectResult: true, result, chedRef);
    }

    [Fact]
    public async Task WhenImportPreNotificationTypeMatch_ThenNotificationIdAndUpdateAsExpected()
    {
        var (chedRef, _) = ImportPreNotificationIdGenerator.GenerateReturnId();
        var (chedRefNoMatch, _) = ImportPreNotificationIdGenerator.GenerateReturnId();

        await CreateNotification(chedRef, type: "CHEDA");
        await OverrideUpdated(Notifications, chedRef, Updated);

        await CreateNotification(chedRefNoMatch, type: "CHEDP");
        await OverrideUpdated(Notifications, chedRefNoMatch, Updated);

        var result = await Subject.GetImportPreNotificationUpdates(
            Updated.AddSeconds(-1),
            Updated.AddSeconds(1),
            pointOfEntry: null,
            type: ["CHEDA"],
            status: null,
            cancellationToken: CancellationToken.None
        );

        AssertResult(expectResult: true, result, chedRef);
    }

    [Fact]
    public async Task WhenImportPreNotificationStatusMatch_ThenNotificationIdAndUpdateAsExpected()
    {
        var (chedRef, _) = ImportPreNotificationIdGenerator.GenerateReturnId();
        var (chedRefNoMatch, _) = ImportPreNotificationIdGenerator.GenerateReturnId();

        await CreateNotification(chedRef, status: "SUBMITTED");
        await OverrideUpdated(Notifications, chedRef, Updated);

        await CreateNotification(chedRefNoMatch, status: "DRAFT");
        await OverrideUpdated(Notifications, chedRefNoMatch, Updated);

        var result = await Subject.GetImportPreNotificationUpdates(
            Updated.AddSeconds(-1),
            Updated.AddSeconds(1),
            pointOfEntry: null,
            type: null,
            status: ["SUBMITTED"],
            cancellationToken: CancellationToken.None
        );

        AssertResult(expectResult: true, result, chedRef);
    }

    [Fact]
    public async Task WhenImportPreNotificationAllFilters_ThenNotificationIdAndUpdateAsExpected()
    {
        var (chedRef, _) = ImportPreNotificationIdGenerator.GenerateReturnId();

        await CreateNotification(chedRef, pointOfEntry: "BCP1", type: "CHEDA", status: "SUBMITTED");
        await OverrideUpdated(Notifications, chedRef, Updated);

        for (var i = 0; i < 10; i++)
        {
            var (chedRefNoMatch, _) = ImportPreNotificationIdGenerator.GenerateReturnId();

            await CreateNotification(chedRefNoMatch, status: "DRAFT");
            await OverrideUpdated(Notifications, chedRefNoMatch, Updated.AddSeconds(1));
        }

        var result = await Subject.GetImportPreNotificationUpdates(
            Updated.AddSeconds(-1),
            Updated.AddSeconds(1),
            pointOfEntry: ["BCP1", "BCP2"],
            type: ["CHEDA", "CHEDP"],
            status: ["SUBMITTED", "VALIDATED"],
            cancellationToken: CancellationToken.None
        );

        AssertResult(expectResult: true, result, chedRef);
    }

    [Fact]
    public async Task WhenImportPreNotification_AndLinkedData_ThenNotificationIdAndUpdateAsExpected()
    {
        var (chedRef, chedId) = ImportPreNotificationIdGenerator.GenerateReturnId();
        var mrn = Guid.NewGuid().ToString("N");
        var gmrId = Guid.NewGuid().ToString("N");

        await CreateNotification(chedRef, pointOfEntry: "BCP1", type: "CHEDA", status: "SUBMITTED");
        await OverrideUpdated(Notifications, chedRef, Updated);
        await CreateCustomsDeclaration(mrn, chedId);
        await OverrideUpdated(CustomsDeclarations, mrn, Updated);
        await CreateGmr(gmrId, transitsMrn: mrn);
        await OverrideUpdated(Gmrs, gmrId, Updated);

        for (var i = 0; i < 10; i++)
        {
            var (chedRefNoMatch, _) = ImportPreNotificationIdGenerator.GenerateReturnId();

            await CreateNotification(chedRefNoMatch, status: "DRAFT");
            await OverrideUpdated(Notifications, chedRefNoMatch, Updated.AddSeconds(1));
        }

        var result = await Subject.GetImportPreNotificationUpdates(
            Updated.AddSeconds(-1),
            Updated.AddSeconds(1),
            pointOfEntry: ["BCP1", "BCP2"],
            type: ["CHEDA", "CHEDP"],
            status: ["SUBMITTED", "VALIDATED"],
            cancellationToken: CancellationToken.None
        );

        AssertResult(expectResult: true, result, chedRef);
    }

    private static void AssertResult(bool expectResult, List<ImportPreNotificationUpdate> result, string chedRef)
    {
        if (expectResult)
        {
            result.Should().HaveCount(1);
            result[0].ReferenceNumber.Should().Be(chedRef);
            result[0].Updated.Should().Be(Updated);
        }
        else
        {
            result.Should().BeEmpty();
        }
    }

    private async Task CreateNotification(
        string chedRef,
        string? pointOfEntry = null,
        string? type = null,
        string? status = null
    )
    {
        var notification = new ImportPreNotification { ReferenceNumber = chedRef, Version = 1 };

        if (pointOfEntry != null)
            notification.PartOne = new PartOne { PointOfEntry = pointOfEntry };

        if (type != null)
            notification.ImportNotificationType = type;

        if (status != null)
            notification.Status = status;

        await Client.PutImportPreNotification(chedRef, notification, null, CancellationToken.None);
    }

    private static async Task OverrideUpdated<T>(IMongoCollection<T> collection, string id, DateTime updated)
        where T : IDataEntity
    {
        var filter = Builders<T>.Filter.Eq(x => x.Id, id);
        var update = Builders<T>.Update.Set(x => x.Updated, updated);

        await collection.UpdateOneAsync(filter, update);
    }

    private async Task CreateCustomsDeclaration(string mrn, string? chedId = null)
    {
        var customsDeclaration = new CustomsDeclaration { ClearanceRequest = new ClearanceRequest() };

        if (chedId != null)
            customsDeclaration = new CustomsDeclaration
            {
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

        await Client.PutCustomsDeclaration(mrn, customsDeclaration, null, CancellationToken.None);
    }

    private async Task CreateGmr(string gmrId, string? transitsMrn = null, string? customsMrn = null)
    {
        var gmr = new Gmr();

        if (transitsMrn != null)
        {
            gmr.Declarations ??= new Declarations();
            gmr.Declarations.Transits = [new Transits { Id = transitsMrn }];
        }

        if (customsMrn != null)
        {
            gmr.Declarations ??= new Declarations();
            gmr.Declarations.Customs = [new Customs { Id = customsMrn }];
        }

        await Client.PutGmr(gmrId, gmr, null, CancellationToken.None);
    }
}
