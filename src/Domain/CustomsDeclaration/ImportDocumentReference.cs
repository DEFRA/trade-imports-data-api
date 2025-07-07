namespace Defra.TradeImportsDataApi.Domain.CustomsDeclaration;

public class ImportDocumentReference(string value)
{
    private static readonly string[] _validDocumentCodes = ["C640", "C678", "N853", "N851", "9115", "C085", "N002"];

    public string Value { get; set; } = value;

    public static bool IsValid(string documentCode)
    {
        return _validDocumentCodes.Contains(documentCode);
    }

    public string GetIdentifier(string documentCode)
    {
        if (!IsValid(documentCode))
            return string.Empty;
        var identifier = ChedReferenceRegexes.DocumentReferenceIdentifier().Match(Value);
        return identifier.Value;
    }
}
