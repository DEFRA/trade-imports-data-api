using System.Diagnostics.CodeAnalysis;
using Defra.TradeImportsDataApi.Api.Authentication;
using Defra.TradeImportsDataApi.Api.Extensions;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Api.Utils;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using Microsoft.AspNetCore.Mvc;

namespace Defra.TradeImportsDataApi.Api.Endpoints.ImportPreNotifications;

public static class EndpointRouteBuilderExtensions
{
    public static void MapImportPreNotificationEndpoints(this IEndpointRouteBuilder app, bool isDevelopment)
    {
        var route = app.MapGet("import-pre-notifications/{chedId}/", Get)
            .WithName("GetImportPreNotificationByChedId")
            .WithTags("ImportPreNotifications")
            .WithSummary("Get ImportPreNotification")
            .WithDescription("Get an import pre-notification by CHED ID")
            .Produces<ImportPreNotificationResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization(PolicyNames.Read);

        AllowAnonymousForDevelopment(isDevelopment, route);

        route = app.MapPut("import-pre-notifications/{chedId}/", Put)
            .WithName("PutImportPreNotification")
            .WithTags("ImportPreNotifications")
            .WithSummary("Put ImportPreNotification")
            .WithDescription("Put an import pre-notification")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status204NoContent)
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

    /// <param name="chedId" example="CHEDA.GB.2024.1020304">CHED ID</param>
    /// <param name="context"></param>
    /// <param name="importPreNotificationService"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    private static async Task<IResult> Get(
        [FromRoute] string chedId,
        HttpContext context,
        [FromServices] IImportPreNotificationService importPreNotificationService,
        CancellationToken cancellationToken
    )
    {
        var importPreNotificationEntity = await importPreNotificationService.GetImportPreNotification(
            chedId,
            cancellationToken
        );
        if (importPreNotificationEntity is null)
        {
            return Results.NotFound();
        }

        context.SetResponseEtag(importPreNotificationEntity.ETag);

        return Results.Ok(
            new ImportPreNotificationResponse(
                importPreNotificationEntity.ImportPreNotification,
                importPreNotificationEntity.Created,
                importPreNotificationEntity.Updated
            )
        );
    }

    /// <param name="chedId" example="CHEDA.GB.2024.1020304">CHED ID</param>
    /// <param name="context"></param>
    /// <param name="importPreNotification"></param>
    /// <param name="ifMatch"></param>
    /// <param name="importPreNotificationService"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut]
    private static async Task<IResult> Put(
        [FromRoute] string chedId,
        HttpContext context,
        [FromBody] ImportPreNotification importPreNotification,
        [FromHeader(Name = "If-Match")] string? ifMatch,
        [FromServices] IImportPreNotificationService importPreNotificationService,
        CancellationToken cancellationToken
    )
    {
        var importPreNotificationEntity = new ImportPreNotificationEntity
        {
            Id = chedId,
            ImportPreNotification = importPreNotification,
        };

        var etag = ETags.ValidateAndParseFirst(ifMatch);

        try
        {
            if (string.IsNullOrEmpty(etag))
            {
                await importPreNotificationService.Insert(importPreNotificationEntity, cancellationToken);

                return Results.Created();
            }

            await importPreNotificationService.Update(importPreNotificationEntity, etag, cancellationToken);

            return Results.NoContent();
        }
        catch (ConcurrencyException)
        {
            return Results.Conflict();
        }
    }
}
