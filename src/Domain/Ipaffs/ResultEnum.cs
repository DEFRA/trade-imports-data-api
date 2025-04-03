using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Result
{
    Satisfactory,

    SatisfactoryFollowingOfficialIntervention,

    NotSatisfactory,

    NotDone,

    Derogation,

    NotSet,
}
