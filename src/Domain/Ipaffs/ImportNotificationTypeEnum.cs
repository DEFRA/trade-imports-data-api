using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ImportNotificationType
{
    Cveda,

    Cvedp,

    Chedpp,

    Ced,

    Imp,
}
