using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Gvms;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Direction
{
    UkInbound,

    UkOutbound,

    GbToNi,

    NiToGb,
}
