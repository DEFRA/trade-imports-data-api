using System.Text.Json.Serialization;

namespace Defra.TradeImportsData.Domain.CustomsDeclaration.Decision;

public class Item
{
    [JsonPropertyName("itemNumber")]
    public int ItemNumber { get; set; }

    [JsonPropertyName("checks")]
    public required Check[] Checks { get; set; }
}
