using System.Text.Json.Serialization;

namespace Defra.TradeImportsData.Domain.IPaffs;

/// <summary>
/// Details of transport
/// </summary>
public class MeansOfTransport
{
    /// <summary>
    /// Type of transport
    /// </summary>

    [JsonPropertyName("type")]
    [System.ComponentModel.Description("Type of transport")]
    [MongoDB.Bson.Serialization.Attributes.BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public MeansOfTransportType? Type { get; set; }

    /// <summary>
    /// Document for transport
    /// </summary>

    [JsonPropertyName("document")]
    [System.ComponentModel.Description("Document for transport")]
    public string? Document { get; set; }

    /// <summary>
    /// ID of transport
    /// </summary>

    [JsonPropertyName("id")]
    [System.ComponentModel.Description("ID of transport")]
    public string? Id { get; set; }
}
