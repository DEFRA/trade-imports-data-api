using Defra.TradeImportsData.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Defra.TradeImportsData.Api.Endpoints.Gmrs;

public static class EndpointRouteBuilderExtensions
{
    public static void MapGmrEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("gmrs/{gmrId}/", Get)
            .WithName("GmrsByGmrId")
            .WithTags("Gmrs")
            .WithSummary("Get Gmr")
            .WithDescription("Get a GMR by GMR ID")
            .Produces<GmrResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }

    /// <param name="gmrId">GMR ID</param>
    /// <param name="gmrService"></param>
    /// <param name="cancellationToken">Cancellation Token</param>
    [HttpGet]
    private static async Task<IResult> Get(
        [FromRoute] string gmrId,
        [FromServices] IGmrService gmrService,
        CancellationToken cancellationToken
    )
    {
        var gmr = await gmrService.GetGmr(gmrId);

        return gmr is not null ? Results.Ok(new GmrResponse(gmr.Data, gmr.Created, gmr.Updated)) : Results.NotFound();
    }
}
