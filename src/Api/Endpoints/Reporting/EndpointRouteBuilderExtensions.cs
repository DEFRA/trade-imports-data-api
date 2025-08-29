using Defra.TradeImportsDataApi.Api.Authentication;
using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Defra.TradeImportsDataApi.Api.Endpoints.Reporting;

public static class EndpointRouteBuilderExtensions
{
    public static void MapReportingEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("reporting/manual-release", ManualRelease)
            .ExcludeFromDescription()
            .RequireAuthorization(PolicyNames.Read);

        app.MapGet("reporting/decisions", Decisions).ExcludeFromDescription().RequireAuthorization(PolicyNames.Read);
    }

    [HttpGet]
    private static async Task<IResult> ManualRelease(
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        [FromServices] IDbContext dbContext,
        CancellationToken cancellationToken
    )
    {
        var errors = new Dictionary<string, string[]>();
        if (from > to)
        {
            errors.Add("from", ["from cannot be greater than to"]);
        }

        if (to.Subtract(from).Days > 31)
        {
            errors.Add("", ["date span cannot be greater than 31 days"]);
        }

        if (from.Kind != DateTimeKind.Utc)
        {
            errors.Add("from", ["date must be UTC"]);
        }

        if (to.Kind != DateTimeKind.Utc)
        {
            errors.Add("to", ["date must be UTC"]);
        }

        if (errors.Count > 0)
        {
            return Results.ValidationProblem(errors);
        }

        var query = dbContext
            .CustomsDeclarations.Where(x => x.Finalisation!.MessageSentAt >= from && x.Finalisation!.MessageSentAt < to)
            .Select(x => new { x.Id, x.Finalisation!.IsManualRelease });

        var dbResult = await query.ToListWithFallbackAsync(cancellationToken);

        var total = dbResult.Count;
        var manualMrns = dbResult.Where(x => x.IsManualRelease).Select(x => x.Id).ToArray();
        var autoReleaseCounts = dbResult.Count(x => !x.IsManualRelease);

        return Results.Ok(new ManualReleaseReportResponse(total, autoReleaseCounts, manualMrns.Length, manualMrns));
    }

    [HttpGet]
    private static async Task<IResult> Decisions(
        [FromQuery] DateTime day,
        [FromServices] IReportRepository reportRepository,
        CancellationToken cancellationToken
    )
    {
        var clearanceDecisions = await reportRepository.GetClearanceDecisions(day, cancellationToken);
        var buckets = Enumerable.Range(0, 24).ToArray();
        var matchBuckets = new List<int>();
        var noMatchBuckets = new List<int>();

        foreach (var bucket in buckets)
        {
            // This repeated lookup is poor, refactor
            var match = clearanceDecisions.FirstOrDefault(x => x.Bucket.Hour == bucket && x.Match);
            var noMatch = clearanceDecisions.FirstOrDefault(x => x.Bucket.Hour == bucket && !x.Match);

            matchBuckets.Add(match?.Count ?? 0);
            noMatchBuckets.Add(noMatch?.Count ?? 0);
        }

        var response = new ReportResponse(
            buckets.Select(x => $"{x:00}:00").ToArray(),
            [
                new ReportDataset("Match", matchBuckets.ToArray()),
                new ReportDataset("No Match", noMatchBuckets.ToArray()),
            ]
        );

        return Results.Ok(response);
    }
}
