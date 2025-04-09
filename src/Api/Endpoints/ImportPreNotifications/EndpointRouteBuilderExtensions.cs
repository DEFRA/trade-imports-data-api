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
        var importNotificationEntity = await importPreNotificationService.GetImportPreNotification(
            chedId,
            cancellationToken
        );
        if (importNotificationEntity is null)
        {
            return Results.NotFound();
        }

        context.SetResponseEtag(importNotificationEntity.ETag);

        return Results.Ok(
            new ImportPreNotificationResponse(
                importNotificationEntity.ImportPreNotification,
                importNotificationEntity.Created,
                importNotificationEntity.Updated
            )
        );
    }

    [HttpPut]
    private static async Task<IResult> Put(
        [FromRoute] string chedId,
        HttpContext context,
        [FromBody] ImportPreNotification data,
        [FromHeader(Name = "If-Match")] string? ifMatch,
        [FromServices] IImportPreNotificationService importPreNotificationService,
        CancellationToken cancellationToken
    )
    {
        var importNotificationEntity = new ImportPreNotificationEntity
        {
            Id = chedId,
            CustomsDeclarationIdentifier = chedId,
            ImportPreNotification = data,
        };

        var etag = ETags.ValidateAndParseFirst(ifMatch);

        try
        {
            importNotificationEntity = string.IsNullOrEmpty(etag)
                ? await importPreNotificationService.Insert(importNotificationEntity, cancellationToken)
                : await importPreNotificationService.Update(importNotificationEntity, etag, cancellationToken);
            if (string.IsNullOrEmpty(etag))
            {
                await importPreNotificationService.Insert(importNotificationEntity, cancellationToken);

                return Results.Created();
            }

            await importPreNotificationService.Update(importNotificationEntity, etag, cancellationToken);

            return Results.NoContent();
        }
        catch (ConcurrencyException)
        {
            return Results.Conflict();
        }
    }
}
