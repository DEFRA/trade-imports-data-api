namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

public class ChedIdReference(string value)
{
    public string Value { get; set; } = value;

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
