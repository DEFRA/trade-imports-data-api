using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Gvms;

public class Gmr
{
    /// <summary>
    /// The Goods Movement Record (GMR) ID for this GMR.  Do not include when POSTing a GMR - GVMS will assign an ID.
    /// </summary>
    [JsonPropertyName("id")]
    [System.ComponentModel.Description(
        "The Goods Movement Record (GMR) ID for this GMR.  Do not include when POSTing a GMR - GVMS will assign an ID."
    )]
    public string? Id { get; set; }

    /// <summary>
    /// The EORI of the haulier that is responsible for the vehicle making the goods movement
    /// </summary>
    [JsonPropertyName("haulierEori")]
    [System.ComponentModel.Description(
        "The EORI of the haulier that is responsible for the vehicle making the goods movement"
    )]
    public string? HaulierEori { get; set; }

    /// <summary>
    /// The state of the GMR
    /// </summary>
    [JsonPropertyName("state")]
    [System.ComponentModel.Description("The state of the GMR")]
    public State? State { get; set; }

    /// <summary>
    /// If set to true, indicates that the vehicle requires a customs inspection.  If set to false, indicates that the vehicle does not require a customs inspection.  If not set, indicates the customs inspection decision has not yet been made or is not applicable.  For outbound GMRs, this indicates that the vehicle must present at an inspection facility prior to checking-in at the port.  For Office of Transit inspections for inbound GMRs, a decision may be made to inspect subsequent to a notification that inspection is not required.  In this event a notification will be sent that changes inspectionRequired from false to true.  If this happens after leaving the port of arrival, the inspection should be carried out at the next transit office e.g. the office of destination.
    /// </summary>
    [JsonPropertyName("inspectionRequired")]
    [System.ComponentModel.Description(
        "If set to true, indicates that the vehicle requires a customs inspection.  If set to false, indicates that the vehicle does not require a customs inspection.  If not set, indicates the customs inspection decision has not yet been made or is not applicable.  For outbound GMRs, this indicates that the vehicle must present at an inspection facility prior to checking-in at the port.  For Office of Transit inspections for inbound GMRs, a decision may be made to inspect subsequent to a notification that inspection is not required.  In this event a notification will be sent that changes inspectionRequired from false to true.  If this happens after leaving the port of arrival, the inspection should be carried out at the next transit office e.g. the office of destination."
    )]
    public bool? InspectionRequired { get; set; }

    /// <summary>
    /// A list of one or more inspection types, from GVMS Reference Data, that the vehicle must have carried out, in the order specified.  This means that where there are multiple entries here, the vehicle must report for the first inspection type listed before each subsequent listed inspection.  Each entry includes a list of available locations for the inspection type.
    /// </summary>
    [JsonPropertyName("reportToLocations")]
    [System.ComponentModel.Description(
        "A list of one or more inspection types, from GVMS Reference Data, that the vehicle must have carried out, in the order specified.  This means that where there are multiple entries here, the vehicle must report for the first inspection type listed before each subsequent listed inspection.  Each entry includes a list of available locations for the inspection type."
    )]
    public ReportToLocations[]? ReportToLocations { get; set; }

    /// <summary>
    /// The date and time that this GMR was last updated.
    /// </summary>
    [JsonPropertyName("updatedSource")]
    [System.ComponentModel.Description("The date and time that this GMR was last updated.")]
    public DateTime? UpdatedSource { get; set; }

    /// <summary>
    /// The direction of the movement - into or out of the UK, or between Great Britain and Northern Ireland
    /// </summary>
    [JsonPropertyName("direction")]
    [System.ComponentModel.Description(
        "The direction of the movement - into or out of the UK, or between Great Britain and Northern Ireland"
    )]
    public Direction? Direction { get; set; }

    /// <summary>
    /// The type of haulier moving the goods
    /// </summary>
    [JsonPropertyName("haulierType")]
    [System.ComponentModel.Description("The type of haulier moving the goods")]
    public HaulierType? HaulierType { get; set; }

    /// <summary>
    /// Set to true if the vehicle will not be accompanying the trailer(s) during the crossing, or if the vehicle is carrying a container that will be detached and loaded for the crossing.
    /// </summary>
    [JsonPropertyName("isUnaccompanied")]
    [System.ComponentModel.Description(
        "Set to true if the vehicle will not be accompanying the trailer(s) during the crossing, or if the vehicle is carrying a container that will be detached and loaded for the crossing."
    )]
    public bool? IsUnaccompanied { get; set; }

    /// <summary>
    /// Vehicle registration number of the vehicle that will arrive at port.  If isUnaccompanied is set to false then vehicleRegNum must be provided to check-in.
    /// </summary>
    [JsonPropertyName("vehicleRegistrationNumber")]
    [System.ComponentModel.Description(
        "Vehicle registration number of the vehicle that will arrive at port.  If isUnaccompanied is set to false then vehicleRegNum must be provided to check-in."
    )]
    public string? VehicleRegistrationNumber { get; set; }

    /// <summary>
    /// For vehicles carrying trailers, the trailer registration number of each trailer.  If isUnaccompanied is set to true then trailerRegistrationNums or containerReferenceNums must be provided before check-in.
    /// </summary>
    [JsonPropertyName("trailerRegistrationNums")]
    [System.ComponentModel.Description(
        "For vehicles carrying trailers, the trailer registration number of each trailer.  If isUnaccompanied is set to true then trailerRegistrationNums or containerReferenceNums must be provided before check-in."
    )]
    public string[]? TrailerRegistrationNums { get; set; }

    /// <summary>
    /// For vehicles arriving with containers that will be detached and loaded, the container reference number of each container in the movement. If isUnaccompanied is set to true then trailerRegistrationNums or containerReferenceNums must be provided before check-in.
    /// </summary>
    [JsonPropertyName("containerReferenceNums")]
    [System.ComponentModel.Description(
        "For vehicles arriving with containers that will be detached and loaded, the container reference number of each container in the movement. If isUnaccompanied is set to true then trailerRegistrationNums or containerReferenceNums must be provided before check-in."
    )]
    public string[]? ContainerReferenceNums { get; set; }

    [JsonPropertyName("plannedCrossing")]
    public PlannedCrossing? PlannedCrossing { get; set; }

    [JsonPropertyName("checkedInCrossing")]
    public CheckedInCrossing? CheckedInCrossing { get; set; }

    [JsonPropertyName("actualCrossing")]
    public ActualCrossing? ActualCrossing { get; set; }

    [JsonPropertyName("declarations")]
    public Declarations? Declarations { get; set; }
}
