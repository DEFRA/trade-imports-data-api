using System.Text.Json.Serialization;

namespace Defra.TradeImportsData.Domain.Gvms;

public class Customs
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
}
