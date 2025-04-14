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
}
