using Defra.TradeImportsDataApi.Api.Authentication;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Defra.TradeImportsDataApi.Api.Endpoints.Admin;

public static class EndpointRouteBuilderExtensions
{
    public static void MapAdminEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("admin/latest", Latest).ExcludeFromDescription().RequireAuthorization(PolicyNames.Read);
    }

    [HttpGet]
    private static async Task<IResult> Latest([FromServices] IDbContext dbContext, CancellationToken cancellationToken)
    {
        var latestNotification = await dbContext
            .ImportPreNotifications.OrderByDescending(x => x.Updated)
            .FirstOrDefaultWithFallbackAsync(cancellationToken);

        var latestCustomsDeclaration = await dbContext
            .CustomsDeclarations.OrderByDescending(x => x.Updated)
            .FirstOrDefaultWithFallbackAsync(cancellationToken);

        var latestGmr = await dbContext
            .Gmrs.OrderByDescending(x => x.Updated)
            .FirstOrDefaultWithFallbackAsync(cancellationToken);

        var latestProcessingError = await dbContext
            .ProcessingErrors.OrderByDescending(x => x.Updated)
            .FirstOrDefaultWithFallbackAsync(cancellationToken);

        return Results.Ok(
            new LatestResponse(
                latestNotification?.Id,
                latestCustomsDeclaration?.Id,
                latestGmr?.Id,
                latestProcessingError?.Id
            )
        );
    }
}
