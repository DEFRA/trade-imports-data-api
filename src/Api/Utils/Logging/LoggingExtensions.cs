using System.Diagnostics.CodeAnalysis;

namespace Defra.TradeImportsDataApi.Api.Utils.Logging;

[SuppressMessage("Major Code Smell", "S2629:Logging templates should be constant")]
public static class LoggingExtensions
{
    public static void LogInformationWithPrefix(this ILogger logger, string message)
    {
        logger.LogInformation($"{{Prefix}} {message}");
    }
}
