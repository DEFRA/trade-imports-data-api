using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

/// <summary>
/// Decision if the consignment can be imported
/// </summary>
public class Decision
{
    /// <summary>
    /// Is consignment acceptable or not
    /// </summary>
    [JsonPropertyName("consignmentAcceptable")]
    [System.ComponentModel.Description("Is consignment acceptable or not")]
    public bool? ConsignmentAcceptable { get; set; }

    /// <summary>
    /// Filled if consignmentAcceptable is set to false
    /// </summary>
    [JsonPropertyName("notAcceptableAction")]
    [System.ComponentModel.Description("Filled if consignmentAcceptable is set to false")]
    public string? NotAcceptableAction { get; set; }

    /// <summary>
    /// Filled if not acceptable action is set to destruction
    /// </summary>
    [JsonPropertyName("notAcceptableActionDestructionReason")]
    [System.ComponentModel.Description("Filled if not acceptable action is set to destruction")]
    public string? NotAcceptableActionDestructionReason { get; set; }

    /// <summary>
    /// Filled if not acceptable action is set to entry refusal
    /// </summary>
    [JsonPropertyName("notAcceptableActionEntryRefusalReason")]
    [System.ComponentModel.Description("Filled if not acceptable action is set to entry refusal")]
    public string? NotAcceptableActionEntryRefusalReason { get; set; }

    /// <summary>
    /// Filled if not acceptable action is set to quarantine imposed
    /// </summary>
    [JsonPropertyName("notAcceptableActionQuarantineImposedReason")]
    [System.ComponentModel.Description("Filled if not acceptable action is set to quarantine imposed")]
    public string? NotAcceptableActionQuarantineImposedReason { get; set; }

    /// <summary>
    /// Filled if not acceptable action is set to special treatment
    /// </summary>
    [JsonPropertyName("notAcceptableActionSpecialTreatmentReason")]
    [System.ComponentModel.Description("Filled if not acceptable action is set to special treatment")]
    public string? NotAcceptableActionSpecialTreatmentReason { get; set; }

    /// <summary>
    /// Filled if not acceptable action is set to industrial processing
    /// </summary>
    [JsonPropertyName("notAcceptableActionIndustrialProcessingReason")]
    [System.ComponentModel.Description("Filled if not acceptable action is set to industrial processing")]
    public string? NotAcceptableActionIndustrialProcessingReason { get; set; }

    /// <summary>
    /// Filled if not acceptable action is set to re-dispatch
    /// </summary>
    [JsonPropertyName("notAcceptableActionReDispatchReason")]
    [System.ComponentModel.Description("Filled if not acceptable action is set to re-dispatch")]
    public string? NotAcceptableActionReDispatchReason { get; set; }

    /// <summary>
    /// Filled if not acceptable action is set to use for other purposes
    /// </summary>
    [JsonPropertyName("notAcceptableActionUseForOtherPurposesReason")]
    [System.ComponentModel.Description("Filled if not acceptable action is set to use for other purposes")]
    public string? NotAcceptableActionUseForOtherPurposesReason { get; set; }

    /// <summary>
    /// Filled when notAcceptableAction is equal to destruction
    /// </summary>
    [JsonPropertyName("notAcceptableDestructionReason")]
    [System.ComponentModel.Description("Filled when notAcceptableAction is equal to destruction")]
    public string? NotAcceptableDestructionReason { get; set; }

    /// <summary>
    /// Filled when notAcceptableAction is equal to other
    /// </summary>
    [JsonPropertyName("notAcceptableActionOtherReason")]
    [System.ComponentModel.Description("Filled when notAcceptableAction is equal to other")]
    public string? NotAcceptableActionOtherReason { get; set; }

    /// <summary>
    /// Filled when consignmentAcceptable is set to false
    /// </summary>
    [JsonPropertyName("notAcceptableActionByDate")]
    [System.ComponentModel.Description("Filled when consignmentAcceptable is set to false")]
    public DateOnly? NotAcceptableActionByDate { get; set; }

    /// <summary>
    /// List of details for individual chedpp not acceptable reasons
    /// </summary>
    [JsonPropertyName("chedppNotAcceptableReasons")]
    [System.ComponentModel.Description("List of details for individual chedpp not acceptable reasons")]
    public ChedppNotAcceptableReason[]? ChedppNotAcceptableReasons { get; set; }

    /// <summary>
    /// If the consignment was not accepted what was the reason
    /// </summary>
    [JsonPropertyName("notAcceptableReasons")]
    [System.ComponentModel.Description("If the consignment was not accepted what was the reason")]
    public string[]? NotAcceptableReasons { get; set; }

    /// <summary>
    /// 2 digits ISO code of country (not acceptable country can be empty)
    /// </summary>
    [JsonPropertyName("notAcceptableCountry")]
    [System.ComponentModel.Description("2 digits ISO code of country (not acceptable country can be empty)")]
    public string? NotAcceptableCountry { get; set; }

    /// <summary>
    /// Filled if consignmentAcceptable is set to false
    /// </summary>
    [JsonPropertyName("notAcceptableEstablishment")]
    [System.ComponentModel.Description("Filled if consignmentAcceptable is set to false")]
    public string? NotAcceptableEstablishment { get; set; }

    /// <summary>
    /// Filled if consignmentAcceptable is set to false
    /// </summary>
    [JsonPropertyName("notAcceptableOtherReason")]
    [System.ComponentModel.Description("Filled if consignmentAcceptable is set to false")]
    public string? NotAcceptableOtherReason { get; set; }

    /// <summary>
    /// Details of controlled destinations
    /// </summary>
    [JsonPropertyName("detailsOfControlledDestinations")]
    [System.ComponentModel.Description("Details of controlled destinations")]
    public Party? DetailsOfControlledDestinations { get; set; }

    /// <summary>
    /// Filled if consignment is set to acceptable and decision type is Specific Warehouse
    /// </summary>
    [JsonPropertyName("specificWarehouseNonConformingConsignment")]
    [System.ComponentModel.Description(
        "Filled if consignment is set to acceptable and decision type is Specific Warehouse"
    )]
    public string? SpecificWarehouseNonConformingConsignment { get; set; }

    /// <summary>
    /// Deadline when consignment has to leave borders
    /// </summary>
    [JsonPropertyName("temporaryDeadline")]
    [System.ComponentModel.Description("Deadline when consignment has to leave borders")]
    public string? TemporaryDeadline { get; set; }

    /// <summary>
    /// Detailed decision for consignment
    /// </summary>
    [JsonPropertyName("decision")]
    [System.ComponentModel.Description("Detailed decision for consignment")]
    public string? ConsignmentDecision { get; set; }

    /// <summary>
    /// Decision over purpose of free circulation in country
    /// </summary>
    [JsonPropertyName("freeCirculationPurpose")]
    [System.ComponentModel.Description("Decision over purpose of free circulation in country")]
    public string? FreeCirculationPurpose { get; set; }

    /// <summary>
    /// Decision over purpose of definitive import
    /// </summary>
    [JsonPropertyName("definitiveImportPurpose")]
    [System.ComponentModel.Description("Decision over purpose of definitive import")]
    public string? DefinitiveImportPurpose { get; set; }

    /// <summary>
    /// Decision channeled option based on (article8, article15)
    /// </summary>
    [JsonPropertyName("ifChanneledOption")]
    [System.ComponentModel.Description("Decision channeled option based on (article8, article15)")]
    public string? IfChanneledOption { get; set; }

    /// <summary>
    /// Custom warehouse registered number
    /// </summary>
    [JsonPropertyName("customWarehouseRegisteredNumber")]
    [System.ComponentModel.Description("Custom warehouse registered number")]
    public string? CustomWarehouseRegisteredNumber { get; set; }

    /// <summary>
    /// Free warehouse registered number
    /// </summary>
    [JsonPropertyName("freeWarehouseRegisteredNumber")]
    [System.ComponentModel.Description("Free warehouse registered number")]
    public string? FreeWarehouseRegisteredNumber { get; set; }

    /// <summary>
    /// Ship name
    /// </summary>
    [JsonPropertyName("shipName")]
    [System.ComponentModel.Description("Ship name")]
    public string? ShipName { get; set; }

    /// <summary>
    /// Port of exit
    /// </summary>
    [JsonPropertyName("shipPortOfExit")]
    [System.ComponentModel.Description("Port of exit")]
    public string? ShipPortOfExit { get; set; }

    /// <summary>
    /// Ship supplier registered number
    /// </summary>
    [JsonPropertyName("shipSupplierRegisteredNumber")]
    [System.ComponentModel.Description("Ship supplier registered number")]
    public string? ShipSupplierRegisteredNumber { get; set; }

    /// <summary>
    /// Transhipment BIP
    /// </summary>
    [JsonPropertyName("transhipmentBip")]
    [System.ComponentModel.Description("Transhipment BIP")]
    public string? TranshipmentBip { get; set; }

    /// <summary>
    /// Transhipment third country
    /// </summary>
    [JsonPropertyName("transhipmentThirdCountry")]
    [System.ComponentModel.Description("Transhipment third country")]
    public string? TranshipmentThirdCountry { get; set; }

    /// <summary>
    /// Transit exit BIP
    /// </summary>
    [JsonPropertyName("transitExitBip")]
    [System.ComponentModel.Description("Transit exit BIP")]
    public string? TransitExitBip { get; set; }

    /// <summary>
    /// Transit third country
    /// </summary>
    [JsonPropertyName("transitThirdCountry")]
    [System.ComponentModel.Description("Transit third country")]
    public string? TransitThirdCountry { get; set; }

    /// <summary>
    /// Transit destination third country
    /// </summary>
    [JsonPropertyName("transitDestinationThirdCountry")]
    [System.ComponentModel.Description("Transit destination third country")]
    public string? TransitDestinationThirdCountry { get; set; }

    /// <summary>
    /// Temporary exit BIP
    /// </summary>
    [JsonPropertyName("temporaryExitBip")]
    [System.ComponentModel.Description("Temporary exit BIP")]
    public string? TemporaryExitBip { get; set; }

    /// <summary>
    /// Horse re-entry
    /// </summary>
    [JsonPropertyName("horseReentry")]
    [System.ComponentModel.Description("Horse re-entry")]
    public string? HorseReentry { get; set; }

    /// <summary>
    /// Is it transshipped to EU or third country (values EU / country name)
    /// </summary>
    [JsonPropertyName("transhipmentEuOrThirdCountry")]
    [System.ComponentModel.Description("Is it transshipped to EU or third country (values EU / country name)")]
    public string? TranshipmentEuOrThirdCountry { get; set; }
}
