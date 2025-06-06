using System.Text.Json.Serialization;
using Defra.TradeImportsDataApi.Domain.Attributes;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

public class ImportPreNotification
{
    /// <summary>
    /// The IPAFFS ID number for this notification.
    /// </summary>
    [JsonPropertyName("ipaffsId")]
    [System.ComponentModel.Description("The IPAFFS ID number for this notification.")]
    public int? IpaffsId { get; set; }

    /// <summary>
    /// The etag for this notification.
    /// </summary>
    [JsonPropertyName("etag")]
    [System.ComponentModel.Description("The etag for this notification.")]
    public string? Etag { get; set; }

    /// <summary>
    /// List of external references, which relate to downstream services
    /// </summary>
    [JsonPropertyName("externalReferences")]
    [System.ComponentModel.Description("List of external references, which relate to downstream services")]
    public ExternalReference[]? ExternalReferences { get; set; }

    /// <summary>
    /// Reference number of the notification
    /// </summary>
    [JsonPropertyName("referenceNumber")]
    [System.ComponentModel.Description("Reference number of the notification")]
    public string? ReferenceNumber { get; set; }

    /// <summary>
    /// Current version of the notification
    /// </summary>
    [JsonPropertyName("version")]
    [System.ComponentModel.Description("Current version of the notification")]
    public int? Version { get; set; }

    /// <summary>
    /// Date when the notification was last updated.
    /// </summary>
    [JsonPropertyName("updatedSource")]
    [System.ComponentModel.Description("Date when the notification was last updated.")]
    public DateTime? UpdatedSource { get; set; }

    /// <summary>
    /// User entity whose update was last
    /// </summary>
    [JsonPropertyName("lastUpdatedBy")]
    [System.ComponentModel.Description("User entity whose update was last")]
    public UserInformation? LastUpdatedBy { get; set; }

    /// <summary>
    /// The Type of notification that has been submitted
    /// </summary>
    [JsonPropertyName("importNotificationType")]
    [System.ComponentModel.Description("The Type of notification that has been submitted")]
    [PossibleValue("CVEDA")]
    [PossibleValue("CVEDP")]
    [PossibleValue("CHEDPP")]
    [PossibleValue("CED")]
    [PossibleValue("IMP")]
    public string? ImportNotificationType { get; set; }

    /// <summary>
    /// Reference number of notification that was replaced by this one
    /// </summary>
    [JsonPropertyName("replaces")]
    [System.ComponentModel.Description("Reference number of notification that was replaced by this one another test")]
    public string? Replaces { get; set; }

    /// <summary>
    /// Reference number of notification that replaced this one
    /// </summary>
    [JsonPropertyName("replacedBy")]
    [System.ComponentModel.Description("Reference number of notification that replaced this one")]
    public string? ReplacedBy { get; set; }

    /// <summary>
    /// Current status of the notification. When created by an importer, the notification has the status 'SUBMITTED'. Before submission of the notification it has the status 'DRAFT'. When the BIP starts validation of the notification it has the status 'IN PROGRESS' Once the BIP validates the notification, it gets the status 'VALIDATED'. 'AMEND' is set when the Part-1 user is modifying the notification. 'MODIFY' is set when Part-2 user is modifying the notification. Replaced - When the notification is replaced by another notification. Rejected - Notification moves to Rejected status when rejected by a Part-2 user.
    /// </summary>
    [JsonPropertyName("status")]
    [System.ComponentModel.Description(
        "Current status of the notification. When created by an importer, the notification has the status 'SUBMITTED'. Before submission of the notification it has the status 'DRAFT'. When the BIP starts validation of the notification it has the status 'IN PROGRESS' Once the BIP validates the notification, it gets the status 'VALIDATED'. 'AMEND' is set when the Part-1 user is modifying the notification. 'MODIFY' is set when Part-2 user is modifying the notification. Replaced - When the notification is replaced by another notification. Rejected - Notification moves to Rejected status when rejected by a Part-2 user."
    )]
    [PossibleValue("DRAFT")]
    [PossibleValue("SUBMITTED")]
    [PossibleValue("VALIDATED")]
    [PossibleValue("REJECTED")]
    [PossibleValue("IN_PROGRESS")]
    [PossibleValue("AMEND")]
    [PossibleValue("MODIFY")]
    [PossibleValue("REPLACED")]
    [PossibleValue("CANCELLED")]
    [PossibleValue("DELETED")]
    [PossibleValue("PARTIALLY_REJECTED")]
    [PossibleValue("SPLIT_CONSIGNMENT")]
    [PossibleValue("SUBMITTED,IN_PROGRESS,MODIFY")]
    public string? Status { get; set; }

    /// <summary>
    /// Present if the consignment has been split
    /// </summary>
    [JsonPropertyName("splitConsignment")]
    [System.ComponentModel.Description("Present if the consignment has been split")]
    public SplitConsignment? SplitConsignment { get; set; }

    /// <summary>
    /// Is this notification a child of a split consignment?
    /// </summary>
    [JsonPropertyName("childNotification")]
    [System.ComponentModel.Description("Is this notification a child of a split consignment?")]
    public bool? ChildNotification { get; set; }

    /// <summary>
    /// Result of risk assessment by the risk scorer
    /// </summary>
    [JsonPropertyName("riskAssessment")]
    public RiskAssessmentResult? RiskAssessment { get; init; }

    /// <summary>
    /// Details of the risk categorisation level for a notification
    /// </summary>
    [JsonPropertyName("journeyRiskCategorisation")]
    [System.ComponentModel.Description("Details of the risk categorisation level for a notification")]
    public JourneyRiskCategorisationResult? JourneyRiskCategorisation { get; set; }

    /// <summary>
    /// Is this notification a high risk notification from the EU/EEA?
    /// </summary>
    [JsonPropertyName("isHighRiskEuImport")]
    [System.ComponentModel.Description("Is this notification a high risk notification from the EU/EEA?")]
    public bool? IsHighRiskEuImport { get; set; }

    [JsonPropertyName("partOne")]
    public PartOne? PartOne { get; set; }

    /// <summary>
    /// Information about the user who set the decision in Part 2
    /// </summary>
    [JsonPropertyName("decisionBy")]
    [System.ComponentModel.Description("Information about the user who set the decision in Part 2")]
    public UserInformation? DecisionBy { get; set; }

    /// <summary>
    /// Date when the notification was validated or rejected
    /// </summary>
    [JsonPropertyName("decisionDate")]
    [System.ComponentModel.Description("Date when the notification was validated or rejected")]
    public string? DecisionDate { get; set; }

    /// <summary>
    /// Part of the notification which contains information filled by inspector at BIP/DPE
    /// </summary>
    [JsonPropertyName("partTwo")]
    [System.ComponentModel.Description(
        "Part of the notification which contains information filled by inspector at BIP/DPE"
    )]
    public PartTwo? PartTwo { get; set; }

    /// <summary>
    /// Part of the notification which contains information filled by LVU if control of consignment is needed.
    /// </summary>
    [JsonPropertyName("partThree")]
    [System.ComponentModel.Description(
        "Part of the notification which contains information filled by LVU if control of consignment is needed."
    )]
    public PartThree? PartThree { get; set; }

    /// <summary>
    /// Official veterinarian
    /// </summary>
    [JsonPropertyName("officialVeterinarian")]
    [System.ComponentModel.Description("Official veterinarian")]
    public string? OfficialVeterinarian { get; set; }

    /// <summary>
    /// Validation messages for whole notification
    /// </summary>
    [JsonPropertyName("consignmentValidations")]
    [System.ComponentModel.Description("Validation messages for whole notification")]
    public ValidationMessageCode[]? ConsignmentValidations { get; set; }

    /// <summary>
    /// Organisation id which the agent user belongs to, stored against each notification which has been raised on behalf of another organisation
    /// </summary>
    [JsonPropertyName("agencyOrganisationId")]
    [System.ComponentModel.Description(
        "Organisation id which the agent user belongs to, stored against each notification which has been raised on behalf of another organisation"
    )]
    public string? AgencyOrganisationId { get; set; }

    /// <summary>
    /// Date and Time when risk decision was locked
    /// </summary>
    [JsonPropertyName("riskDecisionLockedOn")]
    [System.ComponentModel.Description("Date and Time when risk decision was locked")]
    public DateTime? RiskDecisionLockedOn { get; set; }

    /// <summary>
    /// is the risk decision locked?
    /// </summary>
    [JsonPropertyName("isRiskDecisionLocked")]
    [System.ComponentModel.Description("is the risk decision locked?")]
    public bool? IsRiskDecisionLocked { get; set; }

    /// <summary>
    /// Boolean flag that indicates whether CHED is exempted from autoclearance
    /// </summary>
    [JsonPropertyName("isAutoClearanceExempted")]
    [System.ComponentModel.Description("Boolean flag that indicates whether CHED is exempted from autoclearance")]
    public bool? IsAutoClearanceExempted { get; set; }

    /// <summary>
    /// Boolean flag that indicates whether a bulk upload is in progress
    /// </summary>
    [JsonPropertyName("isBulkUploadInProgress")]
    [System.ComponentModel.Description("Boolean flag that indicates whether a bulk upload is in progress")]
    public bool? IsBulkUploadInProgress { get; set; }

    /// <summary>
    /// Request UUID to trace bulk upload
    /// </summary>
    [JsonPropertyName("requestId")]
    [System.ComponentModel.Description("Request UUID to trace bulk upload")]
    public string? RequestId { get; set; }

    /// <summary>
    /// Have all commodities been matched with corresponding CDS declaration(s)
    /// </summary>
    [JsonPropertyName("isCdsFullMatched")]
    [System.ComponentModel.Description("Have all commodities been matched with corresponding CDS declaration(s)")]
    public bool? IsCdsFullMatched { get; set; }

    /// <summary>
    /// The version of the ched type the notification was created with
    /// </summary>
    [JsonPropertyName("chedTypeVersion")]
    [System.ComponentModel.Description("The version of the ched type the notification was created with")]
    public int? ChedTypeVersion { get; set; }

    /// <summary>
    /// Indicates whether a CHED has been matched with a GVMS GMR via DMP
    /// </summary>
    [JsonPropertyName("isGMRMatched")]
    [System.ComponentModel.Description("Indicates whether a CHED has been matched with a GVMS GMR via DMP")]
    public bool? IsGMRMatched { get; set; }
}
