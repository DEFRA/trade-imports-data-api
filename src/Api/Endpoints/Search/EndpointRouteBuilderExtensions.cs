using System.Diagnostics.CodeAnalysis;
using Defra.TradeImportsDataApi.Api.Authentication;
using Defra.TradeImportsDataApi.Api.Endpoints.CustomsDeclarations;
using Defra.TradeImportsDataApi.Api.Endpoints.ImportPreNotifications;
using Defra.TradeImportsDataApi.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Defra.TradeImportsDataApi.Api.Endpoints.Search;

public static class EndpointRouteBuilderExtensions
{
    public static void MapSearchEndpoints(this IEndpointRouteBuilder app, bool isDevelopment)
    {
        const string groupName = "RelatedImportDeclarations";
        var route = app.MapGet("related-import-declarations", Search)
            .WithName("related-import-declarations")
            .WithTags(groupName)
            .WithSummary("related-import-declarations")
            .WithDescription("related-import-declarations")
            .Produces<RelatedImportDeclarationsResponse>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization(PolicyNames.Read);

        AllowAnonymousForDevelopment(isDevelopment, route);
    }

    [ExcludeFromCodeCoverage]
    private static void AllowAnonymousForDevelopment(bool isDevelopment, RouteHandlerBuilder route)
    {
        if (isDevelopment)
            route.AllowAnonymous();
    }

    /// <param name="request"></param>
    /// <param name="relatedImportDeclarationsService"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    private static async Task<IResult> Search(
        [AsParameters] RelatedImportDeclarationsRequest request,
        [FromServices] IRelatedImportDeclarationsService relatedImportDeclarationsService,
        CancellationToken cancellationToken
    )
    {
        var searchResults = await relatedImportDeclarationsService.Search(request, cancellationToken);

        var response = new RelatedImportDeclarationsResponse(
            searchResults
                .CustomsDeclaration.Select(x => new CustomsDeclarationResponse(
                    x.Id,
                    x.ClearanceRequest,
                    x.ClearanceDecision,
                    x.Finalisation,
                    x.InboundError,
                    x.Created,
                    x.Updated
                ))
                .ToArray(),
            searchResults
                .ImportPreNotifications.Select(x => new ImportPreNotificationResponse(
                    x.ImportPreNotification,
                    x.Created,
                    x.Updated
                ))
                .ToArray()
        );

        return Results.Ok(response);
    }
}
