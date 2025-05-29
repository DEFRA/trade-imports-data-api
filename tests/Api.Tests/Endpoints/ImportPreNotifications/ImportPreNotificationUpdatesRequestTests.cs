using Defra.TradeImportsDataApi.Api.Endpoints.ImportPreNotifications;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Api.Tests.Endpoints.ImportPreNotifications;

public class ImportPreNotificationUpdatesRequestTests
{
    [Fact]
    public void WhenEmptyPointOfEntry_ShouldBeNull()
    {
        var subject = new ImportPreNotificationUpdatesRequest { PointOfEntry = [""] };

        subject.PointOfEntry.Should().BeNull();
    }

    [Fact]
    public void WhenEmptyType_ShouldBeNull()
    {
        var subject = new ImportPreNotificationUpdatesRequest { Type = [""] };

        subject.Type.Should().BeNull();
    }

    [Fact]
    public void WhenEmptyStatus_ShouldBeNull()
    {
        var subject = new ImportPreNotificationUpdatesRequest { Status = [""] };

        subject.Status.Should().BeNull();
    }

    [Fact]
    public async Task WhenFromAndToAreMoreThanOneHourApart_ShouldBeInvalid()
    {
        var subject = new ImportPreNotificationUpdatesRequest
        {
            From = new DateTime(2025, 5, 28, 13, 55, 0, DateTimeKind.Utc),
            To = new DateTime(2025, 5, 28, 14, 55, 1, DateTimeKind.Utc),
        };

        var result =
            await new ImportPreNotificationUpdatesRequest.ImportPreNotificationUpdatesRequestValidator().ValidateAsync(
                subject
            );

        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
        result.Errors.Should().Contain(x => x.ErrorMessage == "Must not be more than 1 hour(s) of From");
    }

    [Fact]
    public async Task WhenFromAndToAreAnOneHourApart_ShouldBeValid()
    {
        var subject = new ImportPreNotificationUpdatesRequest
        {
            From = new DateTime(2025, 5, 28, 13, 55, 0, DateTimeKind.Utc),
            To = new DateTime(2025, 5, 28, 14, 55, 0, DateTimeKind.Utc),
        };

        var result =
            await new ImportPreNotificationUpdatesRequest.ImportPreNotificationUpdatesRequestValidator().ValidateAsync(
                subject
            );

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task WhenFromAndToNotUtc_ShouldBeInvalid()
    {
        var subject = new ImportPreNotificationUpdatesRequest
        {
            From = new DateTime(2025, 5, 28, 13, 55, 0, DateTimeKind.Unspecified),
            To = new DateTime(2025, 5, 28, 13, 55, 0, DateTimeKind.Unspecified),
        };

        var result =
            await new ImportPreNotificationUpdatesRequest.ImportPreNotificationUpdatesRequestValidator().ValidateAsync(
                subject
            );

        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(2);
        result.Errors.Should().Contain(x => x.PropertyName == "From" && x.ErrorMessage == "Must be UTC");
        result.Errors.Should().Contain(x => x.PropertyName == "To" && x.ErrorMessage == "Must be UTC");
    }
}
