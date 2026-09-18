using Defra.TradeImportsDataApi.Api.Authentication;
using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Api.Endpoints.Gmrs;
using Defra.TradeImportsDataApi.Api.Exceptions;
using Defra.TradeImportsDataApi.Api.Extensions;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Api.Utils;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Defra.TradeImportsDataApi.Api.Endpoints.ChedReservation;

public static class EndpointRouteBuilderExtensions
{
    public static void MapChedReservationsEndpoints(this IEndpointRouteBuilder app)
    {
        const string groupName = "ChedReservations";

        app.MapGet("traces-cheds/{chedId}/reservation/{mrn}/", Get)
            .WithName("GetChedReservationByMrn")
            .WithTags(groupName)
            .WithSummary("Get Ched Reservation")
            .WithDescription("Get a Ched Reservation by Ched Reservation ID")
            .Produces<ChedReservationResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization(PolicyNames.Read);

        app.MapPut("traces-cheds/{chedId}/reservation/{mrn}/", Put)
            .WithName("PutChedReservation")
            .WithTags(groupName)
            .WithSummary("Put Ched Reservation")
            .WithDescription("Put a Ched Reservation")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization(PolicyNames.Write);

        app.MapDelete("traces-cheds/{chedId}/reservation/{mrn}/", Delete)
            .WithName("DeleteChedReservation")
            .WithTags(groupName)
            .WithSummary("Delete Ched Reservation")
            .WithDescription("Delete a Ched Reservation")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization(PolicyNames.Write);
    }

    /// <param name="chedId"></param>
    /// <param name="mrn"></param>
    /// <param name="context"></param>
    /// <param name="chedReservationRepository"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    private static async Task<IResult> Get(
        [FromRoute] string chedId,
        [FromRoute] string mrn,
        HttpContext context,
        [FromServices] IChedReservationRepository chedReservationRepository,
        CancellationToken cancellationToken
    )
    {
        var entity = (await chedReservationRepository.GetAll([$"{chedId}_{mrn}"], cancellationToken)).FirstOrDefault();
        if (entity is null)
        {
            return Results.NotFound();
        }

        context.SetResponseEtag(entity.ETag);

        return Results.Ok(new ChedReservationResponse(entity.Reservation, entity.Created, entity.Updated));
    }

    [HttpPut]
    private static async Task<IResult> Put(
        [FromRoute] string chedId,
        [FromRoute] string mrn,
        HttpContext context,
        [FromBody] Domain.Traces.Reservation reservation,
        [FromHeader(Name = "If-Match")] string? ifMatch,
        [FromServices] IChedReservationService chedReservationService,
        CancellationToken cancellationToken
    )
    {
        var entity = new ChedReservationEntity() { Id = $"{chedId}_{mrn}", Reservation = reservation };

        var etag = ETags.ValidateAndParseFirst(ifMatch);

        try
        {
            var serviceResponse = await chedReservationService.Upsert(entity, etag, cancellationToken);
            context.SetResponseEtag(serviceResponse.ETag);

            return Results.Ok(
                new ChedReservationResponse(
                    serviceResponse.Reservation,
                    serviceResponse.Created,
                    serviceResponse.Updated
                )
            );
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

    [HttpDelete]
    private static async Task<IResult> Delete(
        [FromRoute] string chedId,
        [FromRoute] string mrn,
        [FromServices] IChedReservationService chedReservationService,
        CancellationToken cancellationToken
    )
    {
        try
        {
            await chedReservationService.Delete(chedId, mrn, cancellationToken);

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
}
