using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

public class ChedIdReference(string value)
{
    public string Value { get; set; } = value;

    public string GetIdentifier()
    {
        var identifier = ChedReferenceRegexes.DocumentReferenceIdentifier().Match(Value);

        if (identifier.Length == 0)
        {
            throw new FormatException($"Invalid value {Value}");
        }

        return identifier.Value;
    }
}
