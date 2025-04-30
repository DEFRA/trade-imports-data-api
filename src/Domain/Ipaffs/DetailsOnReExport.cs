using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

/// <summary>
/// Details on re-export
/// </summary>
public class DetailsOnReExport
{
    /// <summary>
    /// Date of re-export
    /// </summary>
    [JsonPropertyName("date")]
    [System.ComponentModel.Description("Date of re-export")]
    public DateOnly? Date { get; set; }

    /// <summary>
    /// Number of vehicle
    /// </summary>
    [JsonPropertyName("meansOfTransportNo")]
    [System.ComponentModel.Description("Number of vehicle")]
    public string? MeansOfTransportNo { get; set; }

    /// <summary>
    /// Type of transport to be used
    /// </summary>
    [JsonPropertyName("transportType")]
    [System.ComponentModel.Description("Type of transport to be used")]
    public string? TransportType { get; set; }

    /// <summary>
    /// Document issued for re-export
    /// </summary>
    [JsonPropertyName("document")]
    [System.ComponentModel.Description("Document issued for re-export")]
    public string? Document { get; set; }

    /// <summary>
    /// Two letter ISO code for country of re-dispatching
    /// </summary>
    [JsonPropertyName("countryOfReDispatching")]
    [System.ComponentModel.Description("Two letter ISO code for country of re-dispatching")]
    public string? CountryOfReDispatching { get; set; }

    /// <summary>
    /// Exit BIP (where consignment will leave the country)
    /// </summary>
    [JsonPropertyName("exitBip")]
    [System.ComponentModel.Description("Exit BIP (where consignment will leave the country)")]
    public string? ExitBip { get; set; }
}
