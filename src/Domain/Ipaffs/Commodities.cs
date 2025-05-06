using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Domain.Attributes;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

public class Commodities
{
    /// <summary>
    /// Flag to record when the GMS declaration has been accepted
    /// </summary>
    [JsonPropertyName("gmsDeclarationAccepted")]
    [System.ComponentModel.Description("Flag to record when the GMS declaration has been accepted")]
    public bool? GmsDeclarationAccepted { get; set; }

    /// <summary>
    /// Flag to record whether the consigned country is in an ipaffs charge group
    /// </summary>
    [JsonPropertyName("consignedCountryInChargeGroup")]
    [System.ComponentModel.Description("Flag to record whether the consigned country is in an ipaffs charge group")]
    public bool? ConsignedCountryInChargeGroup { get; set; }

    /// <summary>
    /// The total gross weight of the consignment.  It must be bigger than the total net weight of the commodities
    /// </summary>
    [JsonPropertyName("totalGrossWeight")]
    [System.ComponentModel.Description(
        "The total gross weight of the consignment.  It must be bigger than the total net weight of the commodities"
    )]
    public double? TotalGrossWeight { get; set; }

    /// <summary>
    /// The total net weight of the commodities within this consignment
    /// </summary>
    [JsonPropertyName("totalNetWeight")]
    [System.ComponentModel.Description("The total net weight of the commodities within this consignment")]
    public double? TotalNetWeight { get; set; }

    /// <summary>
    /// The total gross volume of the commodities within this consignment
    /// </summary>
    [JsonPropertyName("totalGrossVolume")]
    [System.ComponentModel.Description("The total gross volume of the commodities within this consignment")]
    public double? TotalGrossVolume { get; set; }

    /// <summary>
    /// Unit used for specifying total gross volume of this consignment (litres or metres cubed)
    /// </summary>
    [JsonPropertyName("totalGrossVolumeUnit")]
    [System.ComponentModel.Description(
        "Unit used for specifying total gross volume of this consignment (litres or metres cubed)"
    )]
    public string? TotalGrossVolumeUnit { get; set; }

    /// <summary>
    /// The total number of packages within this consignment
    /// </summary>
    [JsonPropertyName("numberOfPackages")]
    [System.ComponentModel.Description("The total number of packages within this consignment")]
    public int? NumberOfPackages { get; set; }

    /// <summary>
    /// Temperature (type) of commodity
    /// </summary>
    [JsonPropertyName("temperature")]
    [System.ComponentModel.Description("Temperature (type) of commodity")]
    public string? Temperature { get; set; }

    /// <summary>
    /// The total number of animals within this consignment
    /// </summary>
    [JsonPropertyName("numberOfAnimals")]
    [System.ComponentModel.Description("The total number of animals within this consignment")]
    public int? NumberOfAnimals { get; set; }

    /// <summary>
    /// Does consignment contain ablacted animals
    /// </summary>
    [JsonPropertyName("includeNonAblactedAnimals")]
    [System.ComponentModel.Description("Does consignment contain ablacted animals")]
    public bool? IncludeNonAblactedAnimals { get; set; }

    /// <summary>
    /// Consignments country of origin
    /// </summary>
    [JsonPropertyName("countryOfOrigin")]
    [System.ComponentModel.Description("Consignments country of origin")]
    public string? CountryOfOrigin { get; set; }

    /// <summary>
    /// Flag to record whether country of origin is a temporary PoD country
    /// </summary>
    [JsonPropertyName("countryOfOriginIsPodCountry")]
    [System.ComponentModel.Description("Flag to record whether country of origin is a temporary PoD country")]
    public bool? CountryOfOriginIsPodCountry { get; set; }

    /// <summary>
    /// Flag to record whether country of origin is a low risk article 72 country
    /// </summary>
    [JsonPropertyName("isLowRiskArticle72Country")]
    [System.ComponentModel.Description("Flag to record whether country of origin is a low risk article 72 country")]
    public bool? IsLowRiskArticle72Country { get; set; }

    /// <summary>
    /// Region of country
    /// </summary>
    [JsonPropertyName("regionOfOrigin")]
    [System.ComponentModel.Description("Region of country")]
    public string? RegionOfOrigin { get; set; }

    /// <summary>
    /// Country from where commodity was sent
    /// </summary>
    [JsonPropertyName("consignedCountry")]
    [System.ComponentModel.Description("Country from where commodity was sent")]
    public string? ConsignedCountry { get; set; }

    /// <summary>
    /// Certification of animals (Breeding, slaughter etc.)
    /// </summary>
    [JsonPropertyName("animalsCertifiedAs")]
    [System.ComponentModel.Description("Certification of animals (Breeding, slaughter etc.)")]
    public string? AnimalsCertifiedAs { get; set; }

    /// <summary>
    /// What the commodity is intended for
    /// </summary>
    [JsonPropertyName("commodityIntendedFor")]
    [System.ComponentModel.Description("What the commodity is intended for")]
    [PossibleValue("human")]
    [PossibleValue("feedingstuff")]
    [PossibleValue("further")]
    [PossibleValue("other")]
    public string? CommodityIntendedFor { get; set; }
}
