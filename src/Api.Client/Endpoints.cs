namespace Defra.TradeImportsDataApi.Api.Client;

internal static class Endpoints
{
    public static string ImportNotifications(string chedId) => $"/import-notifications/{chedId}";
}
