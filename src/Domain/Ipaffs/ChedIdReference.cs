using System.Text.RegularExpressions;
using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

public partial class ChedIdReference(string value)
{
    [GeneratedRegex(
        pattern: @"^CHED(A|P|D|PP)\.[A-Z]{2}\.\d{4}\.\d{7}([VR])?$",
        matchTimeoutMilliseconds: 2000,
        options: RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase
    )]
    private partial Regex ChedIdRegex();

    public string Value { get; set; } = value;

    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(Value) && ChedIdRegex().IsMatch(Value.Trim());
    }

    public string GetIdentifier()
    {
        var identifier = ChedAndClearanceRequestHelper.GetIdentifier(Value);

        if (string.IsNullOrWhiteSpace(identifier))
        {
            throw new FormatException($"Invalid value {Value}");
        }

        return identifier;
    }
}
