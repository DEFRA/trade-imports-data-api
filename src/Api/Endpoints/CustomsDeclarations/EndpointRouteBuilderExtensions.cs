using Defra.TradeImportsDataApi.Api.Authentication;
using Defra.TradeImportsDataApi.Api.Endpoints.ImportPreNotifications;
using Defra.TradeImportsDataApi.Api.Exceptions;
using Defra.TradeImportsDataApi.Api.Extensions;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Api.Utils;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Defra.TradeImportsDataApi.Api.Endpoints.CustomsDeclarations;

public static class EndpointRouteBuilderExtensions
{
    public static void MapCustomsDeclarationEndpoints(this IEndpointRouteBuilder app)
    {
        const string groupName = "CustomsDeclarations";

        app.MapGet("customs-declarations/{mrn}/", Get)
            .WithName("CustomsDeclarationByMrn")
            .WithTags(groupName)
            .WithSummary("Get CustomsDeclaration")
            .WithDescription("Get a Customs Declaration by MRN")
            .Produces<CustomsDeclarationResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization(PolicyNames.Read);

        app.MapGet("customs-declarations/{mrn}/import-pre-notifications", GetImportPreNotifications)
            .WithName("ImportPreNotificationsByMrn")
            .WithTags(groupName)
            .WithSummary("Get ImportPreNotifications by MRN")
            .WithDescription("Get associated import pre-notifications by MRN")
            .Produces<ImportPreNotificationsResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization(PolicyNames.Read);

        app.MapPut("customs-declarations/{mrn}/", Put)
            .WithName("PutCustomsDeclaration")
            .WithTags(groupName)
            .WithSummary("Put CustomsDeclaration")
            .WithDescription("Put a Customs Declaration")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization(PolicyNames.Write);
    }

    /// <param name="mrn">MRN</param>
    /// <param name="context"></param>
    /// <param name="customsDeclarationService"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    private static async Task<IResult> Get(
        [FromRoute] string mrn,
        HttpContext context,
        [FromServices] ICustomsDeclarationService customsDeclarationService,
        CancellationToken cancellationToken
    )
    {
        var customsDeclarationEntity = await customsDeclarationService.GetCustomsDeclaration(mrn, cancellationToken);
        if (customsDeclarationEntity is null)
        {
            return Results.NotFound();
        }

        context.SetResponseEtag(customsDeclarationEntity.ETag);

        return Results.Ok(
            new CustomsDeclarationResponse(
                customsDeclarationEntity.Id,
                customsDeclarationEntity.ClearanceRequest,
                customsDeclarationEntity.ClearanceDecision,
                customsDeclarationEntity.Finalisation,
                customsDeclarationEntity.ExternalErrors,
                customsDeclarationEntity.Created,
                customsDeclarationEntity.Updated
            )
        );
    }

    /// <param name="mrn">MRN</param>
    /// <param name="context"></param>
    /// <param name="importPreNotificationService"></param>
    /// <param name="customsDeclarationService"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    private static async Task<IResult> GetImportPreNotifications(
        [FromRoute] string mrn,
        HttpContext context,
        [FromServices] IImportPreNotificationService importPreNotificationService,
        [FromServices] ICustomsDeclarationService customsDeclarationService,
        CancellationToken cancellationToken
    )
    {
        var importPreNotifications = await importPreNotificationService.GetImportPreNotificationsByMrn(
            mrn,
            cancellationToken
        );

        return Results.Ok(
            new ImportPreNotificationsResponse(
                importPreNotifications
                    .Select(x => new ImportPreNotificationResponse(x.ImportPreNotification, x.Created, x.Updated))
                    .ToList()
            )
        );
    }

    [HttpPut]
    private static async Task<IResult> Put(
        [FromRoute] string mrn,
        HttpContext context,
        [FromBody] Domain.CustomsDeclaration.CustomsDeclaration data,
        [FromHeader(Name = "If-Match")] string? ifMatch,
        [FromServices] ICustomsDeclarationService customsDeclarationService,
        CancellationToken cancellationToken
    )
    {
        var customsDeclarationEntity = new CustomsDeclarationEntity
        {
            Id = mrn,
            ClearanceRequest = data.ClearanceRequest,
            ClearanceDecision = data.ClearanceDecision,
            Finalisation = data.Finalisation,
            ExternalErrors = data.ExternalErrors,
        };

        var etag = ETags.ValidateAndParseFirst(ifMatch);

        try
        {
            if (string.IsNullOrEmpty(etag))
            {
                await customsDeclarationService.Insert(customsDeclarationEntity, cancellationToken);

                return Results.Created();
            }

            await customsDeclarationService.Update(customsDeclarationEntity, etag, cancellationToken);

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
