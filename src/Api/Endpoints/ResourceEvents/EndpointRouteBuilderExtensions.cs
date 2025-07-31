using Defra.TradeImportsDataApi.Api.Authentication;
using Defra.TradeImportsDataApi.Api.Data;
using Microsoft.AspNetCore.Mvc;

namespace Defra.TradeImportsDataApi.Api.Endpoints.ResourceEvents;

public static class EndpointRouteBuilderExtensions
{
    public static void MapResourceEventEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("resource-events/{resourceId}", GetAll)
            .ExcludeFromDescription()
            .RequireAuthorization(PolicyNames.Read);

        app.MapGet("resource-events/{resourceId}/unpublished", GetUnpublished)
            .ExcludeFromDescription()
            .RequireAuthorization(PolicyNames.Read);

        app.MapPut("resource-events/{resourceId}/publish", Publish)
            .ExcludeFromDescription()
            .RequireAuthorization(PolicyNames.Write);
    }

    [HttpGet]
    private static async Task<IResult> GetAll(
        [FromRoute] string resourceId,
        [FromServices] IResourceEventRepository resourceEventRepository,
        CancellationToken cancellationToken
    )
    {
        var resourceEvents = await resourceEventRepository.GetAll(resourceId, cancellationToken);

        return Results.Ok(resourceEvents);
    }

    [HttpGet]
    private static async Task<IResult> GetUnpublished(
        [FromRoute] string resourceId,
        [FromServices] IResourceEventRepository resourceEventRepository,
        CancellationToken cancellationToken
    )
    {
        var resourceEvents = await resourceEventRepository.GetAll(resourceId, cancellationToken);

        return Results.Ok(resourceEvents.Where(x => x.Published is null));
    }

    [HttpPut]
    private static async Task<IResult> Publish(
        [FromRoute] string resourceId,
        [FromQuery] string? resourceEventId,
        [FromServices] IResourceEventRepository resourceEventRepository,
        CancellationToken cancellationToken
    )
    {
        var resourceEvents = await resourceEventRepository.GetAll(resourceId, cancellationToken);
        var resourceEvent = resourceEvents.FirstOrDefault(x => x.Id == resourceEventId);

        if (resourceEvent is null)
            return Results.NotFound();

        return resourceEvent.Published is not null
            ? Results.Conflict("Resource event already published")
            : Results.NoContent();
    }
}
