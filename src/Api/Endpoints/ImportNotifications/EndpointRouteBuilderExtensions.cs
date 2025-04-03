using Defra.TradeImportsData.Api.Extensions;
using Defra.TradeImportsData.Data;
using Defra.TradeImportsData.Data.Entities;
using Defra.TradeImportsData.Domain.IPaffs;
using Microsoft.AspNetCore.Mvc;

namespace Defra.TradeImportsData.Api.Endpoints.ImportNotifications;

public static class EndpointRouteBuilderExtensions
{
    public static void MapImportNotificationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("importnotifications/{chedId}/", Get)
            .WithName("ImportNotificationsByChedId")
            .WithTags("ImportNotifications")
            .WithSummary("Get ImportNotification")
            .WithDescription("Get an ImportNotifications by CHED ID")
            .Produces<ImportNotificationResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        app.MapPut("importnotifications/{chedId}/", Put)
            .WithName("PutImportNotification")
            .WithTags("ImportNotifications")
            .WithSummary("Put ImportNotifications")
            .WithDescription("Put an ImportNotifications")
            .Produces<ImportNotificationResponse>()
            .ProducesProblem(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }

    [HttpGet]
    private static async Task<IResult> Get(
        [FromRoute] string chedId,
        HttpContext context,
        [FromServices] IDbContext dbContext,
        CancellationToken cancellationToken
    )
    {
        var dbNotification = await dbContext.Notifications.Find(chedId, cancellationToken);
        if (dbNotification == null)
        {
            return Results.NotFound();
        }

        context.SetResponseEtag(dbNotification.ETag);
        var apiResponse = new ImportNotificationResponse(
            dbNotification.Data,
            dbNotification.Created,
            dbNotification.Updated
        );
        return Results.Ok(apiResponse);
    }

    [HttpPut]
    private static async Task<IResult> Put(
        [FromRoute] string chedId,
        [FromBody] ImportNotification data,
        [FromHeader(Name = "If-Match")] string? etag,
        [FromServices] IDbContext dbContext,
        CancellationToken cancellationToken
    )
    {
        var dbNotification = new ImportNotificationEntity()
        {
            Id = chedId,
            CustomDeclarationIdentifier = chedId,
            Data = data,
        };
        if (string.IsNullOrEmpty(etag))
        {
            await dbContext.Notifications.Insert(dbNotification, cancellationToken);
        }
        else
        {
            dbNotification.ETag = etag;
            await dbContext.Notifications.Update(dbNotification, cancellationToken);
        }

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
            return Results.Ok();
        }
        catch (ConcurrencyException)
        {
            return Results.Conflict();
        }
    }
}
