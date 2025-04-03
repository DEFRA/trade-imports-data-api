using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum InspectionCheckStatus
{
    ToDo,

    Compliant,

    AutoCleared,

    NonCompliant,

    NotInspected,

    ToBeInspected,

    Hold,
}
