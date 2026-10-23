using Defra.TradeImportsDataApi.Api.Authentication;
using Defra.TradeImportsDataApi.Api.Exceptions;
using Defra.TradeImportsDataApi.Api.Extensions;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Api.Utils;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Iuu;
using Microsoft.AspNetCore.Mvc;

namespace Defra.TradeImportsDataApi.Api.Endpoints.IuuNotifications;

public static class EndpointRouteBuilderExtensions
{
    public static void MapIuuNotificationEndpoints(this IEndpointRouteBuilder app)
    {
        const string groupName = "IuuNotifications";

        app.MapGet("iuu-notifications/{iuuReference}/", Get)
            .WithName("GetIuuNotificationByIuuReference")
            .WithTags(groupName)
            .WithSummary("Get IUU Notification")
            .WithDescription("Get an IUU Notification by Reference")
            .Produces<IuuNotificationResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization(PolicyNames.Read);

        app.MapPut("iuu-notifications/{iuuReference}/", Put)
            .WithName("PutIuuNotification")
            .WithTags(groupName)
            .WithSummary("Put IUU Notification")
            .WithDescription("Put an IUU Notification")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization(PolicyNames.Write);
    }

    /// <param name="iuuReference" example="IUU.FR.2026.0000001">IUU Reference</param>
    /// <param name="context"></param>
    /// <param name="iuuNotificationService"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    private static async Task<IResult> Get(
        [FromRoute] string iuuReference,
        HttpContext context,
        [FromServices] IIuuNotificationService iuuNotificationService,
        CancellationToken cancellationToken
    )
    {
        var entity = await iuuNotificationService.Get(iuuReference, cancellationToken);
        if (entity is null)
        {
            return Results.NotFound();
        }

        context.SetResponseEtag(entity.ETag);

        return Results.Ok(new IuuNotificationResponse(entity.IuuNotification, entity.Created, entity.Updated));
    }

    /// <param name="iuuReference" example="IUU.FR.2026.0000001">IUU Reference</param>
    /// <param name="context"></param>
    /// <param name="iuuNotification"></param>
    /// <param name="ifMatch"></param>
    /// <param name="iuuNotificationService"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut]
    private static async Task<IResult> Put(
        [FromRoute] string iuuReference,
        HttpContext context,
        [FromBody] IuuNotification iuuNotification,
        [FromHeader(Name = "If-Match")] string? ifMatch,
        [FromServices] IIuuNotificationService iuuNotificationService,
        CancellationToken cancellationToken
    )
    {
        var entity = new IuuNotificationEntity { Id = iuuReference, IuuNotification = iuuNotification };

        var etag = ETags.ValidateAndParseFirst(ifMatch);

        try
        {
            if (string.IsNullOrEmpty(etag))
            {
                await iuuNotificationService.Insert(entity, cancellationToken);

                return Results.Created();
            }

            await iuuNotificationService.Update(entity, etag, cancellationToken);

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
