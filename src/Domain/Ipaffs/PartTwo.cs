using System.Text.Json.Serialization;

namespace Defra.TradeImportsData.Domain.IPaffs;

/// <summary>
/// Part 2 of notification - Decision at inspection
/// </summary>
public class PartTwo
{
    /// <summary>
    /// Decision on the consignment
    /// </summary>

    [JsonPropertyName("decision")]
    [System.ComponentModel.Description("Decision on the consignment")]
    public Decision? Decision { get; set; }

    /// <summary>
    /// Consignment check
    /// </summary>

    [JsonPropertyName("consignmentCheck")]
    [System.ComponentModel.Description("Consignment check")]
    public ConsignmentCheck? ConsignmentCheck { get; set; }

    /// <summary>
    /// Checks of impact of transport on animals
    /// </summary>

    [JsonPropertyName("impactOfTransportOnAnimals")]
    [System.ComponentModel.Description("Checks of impact of transport on animals")]
    public ImpactOfTransportOnAnimals? ImpactOfTransportOnAnimals { get; set; }

    /// <summary>
    /// Are laboratory tests required
    /// </summary>

    [JsonPropertyName("laboratoryTestsRequired")]
    [System.ComponentModel.Description("Are laboratory tests required")]
    public bool? LaboratoryTestsRequired { get; set; }

    /// <summary>
    /// Laboratory tests information details
    /// </summary>

    [JsonPropertyName("laboratoryTests")]
    [System.ComponentModel.Description("Laboratory tests information details")]
    public LaboratoryTests? LaboratoryTests { get; set; }

    /// <summary>
    /// Are the containers resealed
    /// </summary>

    [JsonPropertyName("resealedContainersIncluded")]
    [System.ComponentModel.Description("Are the containers resealed")]
    public bool? ResealedContainersIncluded { get; set; }

    /// <summary>
    /// (Deprecated - To be removed as part of IMTA-6256) Resealed containers information details
    /// </summary>

    [JsonPropertyName("resealedContainers")]
    [System.ComponentModel.Description(
        "(Deprecated - To be removed as part of IMTA-6256) Resealed containers information details"
    )]
    public string[]? ResealedContainers { get; set; }

    /// <summary>
    /// Resealed containers information details
    /// </summary>

    [JsonPropertyName("resealedContainersMappings")]
    [System.ComponentModel.Description("Resealed containers information details")]
    public SealContainer[]? ResealedContainersMappings { get; set; }

    /// <summary>
    /// Control Authority information details
    /// </summary>

    [JsonPropertyName("controlAuthority")]
    [System.ComponentModel.Description("Control Authority information details")]
    public ControlAuthority? ControlAuthority { get; set; }

    /// <summary>
    /// Controlled destination
    /// </summary>

    [JsonPropertyName("controlledDestination")]
    [System.ComponentModel.Description("Controlled destination")]
    public EconomicOperator? ControlledDestination { get; set; }

    /// <summary>
    /// Local reference number at BIP
    /// </summary>

    [JsonPropertyName("bipLocalReferenceNumber")]
    [System.ComponentModel.Description("Local reference number at BIP")]
    public string? BipLocalReferenceNumber { get; set; }

    /// <summary>
    /// Part 2 - Sometimes other user can sign decision on behalf of another user
    /// </summary>

    [JsonPropertyName("signedOnBehalfOf")]
    [System.ComponentModel.Description("Part 2 - Sometimes other user can sign decision on behalf of another user")]
    public string? SignedOnBehalfOf { get; set; }

    /// <summary>
    /// Onward transportation
    /// </summary>

    [JsonPropertyName("onwardTransportation")]
    [System.ComponentModel.Description("Onward transportation")]
    public string? OnwardTransportation { get; set; }

    /// <summary>
    /// Validation messages for Part 2 - Decision
    /// </summary>

    [JsonPropertyName("consignmentValidations")]
    [System.ComponentModel.Description("Validation messages for Part 2 - Decision")]
    public ValidationMessageCode[]? ConsignmentValidations { get; set; }

    /// <summary>
    /// User entered date when the checks were completed
    /// </summary>

    [JsonPropertyName("checkedOn")]
    [System.ComponentModel.Description("User entered date when the checks were completed")]
    public DateTime? CheckedOn { get; set; }

    /// <summary>
    /// Accompanying documents
    /// </summary>

    [JsonPropertyName("accompanyingDocuments")]
    [System.ComponentModel.Description("Accompanying documents")]
    public AccompanyingDocument[]? AccompanyingDocuments { get; set; }

    /// <summary>
    /// Have the PHSI regulated commodities been auto cleared?
    /// </summary>

    [JsonPropertyName("phsiAutoCleared")]
    [System.ComponentModel.Description("Have the PHSI regulated commodities been auto cleared?")]
    public bool? PhsiAutoCleared { get; set; }

    /// <summary>
    /// Have the HMI regulated commodities been auto cleared?
    /// </summary>

    [JsonPropertyName("hmiAutoCleared")]
    [System.ComponentModel.Description("Have the HMI regulated commodities been auto cleared?")]
    public bool? HmiAutoCleared { get; set; }

    /// <summary>
    /// Inspection required
    /// </summary>

    [JsonPropertyName("inspectionRequired")]
    [System.ComponentModel.Description("Inspection required")]
    public InspectionRequired? InspectionRequired { get; set; }

    /// <summary>
    /// Details about the manual inspection override
    /// </summary>

    [JsonPropertyName("inspectionOverride")]
    [System.ComponentModel.Description("Details about the manual inspection override")]
    public InspectionOverride? InspectionOverride { get; set; }

    /// <summary>
    /// Date of autoclearance
    /// </summary>

    [JsonPropertyName("autoClearedOn")]
    [System.ComponentModel.Description("Date of autoclearance")]
    public DateTime? AutoClearedOn { get; set; }
}
