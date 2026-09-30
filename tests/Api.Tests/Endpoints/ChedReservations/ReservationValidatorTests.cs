using Defra.TradeImportsDataApi.Api.Endpoints.ChedReservation;
using Defra.TradeImportsDataApi.Domain.Traces;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Api.Tests.Endpoints.ChedReservations;

public class ReservationValidatorTests
{
    private static Reservation CreateReservation(string status, string? unsuccessfulReason = null) =>
        new()
        {
            ChedId = "chedId",
            Mrn = "mrn",
            Status = status,
            UnsuccessfulReason = unsuccessfulReason,
            Timestamp = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
            Commodities = [],
        };

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task WhenUnsuccessfulAndNoReason_ShouldBeInvalid(string? unsuccessfulReason)
    {
        var subject = CreateReservation(ReservationStatus.Unsuccessful, unsuccessfulReason);

        var result = await new ReservationValidator().ValidateAsync(subject);

        result.IsValid.Should().BeFalse();
        result
            .Errors.Should()
            .Contain(x =>
                x.PropertyName == "UnsuccessfulReason"
                && x.ErrorMessage == "UnsuccessfulReason Required when status is Unsuccessful"
            );
    }

    [Fact]
    public async Task WhenUnsuccessfulAndHasReason_ShouldBeValid()
    {
        var subject = CreateReservation(ReservationStatus.Unsuccessful, "reason");

        var result = await new ReservationValidator().ValidateAsync(subject);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(ReservationStatus.Reserved)]
    [InlineData(ReservationStatus.Unreserved)]
    [InlineData(ReservationStatus.Consumed)]
    public async Task WhenNotUnsuccessfulAndNoReason_ShouldBeValid(string status)
    {
        var subject = CreateReservation(status);

        var result = await new ReservationValidator().ValidateAsync(subject);

        result.IsValid.Should().BeTrue();
    }
}
