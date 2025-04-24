using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

public class ChedIdReference(string value)
{
    public string Value { get; set; } = value;

    public string GetIdentifier()
    {
        var identifier = ChedReferenceRegexes.DocumentReferenceIdentifier().Match(Value).Value.Replace(".", "");

        if (identifier.Length > 7)
        {
            identifier = identifier.Substring(identifier.Length - 7);
        }

        return identifier;
    }
}
