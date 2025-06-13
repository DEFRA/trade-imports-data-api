using System.ComponentModel;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Global

namespace Defra.TradeImportsDataApi.Api.Endpoints.ImportPreNotifications;

public class ImportPreNotificationUpdatesRequest
{
    private readonly string[]? _pointOfEntry;
    private readonly string[]? _type;
    private readonly string[]? _status;
    private readonly string[]? _excludeStatus;
    private readonly int? _page;
    private readonly int? _pageSize;

    [Description(
        "Filter import pre notifications updated at this date and time or after this date and time. "
            + " Expected value is UTC using format ISO 8601-1:2019"
    )]
    [FromQuery(Name = "from")]
    public DateTime From { get; init; }

    [Description(
        "Filter import pre notifications updated before this date and time. "
            + "Expected value is UTC using format ISO 8601-1:2019. Cannot be more than 1 hour of From"
    )]
    [FromQuery(Name = "to")]
    public DateTime To { get; init; }

    [Description(
        "Filter import pre notifications by point of entry. Multiple should be specified as pointOfEntry=A&pointOfEntry=B etc. Either a Border-Inspection-Post or Designated-Point-Of-Entry, e.g. GBFXT1"
    )]
    [FromQuery(Name = "pointOfEntry")]
    public string[]? PointOfEntry
    {
        get => _pointOfEntry?.All(string.IsNullOrWhiteSpace) == true ? null : _pointOfEntry;
        init => _pointOfEntry = value;
    }

    [Description("Filter import pre notifications by type. Multiple should be specified as type=A&type=B etc.")]
    [FromQuery(Name = "type")]
    public string[]? Type
    {
        get => _type?.All(string.IsNullOrWhiteSpace) == true ? null : _type;
        init => _type = value;
    }

    [Description("Filter import pre notifications by status. Multiple should be specified as status=A&status=B etc.")]
    [FromQuery(Name = "status")]
    public string[]? Status
    {
        get => _status?.All(string.IsNullOrWhiteSpace) == true ? null : _status;
        init => _status = value;
    }

    [Description(
        "Filter import pre notifications by NOT IN status. Multiple should be specified as excludeStatus=A&excludeStatus=B etc."
    )]
    [FromQuery(Name = "excludeStatus")]
    public string[]? ExcludeStatus
    {
        get => _excludeStatus?.All(string.IsNullOrWhiteSpace) == true ? null : _excludeStatus;
        init => _excludeStatus = value;
    }

    [Description("Page number (1-based). Defaults to 1 if not specified.")]
    [FromQuery(Name = "page")]
    public int? Page
    {
        get => _page ?? 1;
        init => _page = value;
    }

    [Description("Number of items per page. Defaults to 100 if not specified. Max of 1000.")]
    [FromQuery(Name = "pageSize")]
    public int? PageSize
    {
        get => _pageSize ?? 100;
        init => _pageSize = value;
    }

    public class ImportPreNotificationUpdatesRequestValidator
        : ValidationEndpointFilter<ImportPreNotificationUpdatesRequest>
    {
        public ImportPreNotificationUpdatesRequestValidator()
        {
            RuleFor(x => x.From).Must(x => x.Kind == DateTimeKind.Utc).WithMessage("Must be UTC");
            RuleFor(x => x.To).Must(x => x.Kind == DateTimeKind.Utc).WithMessage("Must be UTC");
            RuleFor(x => (x.From - x.To).Duration())
                .LessThanOrEqualTo(TimeSpan.FromHours(1))
                .WithName(nameof(To))
                .WithMessage(
                    $"Must not be more than {TimeSpan.FromHours(1).Duration().TotalHours} hour(s) of {nameof(From)}"
                );
            RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).LessThanOrEqualTo(1000);
        }
    }
}
