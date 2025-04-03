using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Defra.TradeImportsData.Domain.IPaffs;

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
