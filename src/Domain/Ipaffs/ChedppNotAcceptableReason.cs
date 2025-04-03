using System.Text.Json.Serialization;

namespace Defra.TradeImportsData.Domain.IPaffs;

/// <summary>
/// Information about not acceptable reason
/// </summary>
public class ChedppNotAcceptableReason
{
    /// <summary>
    /// reason for refusal
    /// </summary>

    [JsonPropertyName("reason")]
    [System.ComponentModel.Description("reason for refusal")]
    [MongoDB.Bson.Serialization.Attributes.BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public ChedppNotAcceptableReasonReason? Reason { get; set; }

    /// <summary>
    /// commodity or package
    /// </summary>

    [JsonPropertyName("commodityOrPackage")]
    [System.ComponentModel.Description("commodity or package")]
    [MongoDB.Bson.Serialization.Attributes.BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public ChedppNotAcceptableReasonCommodityOrPackage? CommodityOrPackage { get; set; }
}
