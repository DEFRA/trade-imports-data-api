using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class InspectionCheckStatusTests
{
    [Theory]
    [InlineData("To do", true)]
    [InlineData("to do", true)]
    [InlineData(null, false)]
    public void WhenTodo_ThenMatch(string? status, bool expected)
    {
        InspectionCheckStatus.IsTodo(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Compliant", true)]
    [InlineData("compliant", true)]
    [InlineData(null, false)]
    public void WhenCompliant_ThenMatch(string? status, bool expected)
    {
        InspectionCheckStatus.IsCompliant(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Auto cleared", true)]
    [InlineData("auto cleared", true)]
    [InlineData(null, false)]
    public void WhenAutoCleared_ThenMatch(string? status, bool expected)
    {
        InspectionCheckStatus.IsAutoCleared(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Non compliant", true)]
    [InlineData("non compliant", true)]
    [InlineData(null, false)]
    public void WhenNonCompliant_ThenMatch(string? status, bool expected)
    {
        InspectionCheckStatus.IsNonCompliant(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Not inspected", true)]
    [InlineData("not inspected", true)]
    [InlineData(null, false)]
    public void WhenNotInspected_ThenMatch(string? status, bool expected)
    {
        InspectionCheckStatus.IsNotInspected(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("To be inspected", true)]
    [InlineData("to be inspected", true)]
    [InlineData(null, false)]
    public void WhenToBeInspected_ThenMatch(string? status, bool expected)
    {
        InspectionCheckStatus.IsToBeInspected(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Hold", true)]
    [InlineData("hold", true)]
    [InlineData(null, false)]
    public void WhenHold_ThenMatch(string? status, bool expected)
    {
        InspectionCheckStatus.IsHold(status).Should().Be(expected);
    }
}
