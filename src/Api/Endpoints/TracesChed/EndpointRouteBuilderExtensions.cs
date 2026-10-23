using Defra.TradeImportsDataApi.Api.Authentication;
using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Api.Endpoints.CustomsDeclarations;
using Defra.TradeImportsDataApi.Api.Exceptions;
using Defra.TradeImportsDataApi.Api.Extensions;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Api.Utils;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Trade.Gateway.Api.Contract.Certificate;

namespace Defra.TradeImportsDataApi.Api.Endpoints.TracesChed;

public static class EndpointRouteBuilderExtensions
{
    public static void MapTracesChedsEndpoints(this IEndpointRouteBuilder app)
    {
        const string groupName = "TracesCheds";

        app.MapGet("traces-cheds/{chedId}/", Get)
            .WithName("GetTracesChedByChedId")
            .WithTags(groupName)
            .WithSummary("Get Traces Ched")
            .WithDescription("Get a TRACES Ched by Id")
            .Produces<TracesChedResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization(PolicyNames.Read);

        app.MapGet("traces-cheds/{chedId}/customs-declarations", GetCustomsDeclarations)
            .WithName("GetCustomsDeclarationsByTracesChedId")
            .WithTags(groupName)
            .WithSummary("Get CustomsDeclarations by CHED ID")
            .WithDescription("Get associated customs declarations by CHED ID")
            .Produces<CustomsDeclarationsResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization(PolicyNames.Read);

        app.MapPut("traces-cheds/{chedId}/", Put)
            .WithName("PutTracesChed")
            .WithTags(groupName)
            .WithSummary("Put Traces Ched")
            .WithDescription("Put a Traces Ched")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization(PolicyNames.Write);

        app.MapGet("traces-ched-updates/", GetUpdates)
            .WithName("GetTracesChedUpdates")
            .WithTags(groupName)
            .WithSummary("Get TracesChedUpdates")
            .WithDescription("Get TRACES CHEDs updated between a period of time")
            .Produces<TracesChedUpdatesResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization(PolicyNames.Read)
            .AddEndpointFilter<TracesChedUpdatesRequest.TracesChedUpdatesRequestValidator>();
    }

    /// <param name="chedId" example="CHEDA.GB.2024.1020304">CHED ID</param>
    /// <param name="context"></param>
    /// <param name="tracesChedService"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    private static async Task<IResult> Get(
        [FromRoute] string chedId,
        HttpContext context,
        [FromServices] ITracesChedService tracesChedService,
        CancellationToken cancellationToken
    )
    {
        var entity = await tracesChedService.Get(chedId, cancellationToken);
        if (entity is null)
        {
            return Results.NotFound();
        }

        context.SetResponseEtag(entity.ETag);

        return Results.Ok(new TracesChedResponse(entity.Ched, entity.Created, entity.Updated));
    }

    /// <param name="chedId" example="CHEDA.GB.2024.1020304">CHED ID</param>
    /// <param name="context"></param>
    /// <param name="ched"></param>
    /// <param name="ifMatch"></param>
    /// <param name="tracesChedService"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut]
    private static async Task<IResult> Put(
        [FromRoute] string chedId,
        HttpContext context,
        [FromBody] DefraUNVTDCHEDProfile ched,
        [FromHeader(Name = "If-Match")] string? ifMatch,
        [FromServices] ITracesChedService tracesChedService,
        CancellationToken cancellationToken
    )
    {
        var entity = new TracesChedEntity() { Id = chedId, Ched = ched };

        var etag = ETags.ValidateAndParseFirst(ifMatch);

        try
        {
            if (string.IsNullOrEmpty(etag))
            {
                await tracesChedService.Insert(entity, cancellationToken);

                return Results.Created();
            }

            await tracesChedService.Update(entity, etag, cancellationToken);

            return Results.NoContent();
        }
        catch (ConcurrencyException)
        {
            return Results.Conflict();
        }
        catch (EntityNotFoundException)
        {
            return Results.NotFound();
        }
    }

    /// <param name="chedId" example="CHEDA.GB.2024.1020304">CHED ID</param>
    /// <param name="context"></param>
    /// <param name="tracesChedService"></param>
    /// <param name="customsDeclarationService"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    private static async Task<IResult> GetCustomsDeclarations(
        [FromRoute] string chedId,
        HttpContext context,
        [FromServices] ITracesChedService tracesChedService,
        [FromServices] ICustomsDeclarationService customsDeclarationService,
        CancellationToken cancellationToken
    )
    {
        var existing = await tracesChedService.Get(chedId, cancellationToken);

        if (existing is null)
        {
            return Results.NotFound();
        }

        var customsDeclarations = await customsDeclarationService.GetCustomsDeclarationsByFullChedId(
            chedId,
            cancellationToken
        );

        return Results.Ok(
            new CustomsDeclarationsResponse(
                customsDeclarations
                    .Select(customsDeclarationEntity => new CustomsDeclarationResponse(
                        customsDeclarationEntity.Id,
                        customsDeclarationEntity.ClearanceRequest,
                        customsDeclarationEntity.ClearanceDecision,
                        customsDeclarationEntity.Finalisation,
                        customsDeclarationEntity.ExternalErrors,
                        customsDeclarationEntity.Created,
                        customsDeclarationEntity.Updated
                    ))
                    .ToList()
            )
        );
    }

    /// <param name="request"></param>
    /// <param name="tracesChedService"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    private static async Task<IResult> GetUpdates(
        [AsParameters] TracesChedUpdatesRequest request,
        [FromServices] ITracesChedService tracesChedService,
        CancellationToken cancellationToken
    )
    {
        var page = request.Page.GetValueOrDefault();
        var pageSize = request.PageSize.GetValueOrDefault();
        var result = await tracesChedService.GetUpdates(
            new TracesChedUpdateQuery(request.From, request.To, page, pageSize),
            cancellationToken
        );

        return Results.Ok(
            new TracesChedUpdatesResponse(
                result.Updates.Select(x => new TracesChedUpdateResponse(x.ReferenceNumber, x.Updated)).ToList(),
                result.Total,
                page,
                pageSize
            )
        );
    }
}
