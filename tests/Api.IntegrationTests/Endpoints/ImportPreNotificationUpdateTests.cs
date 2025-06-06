using System.Net.Http.Json;
using Defra.TradeImportsDataApi.Api.Client;
using Defra.TradeImportsDataApi.Api.Endpoints.ImportPreNotifications;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Data.Mongo;
using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using Defra.TradeImportsDataApi.Domain.Gvms;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using MongoDB.Driver;
using ImportPreNotificationResponse = Defra.TradeImportsDataApi.Api.Client.ImportPreNotificationResponse;

namespace Defra.TradeImportsDataApi.Api.IntegrationTests.Endpoints;

public class ImportPreNotificationUpdateTests : IntegrationTestBase, IAsyncLifetime
{
    public required IMongoCollection<ImportPreNotificationEntity> Notifications { get; set; }
    public required IMongoCollection<ImportPreNotificationUpdateEntity> NotificationUpdates { get; set; }
    public required IMongoCollection<GmrEntity> Gmrs { get; set; }
    public required IMongoCollection<CustomsDeclarationEntity> CustomsDeclarations { get; set; }
    public required TradeImportsDataApiClient DataApiClient { get; set; }
    public required HttpClient HttpClient { get; set; }
    public required MongoDbContext MongoDbContext { get; set; }

    private static DateTime Updated => new(2025, 5, 20, 16, 0, 0, DateTimeKind.Utc);

    public async Task InitializeAsync()
    {
        Notifications = GetMongoCollection<ImportPreNotificationEntity>();
        NotificationUpdates = GetMongoCollection<ImportPreNotificationUpdateEntity>();
        Gmrs = GetMongoCollection<GmrEntity>();
        CustomsDeclarations = GetMongoCollection<CustomsDeclarationEntity>();

        await Notifications.DeleteManyAsync(FilterDefinition<ImportPreNotificationEntity>.Empty);
        await NotificationUpdates.DeleteManyAsync(FilterDefinition<ImportPreNotificationUpdateEntity>.Empty);
        await Gmrs.DeleteManyAsync(FilterDefinition<GmrEntity>.Empty);
        await CustomsDeclarations.DeleteManyAsync(FilterDefinition<CustomsDeclarationEntity>.Empty);

        DataApiClient = CreateDataApiClient();
        HttpClient = CreateHttpClient();
        MongoDbContext = new MongoDbContext(GetMongoDatabase(), NullLogger<MongoDbContext>.Instance);
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task WhenImportPreNotificationPointOfEntryMatch_ThenNotificationIdAndUpdateAsExpected()
    {
        var (chedRef, _) = ImportPreNotificationIdGenerator.GenerateReturnId();
        var (chedRefNoMatch, _) = ImportPreNotificationIdGenerator.GenerateReturnId();

        await CreateNotification(chedRef, pointOfEntry: "BCP1");
        await CreateNotification(chedRefNoMatch, pointOfEntry: "NoMatch");

        var chedRefResponse = await DataApiClient.GetImportPreNotification(chedRef, CancellationToken.None);
        var chedRefNoMatchResponse = await DataApiClient.GetImportPreNotification(
            chedRefNoMatch,
            CancellationToken.None
        );

        var result = await GetUpdates(pointOfEntry: ["BCP1"]);

        AssertResult(expectResult: true, result, chedRef, chedRefResponse?.Updated);

        // Updates
        await UpdateNotification(chedRefResponse!);
        await UpdateNotification(chedRefNoMatchResponse!);

        chedRefResponse = await DataApiClient.GetImportPreNotification(chedRef, CancellationToken.None);

        result = await GetUpdates(pointOfEntry: ["BCP1"]);

        AssertResult(expectResult: true, result, chedRef, chedRefResponse?.Updated);
    }

    [Fact]
    public async Task WhenImportPreNotificationTypeMatch_ThenNotificationIdAndUpdateAsExpected()
    {
        var (chedRef, _) = ImportPreNotificationIdGenerator.GenerateReturnId();
        var (chedRefNoMatch, _) = ImportPreNotificationIdGenerator.GenerateReturnId();

        await CreateNotification(chedRef, type: "CHEDA");
        await CreateNotification(chedRefNoMatch, type: "CHEDP");

        var chedRefResponse = await DataApiClient.GetImportPreNotification(chedRef, CancellationToken.None);
        var chedRefNoMatchResponse = await DataApiClient.GetImportPreNotification(
            chedRefNoMatch,
            CancellationToken.None
        );

        var result = await GetUpdates(type: ["CHEDA"]);

        AssertResult(expectResult: true, result, chedRef, chedRefResponse?.Updated);

        // Updates
        await UpdateNotification(chedRefResponse!);
        await UpdateNotification(chedRefNoMatchResponse!);

        chedRefResponse = await DataApiClient.GetImportPreNotification(chedRef, CancellationToken.None);

        result = await GetUpdates(type: ["CHEDA"]);

        AssertResult(expectResult: true, result, chedRef, chedRefResponse?.Updated);
    }

    [Fact]
    public async Task WhenImportPreNotificationStatusMatch_ThenNotificationIdAndUpdateAsExpected()
    {
        var (chedRef, _) = ImportPreNotificationIdGenerator.GenerateReturnId();
        var (chedRefNoMatch, _) = ImportPreNotificationIdGenerator.GenerateReturnId();

        await CreateNotification(chedRef, status: "SUBMITTED");
        await CreateNotification(chedRefNoMatch, status: "DRAFT");

        var chedRefResponse = await DataApiClient.GetImportPreNotification(chedRef, CancellationToken.None);
        var chedRefNoMatchResponse = await DataApiClient.GetImportPreNotification(
            chedRefNoMatch,
            CancellationToken.None
        );

        var result = await GetUpdates(status: ["SUBMITTED"]);

        AssertResult(expectResult: true, result, chedRef, chedRefResponse?.Updated);

        // Updates
        await UpdateNotification(chedRefResponse!);
        await UpdateNotification(chedRefNoMatchResponse!);

        chedRefResponse = await DataApiClient.GetImportPreNotification(chedRef, CancellationToken.None);

        result = await GetUpdates(status: ["SUBMITTED"]);

        AssertResult(expectResult: true, result, chedRef, chedRefResponse?.Updated);
    }

    [Fact]
    public async Task WhenImportPreNotificationExcludeStatusMatch_ThenNotificationIdAndUpdateAsExpected()
    {
        var (chedRef, _) = ImportPreNotificationIdGenerator.GenerateReturnId();
        var (chedRefNoMatch, _) = ImportPreNotificationIdGenerator.GenerateReturnId();

        await CreateNotification(chedRef, status: "SUBMITTED");
        await CreateNotification(chedRefNoMatch, status: "DRAFT");

        var chedRefResponse = await DataApiClient.GetImportPreNotification(chedRef, CancellationToken.None);
        var chedRefNoMatchResponse = await DataApiClient.GetImportPreNotification(
            chedRefNoMatch,
            CancellationToken.None
        );

        var result = await GetUpdates(excludeStatus: ["DRAFT"]);

        AssertResult(expectResult: true, result, chedRef, chedRefResponse?.Updated);

        // Updates
        await UpdateNotification(chedRefResponse!);
        await UpdateNotification(chedRefNoMatchResponse!);

        chedRefResponse = await DataApiClient.GetImportPreNotification(chedRef, CancellationToken.None);

        result = await GetUpdates(excludeStatus: ["DRAFT"]);

        AssertResult(expectResult: true, result, chedRef, chedRefResponse?.Updated);
    }

    [Fact]
    public async Task WhenImportPreNotificationAllFilters_ThenNotificationIdAndUpdateAsExpected()
    {
        var (chedRef, _) = ImportPreNotificationIdGenerator.GenerateReturnId();

        await CreateNotification(chedRef, pointOfEntry: "BCP1", type: "CHEDA", status: "SUBMITTED");

        for (var i = 0; i < 10; i++)
        {
            var (chedRefNoMatch, _) = ImportPreNotificationIdGenerator.GenerateReturnId();

            await CreateNotification(chedRefNoMatch, status: "DRAFT");
        }

        var chedRefResponse = await DataApiClient.GetImportPreNotification(chedRef, CancellationToken.None);

        var result = await GetUpdates(
            pointOfEntry: ["BCP1", "BCP2"],
            type: ["CHEDA", "CHEDP"],
            status: ["SUBMITTED", "VALIDATED"]
        );

        AssertResult(expectResult: true, result, chedRef, chedRefResponse?.Updated);

        // Updates
        await UpdateNotification(chedRefResponse!);

        chedRefResponse = await DataApiClient.GetImportPreNotification(chedRef, CancellationToken.None);

        result = await GetUpdates(
            pointOfEntry: ["BCP1", "BCP2"],
            type: ["CHEDA", "CHEDP"],
            status: ["SUBMITTED", "VALIDATED"]
        );

        AssertResult(expectResult: true, result, chedRef, chedRefResponse?.Updated);
    }

    [Fact]
    public async Task WhenGmr_ThenNotificationIdAndUpdateAsExpected()
    {
        var (chedRef, chedId) = ImportPreNotificationIdGenerator.GenerateReturnId();
        var mrn = Guid.NewGuid().ToString("N");
        var gmrId = Guid.NewGuid().ToString("N");

        await CreateNotification(chedRef, pointOfEntry: "BCP1", type: "CHEDA", status: "SUBMITTED");
        await CreateCustomsDeclaration(mrn, chedId);
        await CreateGmr(gmrId, transitsMrn: mrn);

        var gmrResponse = await DataApiClient.GetGmr(gmrId, CancellationToken.None);

        var result = await GetUpdates(
            pointOfEntry: ["BCP1", "BCP2"],
            type: ["CHEDA", "CHEDP"],
            status: ["SUBMITTED", "VALIDATED"]
        );

        AssertResult(expectResult: true, result, chedRef, gmrResponse?.Updated);

        // Updates

        await UpdateGmr(gmrResponse!);

        gmrResponse = await DataApiClient.GetGmr(gmrId, CancellationToken.None);

        result = await GetUpdates(
            pointOfEntry: ["BCP1", "BCP2"],
            type: ["CHEDA", "CHEDP"],
            status: ["SUBMITTED", "VALIDATED"]
        );

        AssertResult(expectResult: true, result, chedRef, gmrResponse?.Updated);
    }

    [Fact]
    public async Task WhenCustomsDeclaration_ThenNotificationIdAndUpdateAsExpected()
    {
        var (chedRef, chedId) = ImportPreNotificationIdGenerator.GenerateReturnId();
        var mrn = Guid.NewGuid().ToString("N");

        await CreateNotification(chedRef, pointOfEntry: "BCP1", type: "CHEDA", status: "SUBMITTED");
        await CreateCustomsDeclaration(mrn, chedId);

        var customsDeclarationResponse = await DataApiClient.GetCustomsDeclaration(mrn, CancellationToken.None);

        var result = await GetUpdates(
            pointOfEntry: ["BCP1", "BCP2"],
            type: ["CHEDA", "CHEDP"],
            status: ["SUBMITTED", "VALIDATED"]
        );

        AssertResult(expectResult: true, result, chedRef, customsDeclarationResponse?.Updated);

        // Updates

        await UpdateCustomsDeclaration(customsDeclarationResponse!);

        customsDeclarationResponse = await DataApiClient.GetCustomsDeclaration(mrn, CancellationToken.None);

        result = await GetUpdates(
            pointOfEntry: ["BCP1", "BCP2"],
            type: ["CHEDA", "CHEDP"],
            status: ["SUBMITTED", "VALIDATED"]
        );

        AssertResult(expectResult: true, result, chedRef, customsDeclarationResponse?.Updated);
    }

    private static void AssertResult(
        bool expectResult,
        ImportPreNotificationUpdatesResponse result,
        string chedRef,
        DateTime? updated = null
    )
    {
        if (expectResult)
        {
            result.ImportPreNotificationUpdates.Should().HaveCount(1);
            result.ImportPreNotificationUpdates[0].ReferenceNumber.Should().Be(chedRef);
            result.ImportPreNotificationUpdates[0].Updated.Should().Be(updated ?? Updated);
        }
        else
        {
            result.ImportPreNotificationUpdates.Should().BeEmpty();
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

        await DataApiClient.PutImportPreNotification(chedRef, notification, null, CancellationToken.None);
    }

    private async Task UpdateNotification(ImportPreNotificationResponse response)
    {
        await DataApiClient.PutImportPreNotification(
            response.ImportPreNotification.ReferenceNumber!,
            response.ImportPreNotification,
            response.ETag,
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

        await DataApiClient.PutCustomsDeclaration(mrn, customsDeclaration, null, CancellationToken.None);
    }

    private async Task UpdateCustomsDeclaration(CustomsDeclarationResponse response)
    {
        await DataApiClient.PutCustomsDeclaration(
            response.MovementReferenceNumber,
            new CustomsDeclaration
            {
                ClearanceRequest = response.ClearanceRequest,
                ClearanceDecision = response.ClearanceDecision,
                Finalisation = response.Finalisation,
                ExternalErrors = response.ExternalErrors,
            },
            response.ETag,
            CancellationToken.None
        );
    }

    private async Task CreateGmr(string gmrId, string? transitsMrn = null, string? customsMrn = null)
    {
        var gmr = new Gmr { Id = gmrId };

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

        await DataApiClient.PutGmr(gmrId, gmr, null, CancellationToken.None);
    }

    private async Task UpdateGmr(GmrResponse response)
    {
        await DataApiClient.PutGmr(response.Gmr.Id!, response.Gmr, response.ETag, CancellationToken.None);
    }

    private async Task<ImportPreNotificationUpdatesResponse> GetUpdates(
        string[]? pointOfEntry = null,
        string[]? type = null,
        string[]? status = null,
        string[]? excludeStatus = null
    )
    {
        var now = DateTime.UtcNow;
        var result =
            await HttpClient.GetFromJsonAsync<ImportPreNotificationUpdatesResponse>(
                Testing.Endpoints.ImportPreNotifications.GetUpdates(
                    EndpointQuery
                        .New.Where(EndpointFilter.From(now.AddMinutes(-30)))
                        .Where(EndpointFilter.To(now.AddMinutes(30)))
                        .Where(EndpointFilter.PointOfEntry(pointOfEntry))
                        .Where(EndpointFilter.Type(type))
                        .Where(EndpointFilter.Status(status))
                        .Where(EndpointFilter.ExcludeStatus(excludeStatus))
                )
            ) ?? throw new InvalidOperationException("Could not deserialize");

        return result;
    }
}
