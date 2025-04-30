using System.Web;

namespace Defra.TradeImportsDataApi.Api.Client;

internal static class Endpoints
{
    public static string ImportPreNotifications(string chedId) => $"/import-pre-notifications/{chedId}";

    public static string CustomsDeclarationsByChed(string chedId) =>
        $"/import-pre-notifications/{chedId}/customs-declarations";

    public static string Gmrs(string gmrId) => $"/gmrs/{gmrId}";

    public static string CustomsDeclarations(string mrn) => $"/customs-declarations/{mrn}";

    public static string ImportPreNotificationsByMrn(string mrn) =>
        $"/customs-declarations/{mrn}/import-pre-notifications";

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

        return properties.Any() ? $"?{string.Join("&", properties.ToArray())}" : string.Empty;
    }
}
