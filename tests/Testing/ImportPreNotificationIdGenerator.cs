using System.Text;

namespace Defra.TradeImportsDataApi.Testing;

public static class ImportPreNotificationIdGenerator
{
    public static string Generate()
    {
        Random rnd = new Random();
        var sb = new StringBuilder();
        for (int i = 0; i < 7; i++)
        {
            sb.Append(rnd.Next(9));
        }

        return $"CHEDA.GB.2025.{sb}";
    }
}
