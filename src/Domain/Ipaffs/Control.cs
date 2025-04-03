using System.Text.Json.Serialization;

namespace Defra.TradeImportsData.Domain.IPaffs;

/// <summary>
/// Details of Control (Part 3)
/// </summary>
public class Control
{
    /// <summary>
    /// Feedback information of Control
    /// </summary>

    [JsonPropertyName("feedbackInformation")]
    [System.ComponentModel.Description("Feedback information of Control")]
    public FeedbackInformation? FeedbackInformation { get; set; }

    /// <summary>
    /// Details on re-export
    /// </summary>

    [JsonPropertyName("detailsOnReExport")]
    [System.ComponentModel.Description("Details on re-export")]
    public DetailsOnReExport? DetailsOnReExport { get; set; }

    /// <summary>
    /// Official inspector
    /// </summary>

    [JsonPropertyName("officialInspector")]
    [System.ComponentModel.Description("Official inspector")]
    public OfficialInspector? OfficialInspector { get; set; }

    /// <summary>
    /// Is the consignment leaving UK borders?
    /// </summary>

    [JsonPropertyName("consignmentLeave")]
    [System.ComponentModel.Description("Is the consignment leaving UK borders?")]
    [MongoDB.Bson.Serialization.Attributes.BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public ControlConsignmentLeave? ConsignmentLeave { get; set; }
}
