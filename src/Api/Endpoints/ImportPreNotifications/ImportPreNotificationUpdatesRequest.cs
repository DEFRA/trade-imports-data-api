using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

namespace Defra.TradeImportsDataApi.Api.Endpoints.ImportPreNotifications;

public class ImportPreNotificationUpdatesRequest
{
    [Description(
        "Filter import pre notifications updated at this date and time or after this date and time. "
            + " Expected value is UTC using format ISO 8601-1:2019"
    )]
    [FromQuery(Name = "from")]
    public DateTime From { get; init; }

    [Description(
        "Filter import pre notifications updated before this date and time. "
            + "Expected value is UTC using format ISO 8601-1:2019"
    )]
    [FromQuery(Name = "to")]
    public DateTime To { get; init; }
}
