namespace Defra.TradeImportsDataApi.Domain.CustomsDeclaration;

public class ImportDocumentReference(string value)
{
    public string Value { get; set; } = value;

    public bool IsValid()
    {
        return ChedReferenceRegexes.IPaffsIdentifier().IsExactMatch(Value)
            || ChedReferenceRegexes.DocumentReferenceWithoutCountry().IsExactMatch(Value);
    }

    public string GetIdentifier()
    {
        if (IsValid())
        {
            var identifier = ChedReferenceRegexes.DocumentReferenceIdentifier().Match(Value).Value.Replace(".", "");
            if (identifier.Length == 9)
            {
                identifier = $"20{identifier}";
            }

            return identifier;
        }
        return string.Empty;
    }
}
