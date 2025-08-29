using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Data.Mongo;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Defra.TradeImportsDataApi.Api.IntegrationTests.Endpoints;

public class ReportTests : IntegrationTestBase, IAsyncLifetime
{
    public required IMongoCollection<ReportClearanceDecisionEntity> Decisions { get; set; }
    public required HttpClient HttpClient { get; set; }
    public required MongoDbContext MongoDbContext { get; set; }

    public async Task InitializeAsync()
    {
        Decisions = GetMongoCollection<ReportClearanceDecisionEntity>();

        await Decisions.DeleteManyAsync(FilterDefinition<ReportClearanceDecisionEntity>.Empty);

        HttpClient = CreateHttpClient();
        MongoDbContext = new MongoDbContext(GetMongoDatabase(), NullLogger<MongoDbContext>.Instance);
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task ClearanceDecisions()
    {
        var repo = new ReportRepository(MongoDbContext);
        var mrn1 = Guid.NewGuid().ToString();
        var mrn2 = Guid.NewGuid().ToString();
        var now = new DateTime(2025, 8, 29, 11, 0, 0, DateTimeKind.Utc);

        // MRN1 - 2 no matches in the same hour, then one match 2 hours later
        await Decisions.InsertOneAsync(
            new ReportClearanceDecisionEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                MovementReferenceNumber = mrn1,
                Match = false,
                Created = now,
                Updated = now,
            }
        );
        await Decisions.InsertOneAsync(
            new ReportClearanceDecisionEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                MovementReferenceNumber = mrn1,
                Match = false,
                Created = now.AddMinutes(5),
                Updated = now.AddMinutes(5),
            }
        );
        await Decisions.InsertOneAsync(
            new ReportClearanceDecisionEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                MovementReferenceNumber = mrn1,
                Match = true,
                Created = now.AddHours(2),
                Updated = now.AddHours(2),
            }
        );

        // MRN2 - 1 no match in the same hour, then one match 2 hours later
        await Decisions.InsertOneAsync(
            new ReportClearanceDecisionEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                MovementReferenceNumber = mrn2,
                Match = false,
                Created = now,
                Updated = now,
            }
        );
        await Decisions.InsertOneAsync(
            new ReportClearanceDecisionEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                MovementReferenceNumber = mrn2,
                Match = true,
                Created = now.AddHours(2),
                Updated = now.AddHours(2),
            }
        );

        var result = await repo.GetClearanceDecisions(now, CancellationToken.None);

        await Verify(result).UseMethodName($"{nameof(ClearanceDecisions)}_Data");

        var response = await HttpClient.GetAsync("reporting/decisions?day=2025-08-29");
        response.EnsureSuccessStatusCode();

        await VerifyJson(await response.Content.ReadAsStringAsync())
            .UseMethodName($"{nameof(ClearanceDecisions)}_Endpoint")
            .UseStrictJson();
    }
}
