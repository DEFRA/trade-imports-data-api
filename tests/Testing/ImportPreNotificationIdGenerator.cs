namespace Defra.TradeImportsDataApi.Testing;

public static class ImportPreNotificationIdGenerator
{
    private static int GenerateId()
    {
        // Random 7 digit
        return Random.Shared.Next(1000000, 10000000);
    }

    public static string Generate()
    {
        return $"CHEDA.GB.2025.{GenerateId()}";
    }

    public static (string, string) GenerateReturnId()
    {
        var id = GenerateId();

        return ($"CHEDA.GB.2025.{id}", id.ToString());
    }
}
