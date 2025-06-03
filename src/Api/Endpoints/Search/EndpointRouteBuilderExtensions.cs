using Defra.TradeImportsDataApi.Api.Authentication;
using Defra.TradeImportsDataApi.Api.Endpoints.CustomsDeclarations;
using Defra.TradeImportsDataApi.Api.Endpoints.ImportPreNotifications;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Defra.TradeImportsDataApi.Api.Endpoints.Search;

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
