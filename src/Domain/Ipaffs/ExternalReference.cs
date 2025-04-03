using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

/// <summary>
/// Reference number from an external system which is related to this notification
/// </summary>
public class ExternalReference
{
    /// <summary>
    /// Identifier of the external system to which the reference relates
    /// </summary>

    [JsonPropertyName("system")]
    [System.ComponentModel.Description("Identifier of the external system to which the reference relates")]
    public ExternalReferenceSystem? System { get; set; }

    /// <summary>
    /// Reference which is added to the notification when either sent to the downstream system or received from it
    /// </summary>

    [JsonPropertyName("reference")]
    [System.ComponentModel.Description(
        "Reference which is added to the notification when either sent to the downstream system or received from it"
    )]
    public string? Reference { get; set; }

    /// <summary>
    /// Details whether there&#x27;s an exact match between the external source and IPAFFS data
    /// </summary>

    [JsonPropertyName("exactMatch")]
    [System.ComponentModel.Description(
        "Details whether there's an exact match between the external source and IPAFFS data"
    )]
    public bool? ExactMatch { get; set; }

    /// <summary>
    /// Details whether an importer has verified the data from an external source
    /// </summary>

    [JsonPropertyName("verifiedByImporter")]
    [System.ComponentModel.Description("Details whether an importer has verified the data from an external source")]
    public bool? VerifiedByImporter { get; set; }

    /// <summary>
    /// Details whether an inspector has verified the data from an external source
    /// </summary>

    [JsonPropertyName("verifiedByInspector")]
    [System.ComponentModel.Description("Details whether an inspector has verified the data from an external source")]
    public bool? VerifiedByInspector { get; set; }
}
