using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ImportNotificationStatus
{
    Draft,

    Submitted,

    Validated,

    Rejected,

    InProgress,

    Amend,

    Modify,

    Replaced,

    Cancelled,

    Deleted,

    PartiallyRejected,

    SplitConsignment,
}
