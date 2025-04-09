using System.Diagnostics.CodeAnalysis;
using Defra.TradeImportsDataApi.Api.Authentication;
using Defra.TradeImportsDataApi.Api.Extensions;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Api.Utils.Logging;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using Microsoft.AspNetCore.Mvc;

namespace Defra.TradeImportsDataApi.Api.Endpoints.ImportNotifications;

public static class EndpointRouteBuilderExtensions
{
    public static void MapImportNotificationEndpoints(this IEndpointRouteBuilder app, bool isDevelopment)
    {
        var route = app.MapGet("import-notifications/{chedId}/", Get)
            .WithName("ImportNotificationByChedId")
            .WithTags("ImportNotifications")
            .WithSummary("Get ImportNotification")
            .WithDescription("Get an Import Notifications by CHED ID")
            .Produces<ImportNotificationResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization(PolicyNames.Read);

        AllowAnonymousForDevelopment(isDevelopment, route);

        route = app.MapPut("import-notifications/{chedId}/", Put)
            .WithName("PutImportNotification")
            .WithTags("ImportNotifications")
            .WithSummary("Put ImportNotification")
            .WithDescription("Put an Import Notification")
            .Produces<ImportNotificationResponse>()
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
    /// <param name="importNotificationService"></param>
    /// <param name="loggerFactory"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    private static async Task<IResult> Get(
        [FromRoute] string chedId,
        HttpContext context,
        [FromServices] IImportNotificationService importNotificationService,
        [FromServices] ILoggerFactory loggerFactory,
        CancellationToken cancellationToken
    )
    {
        var logger = loggerFactory.CreateLogger(nameof(Get));

        using (logger.BeginScope("{Prefix}", chedId))
        {
            logger.LogInformation("Getting Import Notification");

            var importNotificationEntity = await importNotificationService.GetImportNotification(
                chedId,
                cancellationToken
            );
            if (importNotificationEntity is null)
            {
                return Results.NotFound();
            }

            context.SetResponseEtag(importNotificationEntity.ETag);

            return Results.Ok(ToResponse(importNotificationEntity));
        }
    }

    [HttpPut]
    private static async Task<IResult> Put(
        [FromRoute] string chedId,
        HttpContext context,
        [FromBody] ImportNotification data,
        [FromHeader(Name = "If-Match")] string? etag,
        [FromServices] IImportNotificationService importNotificationService,
        CancellationToken cancellationToken
    )
    {
        var importNotificationEntity = new ImportNotificationEntity
        {
            Id = chedId,
            CustomDeclarationIdentifier = chedId,
            Data = data,
        };

        try
        {
            importNotificationEntity = string.IsNullOrEmpty(etag)
                ? await importNotificationService.Insert(importNotificationEntity, cancellationToken)
                : await importNotificationService.Update(importNotificationEntity, etag, cancellationToken);

            context.SetResponseEtag(importNotificationEntity.ETag);

            return Results.Ok(ToResponse(importNotificationEntity));
        }
        catch (ConcurrencyException)
        {
            return Results.Conflict();
        }
    }

    private static ImportNotificationResponse ToResponse(ImportNotificationEntity importNotificationEntity)
    {
        return new ImportNotificationResponse(
            importNotificationEntity.Data,
            importNotificationEntity.Created,
            importNotificationEntity.Updated
        );
    }
}
