using System.Text;

namespace Defra.TradeImportsDataApi.Testing;

public static class ImportPreNotificationIdGenerator
{
    private static string GenerateId()
    {
        var rnd = new Random();
        var sb = new StringBuilder();
        for (var i = 0; i < 7; i++)
        {
            sb.Append(rnd.Next(9));
        }
        return sb.ToString();
    }

    public static string Generate()
    {
        return $"CHEDA.GB.2025.{GenerateId()}";
    }

    public static (string, string) GenerateReturnId()
    {
        var id = GenerateId();
        return ($"CHEDA.GB.2025.{id}", id);
    }
}
