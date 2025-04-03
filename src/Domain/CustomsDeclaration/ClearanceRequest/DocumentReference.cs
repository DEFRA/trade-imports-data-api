namespace Defra.TradeImportsData.Domain.CustomsDeclaration.ClearanceRequest;

public class DocumentReference(string value)
{
    public string Value { get; set; } = value;

    public bool IsValid()
    {
        return DocumentReferenceRegularExpressions.IPaffsIdentifier().IsExactMatch(Value)
            || DocumentReferenceRegularExpressions.DocumentReferenceWithoutCountry().IsExactMatch(Value);
    }

    public string GetIdentifier()
    {
        if (IsValid())
        {
            var identifier = DocumentReferenceRegularExpressions
                .DocumentReferenceIdentifier()
                .Match(Value)
                .Value.Replace(".", "");
            if (identifier.Length == 9)
            {
                identifier = $"20{identifier}";
            }

            return identifier;
        }
        return string.Empty;
    }
}
