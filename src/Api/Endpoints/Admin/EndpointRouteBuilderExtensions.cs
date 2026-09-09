using Defra.TradeImportsDataApi.Api.Authentication;
using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using Microsoft.AspNetCore.Mvc;

namespace Defra.TradeImportsDataApi.Api.Endpoints.Admin;

public static class EndpointRouteBuilderExtensions
{
    private const int MaxChedIdLength = 8;

    public static void MapAdminEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("admin/max-id", MaxId).ExcludeFromDescription().RequireAuthorization(PolicyNames.Read);
    }

    [HttpGet]
    private static async Task<IResult> MaxId(
        [FromServices] IImportPreNotificationRepository importPreNotificationRepository,
        [FromServices] ITracesChedRepository tracesChedRepository,
        CancellationToken cancellationToken
    )
    {
        var maxImportPreNotificationId = await importPreNotificationRepository.GetMaxId(cancellationToken);
        var maxTracesChedId = await tracesChedRepository.GetMaxId(cancellationToken);

        var maxId = MaxByIdentifier(maxImportPreNotificationId, maxTracesChedId);

        return Results.Ok(new MaxIdResponse(maxId));
    }

    private static string? MaxByIdentifier(string? first, string? second)
    {
        if (string.IsNullOrEmpty(first) && string.IsNullOrEmpty(second))
            return null;
        if (string.IsNullOrEmpty(first))
            return second;
        if (string.IsNullOrEmpty(second))
            return first;

        var firstIdentifier = new ChedIdReference(first).GetIdentifier().PadLeft(MaxChedIdLength);
        var secondIdentifier = new ChedIdReference(second).GetIdentifier().PadLeft(MaxChedIdLength);

        return string.CompareOrdinal(firstIdentifier, secondIdentifier) >= 0 ? first : second;
    }
}
