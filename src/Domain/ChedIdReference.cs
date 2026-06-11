using System.Text.RegularExpressions;

namespace Defra.TradeImportsDataApi.Domain;

internal static class ChedAndClearanceRequestHelper
{
    private static readonly Regex IdentifierRegex = new(@"(\d+)([RV])?$", RegexOptions.Compiled);

    public static string GetIdentifier(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        var v = value.Split('.').LastOrDefault();

        if (string.IsNullOrWhiteSpace(v))
            return string.Empty;

        var match = IdentifierRegex.Match(v);
        if (!match.Success)
            return string.Empty;

        var digits = match.Groups[1].Value;
        var suffix = match.Groups[2].Value;

        // Strip year prefix if present
        if (digits.Length is 11 or 12)
        {
            var year = digits[..4];

            if (int.TryParse(year, out var y) && y >= 2000 && y <= 2099)
            {
                digits = digits[4..];
            }
        }

        switch (digits.Length)
        {
            case < 7:
                return string.Empty;
            case > 8:
                digits = digits[^8..];
                break;
        }

        return digits + suffix;
    }
}
