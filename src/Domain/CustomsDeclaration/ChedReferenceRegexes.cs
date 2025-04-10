using System.Text.RegularExpressions;

namespace Defra.TradeImportsDataApi.Domain.CustomsDeclaration;

// <summary> Common Health Entry Document reference regular expressions. </summary>
public static partial class ChedReferenceRegexes
{
    [GeneratedRegex(
        "(CHEDD|CHEDA|CHEDP|CHEDPP)\\.?GB\\.?(20|21)\\d{2}\\.?\\d{7}[rv]?",
        RegexOptions.Compiled | RegexOptions.IgnoreCase
    )]
    internal static partial Regex IPaffsIdentifier();

    [GeneratedRegex("[gbchdv]{5}\\.?(20|21)?\\d{2}\\.?\\d{7}[rv]?", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
    internal static partial Regex DocumentReferenceWithoutCountry();

    [GeneratedRegex("\\d{2,4}\\.?\\d{7}", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
    internal static partial Regex DocumentReferenceIdentifier();

    public static bool IsExactMatch(this Regex regex, string input)
    {
        return regex.Match(input).Value == input;
    }
}
