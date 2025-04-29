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
        const string groupName = "Search";
        var route = app.MapGet("search", Search)
            .WithName("Search")
            .WithTags(groupName)
            .WithSummary("Search")
            .WithDescription("Search")
            .Produces<SearchResponse>()
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
    /// <param name="searchService"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    private static async Task<IResult> Search(
        [AsParameters] SearchRequest request,
        [FromServices] ISearchService searchService,
        CancellationToken cancellationToken
    )
    {
        Console.WriteLine(request);
        var searchResults = await searchService.Search(request, cancellationToken);

        var response = new SearchResponse(
            searchResults.customsDeclaration.Select(x => new CustomsDeclarationResponse(
                x.Id,
                x.ClearanceRequest,
                x.ClearanceDecision,
                x.Finalisation,
                x.InboundError,
                x.Created,
                x.Updated
            )).ToArray(),
            searchResults.importPreNotifications.Select(x => new ImportPreNotificationResponse(
                x.ImportPreNotification,
                x.Created,
                x.Updated
            )).ToArray());

        return Results.Ok(response);
    }
}
