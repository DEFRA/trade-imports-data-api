using System.Diagnostics.CodeAnalysis;
using Defra.TradeImportsDataApi.Api.Authentication;
using Defra.TradeImportsDataApi.Api.Extensions;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Api.Utils;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Defra.TradeImportsDataApi.Api.Endpoints.CustomsDeclarations;

public static class EndpointRouteBuilderExtensions
{
    public static void MapCustomsDeclarationEndpoints(this IEndpointRouteBuilder app, bool isDevelopment)
    {
        var route = app.MapGet("customs-declarations/{mrn}/", Get)
            .WithName("CustomsDeclarationByMrn")
            .WithTags("CustomsDeclarations")
            .WithSummary("Get CustomsDeclaration")
            .WithDescription("Get a Customs Declaration by MRN")
            .Produces<CustomsDeclarationResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization(PolicyNames.Read);

        AllowAnonymousForDevelopment(isDevelopment, route);

        route = app.MapPut("customs-declarations/{mrn}/", Put)
            .WithName("PutCustomsDeclaration")
            .WithTags("CustomsDeclarations")
            .WithSummary("Put CustomsDeclaration")
            .WithDescription("Put a Customs Declaration")
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
                customsDeclarationEntity.Created,
                customsDeclarationEntity.Updated
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
    }
}
