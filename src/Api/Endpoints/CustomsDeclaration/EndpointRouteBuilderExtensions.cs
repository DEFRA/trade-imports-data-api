using Defra.TradeImportsData.Api.Extensions;
using Defra.TradeImportsData.Data;
using Defra.TradeImportsData.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Defra.TradeImportsData.Api.Endpoints.CustomsDeclaration;

public static class EndpointRouteBuilderExtensions
{
    public static void MapCustomsDeclarationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("customs-declaration/{mrn}/", Get)
            .WithName("ImportNotificationsByChedId")
            .WithTags("ImportNotifications")
            .WithSummary("Get ImportNotification")
            .WithDescription("Get an ImportNotifications by CHED ID")
            .Produces<CustomsDeclarationResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        app.MapPut("customs-declaration/{mrn}/", Put)
            .WithName("PutImportNotification")
            .WithTags("ImportNotifications")
            .WithSummary("Put ImportNotifications")
            .WithDescription("Put an ImportNotifications")
            .Produces<CustomsDeclarationResponse>()
            .ProducesProblem(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }

    [HttpGet]
    private static async Task<IResult> Get(
        [FromRoute] string mrn,
        HttpContext context,
        [FromServices] IDbContext dbContext,
        CancellationToken cancellationToken
    )
    {
        var dbEntity = await dbContext.CustomDeclarations.Find(mrn, cancellationToken);
        if (dbEntity == null)
        {
            return Results.NotFound();
        }

        context.SetResponseEtag(dbEntity.ETag);
        var apiResponse = new CustomsDeclarationResponse(dbEntity.Data, dbEntity.Created, dbEntity.Updated);
        return Results.Ok(apiResponse);
    }

    [HttpPut]
    private static async Task<IResult> Put(
        [FromRoute] string mrn,
        [FromBody] Domain.CustomsDeclaration.CustomsDeclaration data,
        [FromHeader(Name = "If-Match")] string? etag,
        [FromServices] IDbContext dbContext,
        CancellationToken cancellationToken
    )
    {
        var dbEntity = new CustomsDeclarationEntity() { Id = mrn, Data = data };
        if (string.IsNullOrEmpty(etag))
        {
            await dbContext.CustomDeclarations.Insert(dbEntity, cancellationToken);
        }
        else
        {
            await dbContext.CustomDeclarations.Update(dbEntity, etag, cancellationToken);
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
