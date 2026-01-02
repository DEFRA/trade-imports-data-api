namespace Defra.TradeImportsDataApi.Domain.CustomsDeclaration;

public class ImportDocumentReference(string value)
{
    private static readonly string[] _validDocumentCodes =
    [
        "C640",
        "C678",
        "N853",
        "N851",
        "N852",
        "9115",
        "C085",
        "N002",
    ];

    public string Value { get; set; } = value;

    public static bool IsValid(string documentCode)
    {
        return _validDocumentCodes.Contains(documentCode);
    }

    public string GetIdentifier(string documentCode)
    {
        return !IsValid(documentCode) ? string.Empty : GetLast7Digits();
    }

    private string GetLast7Digits()
    {
        var input = Value;
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        ReadOnlySpan<char> span = input.AsSpan();
        int i = span.Length - 1;

        // Optional suffix
        char suffix = '\0';
        bool hasSuffix = false;

        if ((uint)i < (uint)span.Length && char.IsLetter(span[i]))
        {
            suffix = span[i];
            hasSuffix = suffix is 'R' or 'V';
            i--;
        }

        // Count trailing digits
        int digitEnd = i;
        while (i >= 0 && char.IsDigit(span[i]))
            i--;

        int digitCount = digitEnd - i;
        if (digitCount < 7)
            return string.Empty;

        ReadOnlySpan<char> last7 = span.Slice(digitEnd - 6, 7);

        return hasSuffix ? last7.ToString() + suffix : last7.ToString();
    }
}
