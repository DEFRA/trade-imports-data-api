namespace Defra.TradeImportsDataApi.Data.Configuration;

[AttributeUsage(AttributeTargets.Class)]
public class DbCollectionAttribute(string name) : Attribute
{
    public string Name { get; private set; } = name;
}
