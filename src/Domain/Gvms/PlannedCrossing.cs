using System.Text.Json.Serialization;
using Defra.TradeImportsData.Domain.Json;

namespace Defra.TradeImportsData.Domain.Gvms;

public class PlannedCrossing
{
    /// <summary>
    /// The ID of the crossing route, using a routeId from the GVMS reference data
    /// </summary>

    [JsonPropertyName("routeId")]
    [System.ComponentModel.Description("The ID of the crossing route, using a routeId from the GVMS reference data")]
    public string? RouteId { get; set; }

    /// <summary>
    /// The planned date and time of departure, in local time of the departure port. Must not include seconds, time zone or UTC marker
    /// </summary>

    [JsonPropertyName("departsAt")]
    [System.ComponentModel.Description(
        "The planned date and time of departure, in local time of the departure port. Must not include seconds, time zone or UTC marker"
    )]
    [
        UnknownTimeZoneDateTimeJsonConverter(nameof(DepartsAt)),
        MongoDB.Bson.Serialization.Attributes.BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)
    ]
    public DateTime? DepartsAt { get; set; }
}
