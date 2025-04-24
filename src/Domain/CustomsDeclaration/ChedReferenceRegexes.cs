using System.Text.RegularExpressions;

namespace Defra.TradeImportsDataApi.Domain.CustomsDeclaration;

// <summary> Common Health Entry Document reference regular expressions. </summary>
public static partial class ChedReferenceRegexes
{
    [GeneratedRegex("\\d{2,4}\\.?\\d{7}", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
    internal static partial Regex DocumentReferenceIdentifier();
}
