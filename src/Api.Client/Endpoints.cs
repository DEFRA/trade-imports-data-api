namespace Defra.TradeImportsDataApi.Api.Client;

internal static class Endpoints
{
    public static string ImportPreNotifications(string chedId) => $"/import-pre-notifications/{chedId}";

    public static string Gmrs(string gmrId) => $"/gmrs/{gmrId}";
}
