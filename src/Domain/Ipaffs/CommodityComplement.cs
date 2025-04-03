using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

/// <summary>
/// Holder for additional parameters of a commodity
/// </summary>
public class CommodityComplement
{
    /// <summary>
    /// UUID used to match commodityComplement to its complementParameter set. CHEDPP only
    /// </summary>

    [JsonPropertyName("uniqueComplementId")]
    [System.ComponentModel.Description(
        "UUID used to match commodityComplement to its complementParameter set. CHEDPP only"
    )]
    public string? UniqueComplementId { get; set; }

    /// <summary>
    /// Description of the commodity
    /// </summary>

    [JsonPropertyName("commodityDescription")]
    [System.ComponentModel.Description("Description of the commodity")]
    public string? CommodityDescription { get; set; }

    /// <summary>
    /// The unique commodity ID
    /// </summary>

    [JsonPropertyName("commodityId")]
    [System.ComponentModel.Description("The unique commodity ID")]
    public string? CommodityId { get; set; }

    /// <summary>
    /// Identifier of complement chosen from speciesFamily,speciesClass and speciesType&#x27;. This is also used to link to the complementParameterSet
    /// </summary>

    [JsonPropertyName("complementId")]
    [System.ComponentModel.Description(
        "Identifier of complement chosen from speciesFamily,speciesClass and speciesType'. This is also used to link to the complementParameterSet"
    )]
    public int? ComplementId { get; set; }

    /// <summary>
    /// Represents the lowest granularity - either type, class, family or species name - for the commodity selected.  This is also used to drive behaviour for EU Import journeys
    /// </summary>

    [JsonPropertyName("complementName")]
    [System.ComponentModel.Description(
        "Represents the lowest granularity - either type, class, family or species name - for the commodity selected.  This is also used to drive behaviour for EU Import journeys"
    )]
    public string? ComplementName { get; set; }

    /// <summary>
    /// EPPO Code related to plant commodities and wood packaging
    /// </summary>

    [JsonPropertyName("eppoCode")]
    [System.ComponentModel.Description("EPPO Code related to plant commodities and wood packaging")]
    public string? EppoCode { get; set; }

    /// <summary>
    /// (Deprecated in IMTA-11832) Is this commodity wood packaging?
    /// </summary>

    [JsonPropertyName("isWoodPackaging")]
    [System.ComponentModel.Description("(Deprecated in IMTA-11832) Is this commodity wood packaging?")]
    public bool? IsWoodPackaging { get; set; }

    /// <summary>
    /// The species ID of the commodity that is imported. Not every commodity has a species ID. This is also used to link to the complementParameterSet. The species ID can change over time
    /// </summary>

    [JsonPropertyName("speciesId")]
    [System.ComponentModel.Description(
        "The species ID of the commodity that is imported. Not every commodity has a species ID. This is also used to link to the complementParameterSet. The species ID can change over time"
    )]
    public string? SpeciesId { get; set; }

    /// <summary>
    /// Species name
    /// </summary>

    [JsonPropertyName("speciesName")]
    [System.ComponentModel.Description("Species name")]
    public string? SpeciesName { get; set; }

    /// <summary>
    /// Species nomination
    /// </summary>

    [JsonPropertyName("speciesNomination")]
    [System.ComponentModel.Description("Species nomination")]
    public string? SpeciesNomination { get; set; }

    /// <summary>
    /// Species type name
    /// </summary>

    [JsonPropertyName("speciesTypeName")]
    [System.ComponentModel.Description("Species type name")]
    public string? SpeciesTypeName { get; set; }

    /// <summary>
    /// Species type identifier of commodity
    /// </summary>

    [JsonPropertyName("speciesType")]
    [System.ComponentModel.Description("Species type identifier of commodity")]
    public string? SpeciesType { get; set; }

    /// <summary>
    /// Species class name
    /// </summary>

    [JsonPropertyName("speciesClassName")]
    [System.ComponentModel.Description("Species class name")]
    public string? SpeciesClassName { get; set; }

    /// <summary>
    /// Species class identifier of commodity
    /// </summary>

    [JsonPropertyName("speciesClass")]
    [System.ComponentModel.Description("Species class identifier of commodity")]
    public string? SpeciesClass { get; set; }

    /// <summary>
    /// Species family name of commodity
    /// </summary>

    [JsonPropertyName("speciesFamilyName")]
    [System.ComponentModel.Description("Species family name of commodity")]
    public string? SpeciesFamilyName { get; set; }

    /// <summary>
    /// Species family identifier of commodity
    /// </summary>

    [JsonPropertyName("speciesFamily")]
    [System.ComponentModel.Description("Species family identifier of commodity")]
    public string? SpeciesFamily { get; set; }

    /// <summary>
    /// Species common name of commodity for IMP notification simple commodity selection
    /// </summary>

    [JsonPropertyName("speciesCommonName")]
    [System.ComponentModel.Description(
        "Species common name of commodity for IMP notification simple commodity selection"
    )]
    public string? SpeciesCommonName { get; set; }

    /// <summary>
    /// Has commodity been matched with corresponding CDS declaration
    /// </summary>

    [JsonPropertyName("isCdsMatched")]
    [System.ComponentModel.Description("Has commodity been matched with corresponding CDS declaration")]
    public bool? IsCdsMatched { get; set; }

    [JsonPropertyName("additionalData")]
    public IDictionary<string, object>? AdditionalData { get; set; }

    [JsonPropertyName("riskAssesment")]
    public CommodityRiskResult? RiskAssesment { get; set; }

    [JsonPropertyName("checks")]
    public InspectionCheck[]? Checks { get; set; }
}
