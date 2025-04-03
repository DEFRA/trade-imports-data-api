using Defra.TradeImportsDataApi.Api.Extensions;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using Microsoft.AspNetCore.Mvc;

namespace Defra.TradeImportsDataApi.Api.Endpoints.ImportNotifications;

public static class EndpointRouteBuilderExtensions
{
    public static void MapImportNotificationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("import-notifications/{chedId}/", Get)
            .WithName("ImportNotificationByChedId")
            .WithTags("ImportNotifications")
            .WithSummary("Get ImportNotification")
            .WithDescription("Get an Import Notifications by CHED ID")
            .Produces<ImportNotificationResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        app.MapPut("import-notifications/{chedId}/", Put)
            .WithName("PutImportNotification")
            .WithTags("ImportNotifications")
            .WithSummary("Put ImportNotification")
            .WithDescription("Put an Import Notification")
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
            await dbContext.Notifications.Update(dbNotification, etag, cancellationToken);
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
