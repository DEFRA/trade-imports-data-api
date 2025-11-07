using Defra.TradeImportsDataApi.Api.Authentication;
using Defra.TradeImportsDataApi.Api.Endpoints.CustomsDeclarations;
using Defra.TradeImportsDataApi.Api.Endpoints.Gmrs;
using Defra.TradeImportsDataApi.Api.Endpoints.ImportPreNotifications;
using Defra.TradeImportsDataApi.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Defra.TradeImportsDataApi.Api.Endpoints.RelatedImportDeclarations;

public static class EndpointRouteBuilderExtensions
{
    public static void MapSearchEndpoints(this IEndpointRouteBuilder app)
    {
        const string groupName = "RelatedImportDeclarations";

        app.MapGet("related-import-declarations", Search)
            .WithName("GetRelatedImportDeclarations")
            .WithTags(groupName)
            .WithSummary("Get RelatedImportDeclarations")
            .WithDescription("Get all import declarations that are related to each other")
            .Produces<RelatedImportDeclarationsResponse>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization(PolicyNames.Read);
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
                .CustomsDeclarations.Select(x => new CustomsDeclarationResponse(
                    x.Id,
                    x.ClearanceRequest,
                    x.ClearanceDecision,
                    x.Finalisation,
                    x.ExternalErrors,
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
                .ToArray(),
            searchResults.Gmrs.Select(x => new GmrResponse(x.Gmr, x.Created, x.Updated)).ToArray()
        );

        return Results.Ok(response);
    }
}
