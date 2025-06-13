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

    [Fact]
    public async Task WhenPaging_ThenPagesReturnedAsExpected()
    {
        const int pageSize = 5;

        // Create 12 CHEDs
        for (var i = 1; i <= 12; i++)
        {
            var ched = $"CHED.2025.{i.ToString().PadLeft(7, '0')}";

            await CreateNotification(ched, status: "DRAFT");
            await Task.Delay(1);

            if (i % 2 != 0)
                continue;

            // Add random update for every other CHED (18 total updates)
            var notification = await DataApiClient.GetImportPreNotification(ched, CancellationToken.None);
            await UpdateNotification(notification!);
        }

        // Loop expected pages
        for (var i = 1; i <= 3; i++)
        {
            var pageOffset = (i - 1) * pageSize;

            var result = await GetUpdates(page: i, pageSize: pageSize);

            // We wrote 12 unique CHEDs
            result.Total.Should().Be(12);

            var lastPage = i == 3;

            // As we wrote 12, we expect 3 pages where the
            // first two pages contain 5 records each and the
            // third page contains 2 records
            result.ImportPreNotificationUpdates.Should().HaveCount(lastPage ? 2 : pageSize);

            // Loop expected records per page
            for (var j = 1; j <= 5; j++)
            {
                result
                    .ImportPreNotificationUpdates[j - 1]
                    .ReferenceNumber.Should()
                    .Be($"CHED.2025.{(j + pageOffset).ToString().PadLeft(7, '0')}");

                // Only 2 records on the last page so break
                if (lastPage && j == 2)
                    break;
            }
        }
    }

    [Fact]
    public async Task WhenPaging_AndFirstChedUpdateAtEndOfUpdates_ThenItIsAlwaysLast()
    {
        string ched = nameof(ched);
        const int total = 12;

        for (var i = 1; i <= total; i++)
        {
            ched = $"CHED.2025.{i.ToString().PadLeft(7, '0')}";

            await CreateNotification(ched, status: "DRAFT");
        }

        var notificationResponse = await DataApiClient.GetImportPreNotification(ched, CancellationToken.None);

        await Task.Delay(1);
        await UpdateNotification(notificationResponse!);

        var result = await GetUpdates(pageSize: 25);

        result.ImportPreNotificationUpdates.Should().HaveCount(total);
        result.Total.Should().Be(total);
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(25);

        result.ImportPreNotificationUpdates[^1].ReferenceNumber.Should().Be(ched);
    }

    [Fact]
    public async Task WhenMultipleUpdatesWithTimePeriod_ShouldReturnSingleUpdate()
    {
        var (chedRef, chedId) = ImportPreNotificationIdGenerator.GenerateReturnId();
        var mrn = Guid.NewGuid().ToString("N");

        await CreateNotification(chedRef, pointOfEntry: "BCP1", type: "CHEDA", status: "SUBMITTED");
        await CreateCustomsDeclaration(mrn, chedId);

        var importPreNotification = await DataApiClient.GetImportPreNotification(chedRef, CancellationToken.None);
        var customsDeclarationResponse = await DataApiClient.GetCustomsDeclaration(mrn, CancellationToken.None);

        // Updates

        await Task.Delay(1);
        await UpdateNotification(importPreNotification!);
        await UpdateCustomsDeclaration(customsDeclarationResponse!);

        importPreNotification = await DataApiClient.GetImportPreNotification(chedRef, CancellationToken.None);

        await Task.Delay(1);
        await UpdateNotification(importPreNotification!);

        var result = await GetUpdates(pointOfEntry: ["BCP1"]);

        result.ImportPreNotificationUpdates.Should().HaveCount(1);
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
        string[]? excludeStatus = null,
        int? page = null,
        int? pageSize = null
    )
    {
        var now = DateTime.UtcNow;
        var query = EndpointQuery
            .New.Where(EndpointFilter.From(now.AddMinutes(-30)))
            .Where(EndpointFilter.To(now.AddMinutes(30)))
            .Where(EndpointFilter.PointOfEntry(pointOfEntry))
            .Where(EndpointFilter.Type(type))
            .Where(EndpointFilter.Status(status))
            .Where(EndpointFilter.ExcludeStatus(excludeStatus));

        if (page != null)
            query = query.Where(EndpointFilter.Page(page.GetValueOrDefault()));

        if (pageSize != null)
            query = query.Where(EndpointFilter.PageSize(pageSize.GetValueOrDefault()));

        var result =
            await HttpClient.GetFromJsonAsync<ImportPreNotificationUpdatesResponse>(
                Testing.Endpoints.ImportPreNotifications.GetUpdates(query)
            ) ?? throw new InvalidOperationException("Could not deserialize");

        return result;
    }
}
