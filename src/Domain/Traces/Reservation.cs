using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Traces;

public class Reservation
{
    [JsonPropertyName("chedId")]
    public required string ChedId { get; set; }

    [JsonPropertyName("mrn")]
    public required string Mrn { get; set; }

    [JsonPropertyName("status")]
    public required string Status { get; set; }

    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("commodities")]
    public required ReservationCommodity[] Commodities { get; set; } = [];
}
