using Defra.TradeImportsDataApi.Domain.Traces;
using FluentValidation;

namespace Defra.TradeImportsDataApi.Api.Endpoints.ChedReservation;

public class ReservationValidator : ValidationEndpointFilter<Reservation>
{
    public ReservationValidator()
    {
        RuleFor(x => x.UnsuccessfulReason)
            .NotEmpty()
            .When(x => x.Status == ReservationStatus.Unsuccessful)
            .WithMessage("UnsuccessfulReason Required when status is Unsuccessful");
    }
}
