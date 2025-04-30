using Defra.TradeImportsDataApi.Domain.Ipaffs;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs;

public class ImportNotificationStatusTests
{
    [Theory]
    [InlineData("DRAFT", true)]
    [InlineData("draft", true)]
    [InlineData(null, false)]
    public void WhenDraft_ThenMatch(string? status, bool expected)
    {
        ImportNotificationStatus.IsDraft(status).Should().Be(expected);

        new ImportPreNotification { Status = status }
            .StatusIsDraft()
            .Should()
            .Be(expected);
    }

    [Theory]
    [InlineData("SUBMITTED", true)]
    [InlineData("submitted", true)]
    [InlineData(null, false)]
    public void WhenSubmitted_ThenMatch(string? status, bool expected)
    {
        ImportNotificationStatus.IsSubmitted(status).Should().Be(expected);

        new ImportPreNotification { Status = status }
            .StatusIsSubmitted()
            .Should()
            .Be(expected);
    }

    [Theory]
    [InlineData("VALIDATED", true)]
    [InlineData("validated", true)]
    [InlineData(null, false)]
    public void WhenValidated_ThenMatch(string? status, bool expected)
    {
        ImportNotificationStatus.IsValidated(status).Should().Be(expected);

        new ImportPreNotification { Status = status }
            .StatusIsValidated()
            .Should()
            .Be(expected);
    }

    [Theory]
    [InlineData("REJECTED", true)]
    [InlineData("rejected", true)]
    [InlineData(null, false)]
    public void WhenRejected_ThenMatch(string? status, bool expected)
    {
        ImportNotificationStatus.IsRejected(status).Should().Be(expected);

        new ImportPreNotification { Status = status }
            .StatusIsRejected()
            .Should()
            .Be(expected);
    }

    [Theory]
    [InlineData("CANCELLED", true)]
    [InlineData("cancelled", true)]
    [InlineData(null, false)]
    public void WhenCancelled_ThenMatch(string? status, bool expected)
    {
        ImportNotificationStatus.IsCancelled(status).Should().Be(expected);

        new ImportPreNotification { Status = status }
            .StatusIsCancelled()
            .Should()
            .Be(expected);
    }

    [Theory]
    [InlineData("IN_PROGRESS", true)]
    [InlineData("in_progress", true)]
    [InlineData(null, false)]
    public void WhenInProgress_ThenMatch(string? status, bool expected)
    {
        ImportNotificationStatus.IsInProgress(status).Should().Be(expected);

        new ImportPreNotification { Status = status }
            .StatusIsInProgress()
            .Should()
            .Be(expected);
    }

    [Theory]
    [InlineData("AMEND", true)]
    [InlineData("amend", true)]
    [InlineData(null, false)]
    public void WhenAmend_ThenMatch(string? status, bool expected)
    {
        ImportNotificationStatus.IsAmend(status).Should().Be(expected);

        new ImportPreNotification { Status = status }
            .StatusIsAmend()
            .Should()
            .Be(expected);
    }

    [Theory]
    [InlineData("MODIFY", true)]
    [InlineData("modify", true)]
    [InlineData(null, false)]
    public void WhenModify_ThenMatch(string? status, bool expected)
    {
        ImportNotificationStatus.IsModify(status).Should().Be(expected);

        new ImportPreNotification { Status = status }
            .StatusIsModify()
            .Should()
            .Be(expected);
    }

    [Theory]
    [InlineData("REPLACED", true)]
    [InlineData("replaced", true)]
    [InlineData(null, false)]
    public void WhenReplaced_ThenMatch(string? status, bool expected)
    {
        ImportNotificationStatus.IsReplaced(status).Should().Be(expected);

        new ImportPreNotification { Status = status }
            .StatusIsReplaced()
            .Should()
            .Be(expected);
    }

    [Theory]
    [InlineData("DELETED", true)]
    [InlineData("deleted", true)]
    [InlineData(null, false)]
    public void WhenDeleted_ThenMatch(string? status, bool expected)
    {
        ImportNotificationStatus.IsDeleted(status).Should().Be(expected);

        new ImportPreNotification { Status = status }
            .StatusIsDeleted()
            .Should()
            .Be(expected);
    }

    [Theory]
    [InlineData("PARTIALLY_REJECTED", true)]
    [InlineData("partially_rejected", true)]
    [InlineData(null, false)]
    public void WhenPartiallyRejected_ThenMatch(string? status, bool expected)
    {
        ImportNotificationStatus.IsPartiallyRejected(status).Should().Be(expected);

        new ImportPreNotification { Status = status }
            .StatusIsPartiallyRejected()
            .Should()
            .Be(expected);
    }
}
