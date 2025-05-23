using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Domain.Attributes;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

/// <summary>
/// consignment checks
/// </summary>
public class ConsignmentCheck
{
    /// <summary>
    /// Does it conform EU standards
    /// </summary>
    [JsonPropertyName("euStandard")]
    [System.ComponentModel.Description("Does it conform EU standards")]
    [PossibleValue("Satisfactory")]
    [PossibleValue("Satisfactory following official intervention")]
    [PossibleValue("Not Satisfactory")]
    [PossibleValue("Not Done")]
    [PossibleValue("Derogation")]
    [PossibleValue("Not Set")]
    public string? EuStandard { get; set; }

    /// <summary>
    /// Result of additional guarantees
    /// </summary>
    [JsonPropertyName("additionalGuarantees")]
    [System.ComponentModel.Description("Result of additional guarantees")]
    [PossibleValue("Satisfactory")]
    [PossibleValue("Satisfactory following official intervention")]
    [PossibleValue("Not Satisfactory")]
    [PossibleValue("Not Done")]
    [PossibleValue("Derogation")]
    [PossibleValue("Not Set")]
    public string? AdditionalGuarantees { get; set; }

    /// <summary>
    /// Additional details for document check
    /// </summary>
    [JsonPropertyName("documentCheckAdditionalDetails")]
    [System.ComponentModel.Description("Additional details for document check")]
    public string? DocumentCheckAdditionalDetails { get; set; }

    /// <summary>
    /// Result of document check
    /// </summary>
    [JsonPropertyName("documentCheckResult")]
    [System.ComponentModel.Description("Result of document check")]
    [PossibleValue("Satisfactory")]
    [PossibleValue("Satisfactory following official intervention")]
    [PossibleValue("Not Satisfactory")]
    [PossibleValue("Not Done")]
    [PossibleValue("Derogation")]
    [PossibleValue("Not Set")]
    public string? DocumentCheckResult { get; set; }

    /// <summary>
    /// Result of national requirements check
    /// </summary>
    [JsonPropertyName("nationalRequirements")]
    [System.ComponentModel.Description("Result of national requirements check")]
    [PossibleValue("Satisfactory")]
    [PossibleValue("Satisfactory following official intervention")]
    [PossibleValue("Not Satisfactory")]
    [PossibleValue("Not Done")]
    [PossibleValue("Derogation")]
    [PossibleValue("Not Set")]
    public string? NationalRequirements { get; set; }

    /// <summary>
    /// Was identity check done
    /// </summary>
    [JsonPropertyName("identityCheckDone")]
    [System.ComponentModel.Description("Was identity check done")]
    public bool? IdentityCheckDone { get; set; }

    /// <summary>
    /// Type of identity check performed
    /// </summary>
    [JsonPropertyName("identityCheckType")]
    [System.ComponentModel.Description("Type of identity check performed")]
    [PossibleValue("Seal Check")]
    [PossibleValue("Full Identity Check")]
    [PossibleValue("Not Done")]
    public string? IdentityCheckType { get; set; }

    /// <summary>
    /// Result of identity check
    /// </summary>
    [JsonPropertyName("identityCheckResult")]
    [System.ComponentModel.Description("Result of identity check")]
    [PossibleValue("Satisfactory")]
    [PossibleValue("Satisfactory following official intervention")]
    [PossibleValue("Not Satisfactory")]
    [PossibleValue("Not Done")]
    [PossibleValue("Derogation")]
    [PossibleValue("Not Set")]
    public string? IdentityCheckResult { get; set; }

    /// <summary>
    /// What was the reason for skipping identity check
    /// </summary>
    [JsonPropertyName("identityCheckNotDoneReason")]
    [System.ComponentModel.Description("What was the reason for skipping identity check")]
    [PossibleValue("Reduced checks regime")]
    [PossibleValue("Not required")]
    [PossibleValue("Chilled equine semen facilitation scheme")]
    public string? IdentityCheckNotDoneReason { get; set; }

    /// <summary>
    /// Was physical check done
    /// </summary>
    [JsonPropertyName("physicalCheckDone")]
    [System.ComponentModel.Description("Was physical check done")]
    public bool? PhysicalCheckDone { get; set; }

    /// <summary>
    /// Result of physical check
    /// </summary>
    [JsonPropertyName("physicalCheckResult")]
    [System.ComponentModel.Description("Result of physical check")]
    [PossibleValue("Satisfactory")]
    [PossibleValue("Satisfactory following official intervention")]
    [PossibleValue("Not Satisfactory")]
    [PossibleValue("Not Done")]
    [PossibleValue("Derogation")]
    [PossibleValue("Not Set")]
    public string? PhysicalCheckResult { get; set; }

    /// <summary>
    /// What was the reason for skipping physical check
    /// </summary>
    [JsonPropertyName("physicalCheckNotDoneReason")]
    [System.ComponentModel.Description("What was the reason for skipping physical check")]
    [PossibleValue("Reduced checks regime")]
    [PossibleValue("Other")]
    public string? PhysicalCheckNotDoneReason { get; set; }

    /// <summary>
    /// Other reason to not do physical check
    /// </summary>
    [JsonPropertyName("physicalCheckOtherText")]
    [System.ComponentModel.Description("Other reason to not do physical check")]
    public string? PhysicalCheckOtherText { get; set; }

    /// <summary>
    /// Welfare check
    /// </summary>
    [JsonPropertyName("welfareCheck")]
    [System.ComponentModel.Description("Welfare check")]
    [PossibleValue("Satisfactory")]
    [PossibleValue("Satisfactory following official intervention")]
    [PossibleValue("Not Satisfactory")]
    [PossibleValue("Not Done")]
    [PossibleValue("Derogation")]
    [PossibleValue("Not Set")]
    public string? WelfareCheck { get; set; }

    /// <summary>
    /// Number of animals checked
    /// </summary>
    [JsonPropertyName("numberOfAnimalsChecked")]
    [System.ComponentModel.Description("Number of animals checked")]
    public int? NumberOfAnimalsChecked { get; set; }

    /// <summary>
    /// Were laboratory tests done
    /// </summary>
    [JsonPropertyName("laboratoryCheckDone")]
    [System.ComponentModel.Description("Were laboratory tests done")]
    public bool? LaboratoryCheckDone { get; set; }

    /// <summary>
    /// Result of laboratory tests
    /// </summary>
    [JsonPropertyName("laboratoryCheckResult")]
    [System.ComponentModel.Description("Result of laboratory tests")]
    [PossibleValue("Satisfactory")]
    [PossibleValue("Satisfactory following official intervention")]
    [PossibleValue("Not Satisfactory")]
    [PossibleValue("Not Done")]
    [PossibleValue("Derogation")]
    [PossibleValue("Not Set")]
    public string? LaboratoryCheckResult { get; set; }
}
