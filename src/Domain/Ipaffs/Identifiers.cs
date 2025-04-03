using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

public class Identifiers
{
    /// <summary>
    /// Number used to identify which item the identifiers are related to
    /// </summary>

    [JsonPropertyName("speciesNumber")]
    [System.ComponentModel.Description("Number used to identify which item the identifiers are related to")]
    public int? SpeciesNumber { get; set; }

    /// <summary>
    /// List of identifiers and their keys
    /// </summary>

    [JsonPropertyName("data")]
    [System.ComponentModel.Description("List of identifiers and their keys")]
    public IDictionary<string, string>? Data { get; set; }

    /// <summary>
    /// Is the place of destination the permanent address?
    /// </summary>

    [JsonPropertyName("isPlaceOfDestinationThePermanentAddress")]
    [System.ComponentModel.Description("Is the place of destination the permanent address?")]
    public bool? IsPlaceOfDestinationThePermanentAddress { get; set; }

    /// <summary>
    /// Permanent address of the species
    /// </summary>

    [JsonPropertyName("permanentAddress")]
    [System.ComponentModel.Description("Permanent address of the species")]
    public EconomicOperator? PermanentAddress { get; set; }
}
