using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ControlConsignmentLeave
{
    Yes,

    No,

    ItHasBeenDestroyed,
}
