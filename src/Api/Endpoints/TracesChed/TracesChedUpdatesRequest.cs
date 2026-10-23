using System.ComponentModel;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Global

namespace Defra.TradeImportsDataApi.Api.Endpoints.TracesChed;

public class TracesChedUpdatesRequest
{
    private readonly int? _page;
    private readonly int? _pageSize;

    [Description(
        "Filter TRACES CHEDs updated at this date and time or after this date and time. "
            + " Expected value is UTC using format ISO 8601-1:2019"
    )]
    [FromQuery(Name = "from")]
    public DateTime From { get; init; }

    [Description(
        "Filter TRACES CHEDs updated before this date and time. "
            + "Expected value is UTC using format ISO 8601-1:2019. Cannot be more than 1 hour of From"
    )]
    [FromQuery(Name = "to")]
    public DateTime To { get; init; }

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

    public class TracesChedUpdatesRequestValidator : ValidationEndpointFilter<TracesChedUpdatesRequest>
    {
        public TracesChedUpdatesRequestValidator()
        {
            RuleFor(x => x.From).Must(x => x.Kind == DateTimeKind.Utc).WithMessage("Must be UTC");
            RuleFor(x => x.To).Must(x => x.Kind == DateTimeKind.Utc).WithMessage("Must be UTC");
            RuleFor(x => (x.From - x.To).Duration())
                .LessThanOrEqualTo(TimeSpan.FromHours(1))
                .WithName(nameof(To))
                .WithMessage(
                    $"Must not be more than {TimeSpan.FromHours(1).Duration().TotalHours} hour(s) of {nameof(From)}"
                );
            RuleFor(x => x.To).GreaterThan(x => x.From).WithMessage($"Must be after {nameof(From)}");
            RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).LessThanOrEqualTo(1000);
        }
    }
}
