using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DecisionNotAcceptableAction
{
    Slaughter,

    Reexport,

    Euthanasia,

    Redispatching,

    Destruction,

    Transformation,

    Other,

    EntryRefusal,

    QuarantineImposed,

    SpecialTreatment,

    IndustrialProcessing,

    ReDispatch,

    UseForOtherPurposes,
}
