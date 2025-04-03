using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

public class IdentificationDetails
{
    /// <summary>
    /// Identification detail
    /// </summary>

    [JsonPropertyName("identificationDetail")]
    [System.ComponentModel.Description("Identification detail")]
    public string? IdentificationDetail { get; set; }

    /// <summary>
    /// Identification description
    /// </summary>

    [JsonPropertyName("identificationDescription")]
    [System.ComponentModel.Description("Identification description")]
    public string? IdentificationDescription { get; set; }
}
