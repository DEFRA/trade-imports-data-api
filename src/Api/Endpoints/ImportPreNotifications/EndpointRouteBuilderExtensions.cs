using Defra.TradeImportsDataApi.Api.Authentication;
using Defra.TradeImportsDataApi.Api.Endpoints.CustomsDeclarations;
using Defra.TradeImportsDataApi.Api.Endpoints.Gmrs;
using Defra.TradeImportsDataApi.Api.Exceptions;
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
    public static void MapImportPreNotificationEndpoints(this IEndpointRouteBuilder app)
    {
        const string groupName = "ImportPreNotifications";

        app.MapGet("import-pre-notifications/{chedId}/", Get)
            .WithName("GetImportPreNotificationByChedId")
            .WithTags(groupName)
            .WithSummary("Get ImportPreNotification")
            .WithDescription("Get an import pre-notification by CHED ID")
            .Produces<ImportPreNotificationResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization(PolicyNames.Read);

        app.MapGet("import-pre-notifications/{chedId}/customs-declarations", GetCustomsDeclarations)
            .WithName("GetCustomsDeclarationsByChedId")
            .WithTags(groupName)
            .WithSummary("Get CustomsDeclarations by CHED ID")
            .WithDescription("Get associated customs declarations by CHED ID")
            .Produces<CustomsDeclarationsResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization(PolicyNames.Read);

        app.MapGet("import-pre-notifications/{chedId}/gmrs", GetGmrs)
            .WithName("GetGmrsByChedId")
            .WithName(groupName)
            .WithSummary("Get Gmrs by CHED ID")
            .WithDescription("Get associated Gmrs by CHED ID")
            .Produces<GmrsResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization(PolicyNames.Read);

        app.MapPut("import-pre-notifications/{chedId}/", Put)
            .WithName("PutImportPreNotification")
            .WithTags(groupName)
            .WithSummary("Put ImportPreNotification")
            .WithDescription("Put an import pre-notification")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization(PolicyNames.Write);

        app.MapGet("import-pre-notification-updates/", GetUpdates)
            .WithName("GetImportPreNotificationUpdates")
            .WithTags(groupName)
            .WithSummary("Get ImportPreNotificationUpdates")
            .WithDescription(
                "Get an import pre-notification updated between a period of time, which will include update checks on linked resources"
            )
            .Produces<ImportPreNotificationUpdatesResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization(PolicyNames.Read);
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
    /// <param name="importPreNotificationService"></param>
    /// <param name="customsDeclarationService"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    private static async Task<IResult> GetCustomsDeclarations(
        [FromRoute] string chedId,
        HttpContext context,
        [FromServices] IImportPreNotificationService importPreNotificationService,
        [FromServices] ICustomsDeclarationService customsDeclarationService,
        CancellationToken cancellationToken
    )
    {
        var customsDeclarations = await customsDeclarationService.GetCustomsDeclarationsByChedId(
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
                        customsDeclarationEntity.InboundError,
                        customsDeclarationEntity.Created,
                        customsDeclarationEntity.Updated
                    ))
                    .ToList()
            )
        );
    }

    /// <param name="chedId" example="CHEDA.GB.2024.1020304">CHED ID</param>
    /// <param name="context"></param>
    /// <param name="importPreNotificationService"></param>
    /// <param name="gmrsService"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    private static async Task<IResult> GetGmrs(
        [FromRoute] string chedId,
        HttpContext context,
        [FromServices] IImportPreNotificationService importPreNotificationService,
        [FromServices] IGmrService gmrsService,
        CancellationToken cancellationToken
    )
    {
        var gmrs = await gmrsService.GetGmrByChedId(chedId, cancellationToken);

        return Results.Ok(
            new GmrsResponse(
                gmrs.Select(gmrEntity => new GmrResponse(gmrEntity.Gmr, gmrEntity.Created, gmrEntity.Updated)).ToList()
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
        catch (EntityNotFoundException)
        {
            return Results.NotFound();
        }
    }

    /// <param name="request"></param>
    /// <param name="importPreNotificationService"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    private static async Task<IResult> GetUpdates(
        [AsParameters] ImportPreNotificationUpdatesRequest request,
        [FromServices] IImportPreNotificationService importPreNotificationService,
        CancellationToken cancellationToken
    )
    {
        var result = await importPreNotificationService.GetImportPreNotificationUpdates(
            request.From,
            request.To,
            request.PointOfEntry,
            request.Type,
            request.Status,
            cancellationToken
        );

        return Results.Ok(
            new ImportPreNotificationUpdatesResponse(
                result.Select(x => new ImportPreNotificationUpdateResponse(x.ReferenceNumber, x.Updated)).ToList()
            )
        );
    }
}
