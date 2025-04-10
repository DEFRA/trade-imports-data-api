using Microsoft.Net.Http.Headers;

namespace Defra.TradeImportsDataApi.Api.Utils;

public static class ETags
{
    /// <summary>
    /// Expected format of If-Match is array of double quote delimited strings. We
    /// want to parse the specified value and use the <see cref="EntityTagHeaderValue"/>
    /// validation as we only expect a single etag being specified.
    ///
    /// Validation will trigger if multiple values are present, and we want to
    /// return a single valid value with double quotes removed.
    /// </summary>
    /// <example>"etag1", "etag2"</example>
    /// <param name="ifMatch"></param>
    /// <returns></returns>
    public static string? ValidateAndParseFirst(string? ifMatch)
    {
        if (string.IsNullOrEmpty(ifMatch))
            return null;

        var etag = EntityTagHeaderValue.Parse(ifMatch);

        return etag.Tag.ToString().Replace("\"", "");
    }
}
