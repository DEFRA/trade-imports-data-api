using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Domain.Json;

namespace Defra.TradeImportsDataApi.Domain.Gvms;

public class ActualCrossing
{
    /// <summary>
    /// The ID of the crossing route, using a routeId from the GVMS reference data
    /// </summary>

    [JsonPropertyName("routeId")]
    [System.ComponentModel.Description("The ID of the crossing route, using a routeId from the GVMS reference data")]
    public string? RouteId { get; set; }

    /// <summary>
    /// The planned date and time of arrival, in local time of the arrival port. Must not include seconds, time zone or UTC marker
    /// </summary>

    [JsonPropertyName("arrivesAt")]
    [System.ComponentModel.Description(
        "The planned date and time of arrival, in local time of the arrival port. Must not include seconds, time zone or UTC marker"
    )]
    [UnknownTimeZoneDateTimeJsonConverter(nameof(ArrivesAt))]
    public DateTime? ArrivesAt { get; set; }
}
