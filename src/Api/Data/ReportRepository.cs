using System.Diagnostics.CodeAnalysis;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Defra.TradeImportsDataApi.Api.Data;

public class ReportRepository(IDbContext dbContext) : IReportRepository
{
    // Consider moving anything business logic related out of the repository

    public void ClearanceRequest(string mrn)
    {
        dbContext.ReportClearanceRequests.Insert(
            new ReportClearanceRequestEntity { Id = ObjectId.GenerateNewId().ToString(), MovementReferenceNumber = mrn }
        );
    }

    public void ClearanceDecision(string mrn, ClearanceDecision? decision)
    {
        if (decision?.Results is null)
            return;

        string[] noMatchCodes = ["E70", "E71", "E72", "E73", "E87"];

        dbContext.ReportClearanceDecisions.Insert(
            new ReportClearanceDecisionEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                MovementReferenceNumber = mrn,
                // Do we expect the internal decision code to ever be null?
                // Front end is using the presence of decision code too
                Match =
                    decision.Results?.All(x =>
                        x.DecisionCode is not null && !noMatchCodes.Contains(x.InternalDecisionCode)
                    ) ?? false,
            }
        );
    }

    public void Finalisation(string mrn, Finalisation? finalisation)
    {
        if (finalisation is null)
            return;

        dbContext.ReportFinalisations.Insert(
            new ReportFinalisationEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                MovementReferenceNumber = mrn,
                AutomaticRelease = !finalisation.IsManualRelease,
            }
        );
    }

    public void ImportPreNotification(ImportPreNotification? importPreNotification)
    {
        if (importPreNotification?.ReferenceNumber is null)
            return;

        dbContext.ReportImportPreNotifications.Insert(
            new ReportImportPreNotificationEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                ImportPreNotificationId = importPreNotification.ReferenceNumber,
                ImportNotificationType = importPreNotification.ImportNotificationType,
            }
        );
    }

    [SuppressMessage("Major Code Smell", "S125:Sections of code should not be commented out")]
    public async Task<IReadOnlyList<ReportClearanceDecision>> GetClearanceDecisions(
        DateTime day,
        CancellationToken cancellationToken
    )
    {
        var start = day.Date;
        var end = start.AddDays(1).Date;
        const string unit = "hour"; // "hour" or "day"

        var aggregatePipeline = new[]
        {
            new BsonDocument(
                "$match",
                new BsonDocument("created", new BsonDocument { { "$gte", start }, { "$lt", end } })
            ),
            new BsonDocument(
                "$set",
                new BsonDocument
                {
                    {
                        "bucket",
                        new BsonDocument(
                            "$dateTrunc",
                            new BsonDocument
                            {
                                { "date", "$created" },
                                { "unit", unit },
                                { "timezone", "UTC" }, // What about timezone?
                            }
                        )
                    },
                }
            ),
            /* new BsonDocument(
                "$group",
                new BsonDocument
                {
                    {
                        "_id",
                        new BsonDocument
                        {
                            { "bucket", "$bucket" },
                            { "match", "$match" },
                            { "mrn", "$movementReferenceNumber" },
                        }
                    },
                }
            ),
            include this if we want to count by mrn
            if added, include _id. as prefix to bucket and match in step below */
            new BsonDocument(
                "$group",
                new BsonDocument
                {
                    {
                        "_id",
                        new BsonDocument
                        {
                            {
                                "bucket",
                                "$bucket" /*$_id.bucket*/
                            },
                            {
                                "match",
                                "$match" /*$_id.match*/
                            },
                        }
                    },
                    { "count", new BsonDocument("$sum", 1) },
                }
            ),
            new BsonDocument(
                "$project",
                new BsonDocument
                {
                    { "_id", 0 },
                    { "bucket", "$_id.bucket" },
                    { "match", "$_id.match" },
                    { "count", "$count" },
                }
            ),
            new BsonDocument("$sort", new BsonDocument { { "bucket", 1 }, { "match", 1 } }),
        };

        var aggregateTask = dbContext.ReportClearanceDecisions.Collection.AggregateAsync<ReportClearanceDecision>(
            aggregatePipeline,
            cancellationToken: cancellationToken
        );

        return await (await aggregateTask).ToListAsync(cancellationToken);
    }
}
