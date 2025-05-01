namespace Defra.TradeImportsDataApi.Domain.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class PossibleValueAttribute(string value) : Attribute
{
    public string Value { get; } = value;
}
