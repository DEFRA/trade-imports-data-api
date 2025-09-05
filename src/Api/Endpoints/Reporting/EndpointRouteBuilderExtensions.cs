using Defra.TradeImportsDataApi.Api.Authentication;
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
            .CustomsDeclarations.Where(x =>
                x.Finalisation!.MessageSentAt >= from
                && x.Finalisation!.MessageSentAt < to
                && x.Finalisation!.FinalState != "1" // Is not cancelled
                && x.Finalisation!.FinalState != "2" // Is not cancelled
            )
            .Select(x => new { x.Id, x.Finalisation!.IsManualRelease });

        var dbResult = await query.ToListWithFallbackAsync(cancellationToken);

        var total = dbResult.Count;
        var manualMrns = dbResult.Where(x => x.IsManualRelease).Select(x => x.Id).ToArray();
        var autoReleaseCounts = dbResult.Count(x => !x.IsManualRelease);

        return Results.Ok(new ManualReleaseReportResponse(total, autoReleaseCounts, manualMrns.Length, manualMrns));
    }
}
