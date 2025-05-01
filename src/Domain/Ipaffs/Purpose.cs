using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Domain.Attributes;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

/// <summary>
/// Purpose of consignment details
/// </summary>
public class Purpose
{
    /// <summary>
    /// Does consignment conforms to UK laws
    /// </summary>
    [JsonPropertyName("conformsToEU")]
    [System.ComponentModel.Description("Does consignment conforms to UK laws")]
    public bool? ConformsToEU { get; set; }

    /// <summary>
    /// Detailed purpose of internal market purpose group
    /// </summary>
    [JsonPropertyName("internalMarketPurpose")]
    [System.ComponentModel.Description("Detailed purpose of internal market purpose group")]
    [PossibleValue("Animal Feeding Stuff")]
    [PossibleValue("Human Consumption")]
    [PossibleValue("Pharmaceutical Use")]
    [PossibleValue("Technical Use")]
    [PossibleValue("Other")]
    [PossibleValue("Commercial Sale")]
    [PossibleValue("Commercial sale or change of ownership")]
    [PossibleValue("Rescue")]
    [PossibleValue("Breeding")]
    [PossibleValue("Research")]
    [PossibleValue("Racing or Competition")]
    [PossibleValue("Approved Premises or Body")]
    [PossibleValue("Companion Animal not for Resale or Rehoming")]
    [PossibleValue("Production")]
    [PossibleValue("Slaughter")]
    [PossibleValue("Fattening")]
    [PossibleValue("Game Restocking")]
    [PossibleValue("Registered Horses")]
    public string? InternalMarketPurpose { get; set; }

    /// <summary>
    /// Country that consignment is transshipped through
    /// </summary>
    [JsonPropertyName("thirdCountryTranshipment")]
    [System.ComponentModel.Description("Country that consignment is transshipped through")]
    public string? ThirdCountryTranshipment { get; set; }

    /// <summary>
    /// Detailed purpose for non conforming purpose group
    /// </summary>
    [JsonPropertyName("forNonConforming")]
    [System.ComponentModel.Description("Detailed purpose for non conforming purpose group")]
    [PossibleValue("Customs Warehouse")]
    [PossibleValue("Free Zone or Free Warehouse")]
    [PossibleValue("Ship Supplier")]
    [PossibleValue("Ship")]
    public string? ForNonConforming { get; set; }

    /// <summary>
    /// There are 3 types of registration number based on the purpose of consignment. Customs registration number, Free zone registration number and Shipping supplier registration number.
    /// </summary>
    [JsonPropertyName("regNumber")]
    [System.ComponentModel.Description(
        "There are 3 types of registration number based on the purpose of consignment. Customs registration number, Free zone registration number and Shipping supplier registration number. "
    )]
    public string? RegNumber { get; set; }

    /// <summary>
    /// Ship name
    /// </summary>
    [JsonPropertyName("shipName")]
    [System.ComponentModel.Description("Ship name")]
    public string? ShipName { get; set; }

    /// <summary>
    /// Destination Ship port
    /// </summary>
    [JsonPropertyName("shipPort")]
    [System.ComponentModel.Description("Destination Ship port")]
    public string? ShipPort { get; set; }

    /// <summary>
    /// Exit Border Inspection Post
    /// </summary>
    [JsonPropertyName("exitBip")]
    [System.ComponentModel.Description("Exit Border Inspection Post")]
    public string? ExitBip { get; set; }

    /// <summary>
    /// Country to which consignment is transited
    /// </summary>
    [JsonPropertyName("thirdCountry")]
    [System.ComponentModel.Description("Country to which consignment is transited")]
    public string? ThirdCountry { get; set; }

    /// <summary>
    /// Countries that consignment is transited through
    /// </summary>
    [JsonPropertyName("transitThirdCountries")]
    [System.ComponentModel.Description("Countries that consignment is transited through")]
    public string[]? TransitThirdCountries { get; set; }

    /// <summary>
    /// Specification of Import or admission purpose
    /// </summary>
    [JsonPropertyName("forImportOrAdmission")]
    [System.ComponentModel.Description("Specification of Import or admission purpose")]
    [PossibleValue("Definitive import")]
    [PossibleValue("Horses Re-entry")]
    [PossibleValue("Temporary admission horses")]
    public string? ForImportOrAdmission { get; set; }

    /// <summary>
    /// Exit date when import or admission
    /// </summary>
    [JsonPropertyName("exitDate")]
    [System.ComponentModel.Description("Exit date when import or admission")]
    public DateOnly? ExitDate { get; set; }

    /// <summary>
    /// Final Border Inspection Post
    /// </summary>
    [JsonPropertyName("finalBip")]
    [System.ComponentModel.Description("Final Border Inspection Post")]
    public string? FinalBip { get; set; }

    /// <summary>
    /// Purpose group of consignment (general purpose)
    /// </summary>
    [JsonPropertyName("purposeGroup")]
    [System.ComponentModel.Description("Purpose group of consignment (general purpose)")]
    [PossibleValue("For Import")]
    [PossibleValue("For NON-Conforming Consignments")]
    [PossibleValue("For Transhipment to")]
    [PossibleValue("For Transit to 3rd Country")]
    [PossibleValue("For Re-Import")]
    [PossibleValue("For Private Import")]
    [PossibleValue("For Transfer To")]
    [PossibleValue("For Import Re-Conformity Check")]
    public string? PurposeGroup { get; set; }

    /// <summary>
    /// DateTime
    /// </summary>
    [JsonPropertyName("estimatedArrivesAtPortOfExit")]
    [System.ComponentModel.Description("DateTime")]
    public DateTime? EstimatedArrivesAtPortOfExit { get; set; }
}
