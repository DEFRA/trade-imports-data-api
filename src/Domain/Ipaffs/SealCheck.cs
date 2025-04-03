using System.Text.Json.Serialization;
using Defra.TradeImportsData.Domain.Json;

namespace Defra.TradeImportsData.Domain.IPaffs;

/// <summary>
/// Details of the seal check
/// </summary>
public class SealCheck
{
    /// <summary>
    /// Is seal check satisfactory
    /// </summary>

    [JsonPropertyName("satisfactory")]
    [System.ComponentModel.Description("Is seal check satisfactory")]
    public bool? Satisfactory { get; set; }

    /// <summary>
    /// reason for not satisfactory
    /// </summary>

    [JsonPropertyName("reason")]
    [System.ComponentModel.Description("reason for not satisfactory")]
    public string? Reason { get; set; }

    /// <summary>
    /// Official inspector
    /// </summary>

    [JsonPropertyName("officialInspector")]
    [System.ComponentModel.Description("Official inspector")]
    public OfficialInspector? OfficialInspector { get; set; }

    /// <summary>
    /// date and time of seal check
    /// </summary>

    [JsonPropertyName("checkedOn")]
    [System.ComponentModel.Description("date and time of seal check")]
    [
        UnknownTimeZoneDateTimeJsonConverter(nameof(CheckedOn)),
        MongoDB.Bson.Serialization.Attributes.BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)
    ]
    public DateTime? CheckedOn { get; set; }
}
