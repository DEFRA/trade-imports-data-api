using Defra.TradeImportsDataApi.Api.Client;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Data.Mongo;
using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using Defra.TradeImportsDataApi.Domain.Gvms;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
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
        Subject = new ImportPreNotificationService(MongoDbContext, Substitute.For<IResourceEventPublisher>());
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Theory]
    // MRN linked in Transits within GMR
    [InlineData(-1, 1, true, true)]
    [InlineData(0, 1, true, true)]
    [InlineData(-1, -1, false, true)]
    // MRN linked in Customs within GMR
    [InlineData(-1, 1, true, false)]
    [InlineData(0, 1, true, false)]
    [InlineData(-1, -1, false, false)]
    public async Task WhenGmrUpdated_ThenNotificationIdAndUpdateAsExpected(
        int fromSeconds,
        int toSeconds,
        bool expectResult,
        bool useTransits
    )
    {
        var (chedRef, chedId) = ImportPreNotificationIdGenerator.GenerateReturnId();
        var excludedDateTime = Updated.AddDays(-1);
        var mrn = Guid.NewGuid().ToString("N");
        var gmrId = Guid.NewGuid().ToString("N");

        // Set up CHED > MRN > GMR where the GMR has been updated within the query period
        await CreateNotification(chedRef);
        await OverrideUpdated(Notifications, chedRef, excludedDateTime);
        await CreateCustomsDeclaration(mrn, chedId);
        await OverrideUpdated(CustomsDeclarations, mrn, excludedDateTime);
        if (useTransits)
            await CreateGmr(gmrId, transitsMrn: mrn);
        else
            await CreateGmr(gmrId, customsMrn: mrn);
        await OverrideUpdated(Gmrs, gmrId, Updated);

        var result = await Subject.GetImportPreNotificationUpdates(
            Updated.AddSeconds(fromSeconds),
            Updated.AddSeconds(toSeconds),
            CancellationToken.None
        );

        AssertResult(expectResult, result, chedRef);
    }

    [Theory]
    [InlineData(-1, 1, true)]
    [InlineData(0, 1, true)]
    [InlineData(-1, -1, false)]
    public async Task WhenCustomsDeclarationUpdated_ThenNotificationIdAndUpdateAsExpected(
        int fromSeconds,
        int toSeconds,
        bool expectResult
    )
    {
        var (chedRef, chedId) = ImportPreNotificationIdGenerator.GenerateReturnId();
        var excludedDateTime = Updated.AddDays(-1);
        var mrn = Guid.NewGuid().ToString("N");

        // Set up CHED > MRN where the MRN has been updated within the query period
        await CreateNotification(chedRef);
        await OverrideUpdated(Notifications, chedRef, excludedDateTime);
        await CreateCustomsDeclaration(mrn, chedId);
        await OverrideUpdated(CustomsDeclarations, mrn, Updated);

        var result = await Subject.GetImportPreNotificationUpdates(
            Updated.AddSeconds(fromSeconds),
            Updated.AddSeconds(toSeconds),
            CancellationToken.None
        );

        AssertResult(expectResult, result, chedRef);
    }

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
            CancellationToken.None
        );

        AssertResult(expectResult, result, chedRef);
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

    private async Task CreateNotification(string chedRef)
    {
        await Client.PutImportPreNotification(
            chedRef,
            new ImportPreNotification { ReferenceNumber = chedRef, Version = 1 },
            null,
            CancellationToken.None
        );
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

    private static async Task OverrideUpdated<T>(IMongoCollection<T> collection, string id, DateTime updated)
        where T : IDataEntity
    {
        var filter = Builders<T>.Filter.Eq(x => x.Id, id);
        var update = Builders<T>.Update.Set(x => x.Updated, updated);

        await collection.UpdateOneAsync(filter, update);
    }
}
