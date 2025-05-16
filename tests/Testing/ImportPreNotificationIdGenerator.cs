namespace Defra.TradeImportsDataApi.Testing;

public static class ImportPreNotificationIdGenerator
{
    public static string Generate()
    {
        // Random 7 digit suffix is valid
        return $"CHEDA.GB.2025.{Random.Shared.Next(1000000, 10000000)}";
    }
}
