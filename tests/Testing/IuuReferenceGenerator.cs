namespace Defra.TradeImportsDataApi.Testing;

public static class IuuReferenceGenerator
{
    private static int GenerateId()
    {
        // Random 7 digit
        return Random.Shared.Next(1000000, 10000000);
    }

    public static string Generate()
    {
        return $"IUU.FR.2026.{GenerateId()}";
    }
}
