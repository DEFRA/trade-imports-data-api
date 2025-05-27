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

    [Description(
        "Filter import pre notifications by point of entry. Multiple should be specified as pointOfEntry=A&pointOfEntry=B etc. Either a Border-Inspection-Post or Designated-Point-Of-Entry, e.g. GBFXT1"
    )]
    [FromQuery(Name = "pointOfEntry")]
    public string[]? PointOfEntry { get; init; }

    [Description("Filter import pre notifications by type. Multiple should be specified as type=A&type=B etc.")]
    [FromQuery(Name = "type")]
    public string[]? Type { get; init; }

    [Description("Filter import pre notifications by status. Multiple should be specified as status=A&status=B etc.")]
    [FromQuery(Name = "status")]
    public string[]? Status { get; init; }
}
