using Defra.TradeImportsDataApi.Api.Authentication;
using Defra.TradeImportsDataApi.Api.Data;
using Microsoft.AspNetCore.Mvc;

namespace Defra.TradeImportsDataApi.Api.Endpoints.Admin;

public static class EndpointRouteBuilderExtensions
{
    public static void MapAdminEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("admin/max-id", MaxId).ExcludeFromDescription().RequireAuthorization(PolicyNames.Read);
    }

    [HttpGet]
    private static async Task<IResult> MaxId(
        [FromServices] IImportPreNotificationRepository importPreNotificationRepository,
        CancellationToken cancellationToken
    )
    {
        var maxId = await importPreNotificationRepository.GetMaxId(cancellationToken);

        return Results.Ok(new MaxIdResponse(maxId));
    }
}
