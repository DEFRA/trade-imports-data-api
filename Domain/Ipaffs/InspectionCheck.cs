using System.Text.Json.Serialization;

namespace Defra.TradeImportsData.Domain.IPaffs;

public class InspectionCheck
{
    /// <summary>
    /// Type of check
    /// </summary>

    [JsonPropertyName("type")]
    [System.ComponentModel.Description("Type of check")]
    [MongoDB.Bson.Serialization.Attributes.BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public InspectionCheckType? Type { get; set; }

    /// <summary>
    /// Status of the check
    /// </summary>

    [JsonPropertyName("status")]
    [System.ComponentModel.Description("Status of the check")]
    [MongoDB.Bson.Serialization.Attributes.BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public InspectionCheckStatus? Status { get; set; }

    /// <summary>
    /// Reason for the status if applicable
    /// </summary>

    [JsonPropertyName("reason")]
    [System.ComponentModel.Description("Reason for the status if applicable")]
    public string? Reason { get; set; }

    /// <summary>
    /// Other reason text when selected reason is &#x27;Other&#x27;
    /// </summary>

    [JsonPropertyName("otherReason")]
    [System.ComponentModel.Description("Other reason text when selected reason is 'Other'")]
    public string? OtherReason { get; set; }

    /// <summary>
    /// Has commodity been selected for checks?
    /// </summary>

    [JsonPropertyName("isSelectedForChecks")]
    [System.ComponentModel.Description("Has commodity been selected for checks?")]
    public bool? IsSelectedForChecks { get; set; }

    /// <summary>
    /// Has commodity completed this type of check
    /// </summary>

    [JsonPropertyName("hasChecksComplete")]
    [System.ComponentModel.Description("Has commodity completed this type of check")]
    public bool? HasChecksComplete { get; set; }
}
