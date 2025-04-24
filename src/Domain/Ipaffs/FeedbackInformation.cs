using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

/// <summary>
/// Feedback information from control
/// </summary>
public class FeedbackInformation
{
    /// <summary>
    /// Type of authority
    /// </summary>
    [JsonPropertyName("authorityType")]
    [System.ComponentModel.Description("Type of authority")]
    public FeedbackInformationAuthorityType? AuthorityType { get; set; }

    /// <summary>
    /// Did the consignment arrive
    /// </summary>
    [JsonPropertyName("consignmentArrival")]
    [System.ComponentModel.Description("Did the consignment arrive")]
    public bool? ConsignmentArrival { get; set; }

    /// <summary>
    /// Does the consignment conform
    /// </summary>
    [JsonPropertyName("consignmentConformity")]
    [System.ComponentModel.Description("Does the consignment conform")]
    public bool? ConsignmentConformity { get; set; }

    /// <summary>
    /// Reason for consignment not arriving at the entry point
    /// </summary>
    [JsonPropertyName("consignmentNoArrivalReason")]
    [System.ComponentModel.Description("Reason for consignment not arriving at the entry point")]
    public string? ConsignmentNoArrivalReason { get; set; }

    /// <summary>
    /// Date of consignment destruction
    /// </summary>
    [JsonPropertyName("destructionDate")]
    [System.ComponentModel.Description("Date of consignment destruction")]
    public string? DestructionDate { get; set; }
}
