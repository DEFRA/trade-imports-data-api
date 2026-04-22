using System.Text.RegularExpressions;

namespace Defra.TradeImportsDataApi.Data.Extensions;

public partial class MrnValidator
{
    [GeneratedRegex(
        pattern: "^\\d{2}[A-Z]{2}[A-Z0-9]{14}$",
        matchTimeoutMilliseconds: 2000,
        options: RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase
    )]
    private partial Regex MrnRegex();

    public bool IsValid(string mrn) => MrnRegex().IsMatch(mrn);
}
