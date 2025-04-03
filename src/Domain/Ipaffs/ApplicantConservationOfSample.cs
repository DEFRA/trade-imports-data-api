using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ApplicantConservationOfSample
{
    Ambient,

    Chilled,

    Frozen,
}
