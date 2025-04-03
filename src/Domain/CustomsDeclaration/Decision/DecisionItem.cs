using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.CustomsDeclaration.Decision;

public class DecisionItem
{
    [JsonPropertyName("itemNumber")]
    public int ItemNumber { get; set; }

    [JsonPropertyName("checks")]
    public required DecisionCheck[] Checks { get; set; }
}
