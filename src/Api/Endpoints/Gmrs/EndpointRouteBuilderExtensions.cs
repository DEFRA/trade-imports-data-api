using System.Diagnostics.CodeAnalysis;
using Defra.TradeImportsDataApi.Api.Authentication;
using Defra.TradeImportsDataApi.Api.Extensions;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Api.Utils;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Defra.TradeImportsDataApi.Api.Endpoints.Gmrs;

public static class EndpointRouteBuilderExtensions
{
    public static void MapGmrEndpoints(this IEndpointRouteBuilder app, bool isDevelopment)
    {
        var route = app.MapGet("gmrs/{gmrId}/", Get)
            .WithName("GmrsByGmrId")
            .WithTags("Gmrs")
            .WithSummary("Get Gmr")
            .WithDescription("Get a GMR by GMR ID")
            .Produces<GmrResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization(PolicyNames.Read);

        AllowAnonymousForDevelopment(isDevelopment, route);

        route = app.MapPut("gmrs/{gmrId}/", Put)
            .WithName("PutGmr")
            .WithTags("Gmrs")
            .WithSummary("Put Gmr")
            .WithDescription("Put a GMR")
            .Produces<GmrResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization(PolicyNames.Write);

        AllowAnonymousForDevelopment(isDevelopment, route);
    }

    [ExcludeFromCodeCoverage]
    private static void AllowAnonymousForDevelopment(bool isDevelopment, RouteHandlerBuilder route)
    {
        if (isDevelopment)
            route.AllowAnonymous();
    }

    /// <param name="gmrId">GMR ID</param>
    /// <param name="context"></param>
    /// <param name="gmrService"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    private static async Task<IResult> Get(
        [FromRoute] string gmrId,
        HttpContext context,
        [FromServices] IGmrService gmrService,
        CancellationToken cancellationToken
    )
    {
        var gmrEntity = await gmrService.GetGmr(gmrId, cancellationToken);
        if (gmrEntity is null)
        {
            return Results.NotFound();
        }

        context.SetResponseEtag(gmrEntity.ETag);

        return Results.Ok(ToResponse(gmrEntity));
    }

    [HttpPut]
    private static async Task<IResult> Put(
        [FromRoute] string gmrId,
        HttpContext context,
        [FromBody] Domain.Gvms.Gmr data,
        [FromHeader(Name = "If-Match")] string? ifMatch,
        [FromServices] IGmrService gmrService,
        CancellationToken cancellationToken
    )
    {
        var gmrEntity = new GmrEntity { Id = gmrId, Data = data };

        var etag = ETags.ValidateAndParseFirst(ifMatch);

        try
        {
            gmrEntity = string.IsNullOrEmpty(etag)
                ? await gmrService.Insert(gmrEntity, cancellationToken)
                : await gmrService.Update(gmrEntity, etag, cancellationToken);

            context.SetResponseEtag(gmrEntity.ETag);

            return Results.Ok(ToResponse(gmrEntity));
        }
        catch (ConcurrencyException)
        {
            return Results.Conflict();
        }
    }

    private static GmrResponse ToResponse(GmrEntity gmrEntity)
    {
        return new GmrResponse(gmrEntity.Data, gmrEntity.Created, gmrEntity.Updated);
    }
}
