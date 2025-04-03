using System.Text.Json.Serialization;

namespace Defra.TradeImportsData.Domain.IPaffs;

/// <summary>
/// Present if the consignment has been split
/// </summary>
public class SplitConsignment
{
    /// <summary>
    /// Reference number of the valid split consignment
    /// </summary>

    [JsonPropertyName("validReferenceNumber")]
    [System.ComponentModel.Description("Reference number of the valid split consignment")]
    public string? ValidReferenceNumber { get; set; }

    /// <summary>
    /// Reference number of the rejected split consignment
    /// </summary>

    [JsonPropertyName("rejectedReferenceNumber")]
    [System.ComponentModel.Description("Reference number of the rejected split consignment")]
    public string? RejectedReferenceNumber { get; set; }
}
