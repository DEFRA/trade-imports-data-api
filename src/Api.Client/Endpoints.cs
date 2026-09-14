using System.Web;

namespace Defra.TradeImportsDataApi.Api.Client;

internal static class Endpoints
{
    public static string ImportPreNotifications(string chedId) => $"/import-pre-notifications/{chedId}";

    public static string TracesCheds(string chedId) => $"/traces-cheds/{chedId}";

    public static string CustomsDeclarationsByTracesChed(string chedId) =>
        $"/traces-cheds/{chedId}/customs-declarations";

    public static string CustomsDeclarationsByChed(string chedId) =>
        $"/import-pre-notifications/{chedId}/customs-declarations";

    public static string GmrsByChed(string chedId) => $"/import-pre-notifications/{chedId}/gmrs";

    public static string Gmrs(string gmrId) => $"/gmrs/{gmrId}";

    public static string CustomsDeclarations(string mrn) => $"/customs-declarations/{mrn}";

    public static string ImportPreNotificationsByMrn(string mrn) =>
        $"/customs-declarations/{mrn}/import-pre-notifications";

    public static string TracesChedsByMrn(string mrn) => $"/customs-declarations/{mrn}/traces-cheds";

    public static string TracesChedUpdates(TracesChedUpdatesRequest request)
    {
        // Dates must be ISO 8601 UTC, which the reflection based query string builder below cannot produce
        var query = new List<string> { $"from={request.From:O}", $"to={request.To:O}" };

        if (request.Page is not null)
            query.Add($"page={request.Page}");

        if (request.PageSize is not null)
            query.Add($"pageSize={request.PageSize}");

        return $"/traces-ched-updates?{string.Join("&", query)}";
    }

    public static string ProcessingErrors(string mrn) => $"/processing-errors/{mrn}";

    public static string RelatedImportDeclarations(RelatedImportDeclarationsRequest request) =>
        $"/related-import-declarations{BuildQueryString(request)}";

    private static string BuildQueryString(object o)
    {
        var properties = (
            from p in o.GetType().GetProperties()
            where p.GetValue(o, null) != null
            select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(o, null)?.ToString())
        ).ToList();

        return properties.Count != 0 ? $"?{string.Join("&", properties.ToArray())}" : string.Empty;
    }
}
