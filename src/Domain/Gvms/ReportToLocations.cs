using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Gvms;

/// <summary>
/// Information about an inspection that is required
/// </summary>
public class ReportToLocations
{
    /// <summary>
    /// An inspectionTypeId from GVMS Reference Data denoting the type of inspection that needs to be performed on the vehicle.
    /// </summary>

    [JsonPropertyName("inspectionTypeId")]
    [System.ComponentModel.Description(
        "An inspectionTypeId from GVMS Reference Data denoting the type of inspection that needs to be performed on the vehicle."
    )]
    public string? InspectionTypeId { get; set; }

    /// <summary>
    /// A list of locationIds from GVMS Reference Data that are available to perform this type of inspection.
    /// </summary>

    [JsonPropertyName("locationIds")]
    [System.ComponentModel.Description(
        "A list of locationIds from GVMS Reference Data that are available to perform this type of inspection."
    )]
    public string[]? LocationIds { get; set; }
}
