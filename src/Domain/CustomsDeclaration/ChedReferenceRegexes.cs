using System.Text.RegularExpressions;

namespace Defra.TradeImportsDataApi.Domain.CustomsDeclaration;

// <summary> Common Health Entry Document reference regular expressions. </summary>
public static partial class ChedReferenceRegexes
{
    [GeneratedRegex("\\d{7}(v|r)?$", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
    public static partial Regex DocumentReferenceIdentifier();
}
