using Defra.TradeImportsDataApi.Api.Authentication;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Defra.TradeImportsDataApi.Api.Endpoints.Admin;

public static class EndpointRouteBuilderExtensions
{
    public static void MapAdminEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("admin/max-id", MaxId).ExcludeFromDescription().RequireAuthorization(PolicyNames.Read);
    }

    [HttpGet]
    private static async Task<IResult> MaxId([FromServices] IDbContext dbContext, CancellationToken cancellationToken)
    {
        var latestNotification = await dbContext
            .ImportPreNotifications.OrderByDescending(x => x.CustomsDeclarationIdentifier)
            .FirstOrDefaultWithFallbackAsync(cancellationToken);

        return Results.Ok(new MaxIdResponse(latestNotification?.Id));
    }
}
