using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class NotificationTypeTests
{
    [Theory]
    [InlineData("CVEDA", true)]
    [InlineData("cveda", true)]
    [InlineData(null, false)]
    public void WhenCveda_ThenMatch(string? status, bool expected)
    {
        NotificationType.IsCveda(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("CVEDP", true)]
    [InlineData("cvedp", true)]
    [InlineData(null, false)]
    public void WhenCvedp_ThenMatch(string? status, bool expected)
    {
        NotificationType.IsCvedp(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("CHEDPP", true)]
    [InlineData("chedpp", true)]
    [InlineData(null, false)]
    public void WhenChedpp_ThenMatch(string? status, bool expected)
    {
        NotificationType.IsChedpp(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("CED", true)]
    [InlineData("ced", true)]
    [InlineData(null, false)]
    public void WhenCed_ThenMatch(string? status, bool expected)
    {
        NotificationType.IsCed(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("IMP", true)]
    [InlineData("imp", true)]
    [InlineData(null, false)]
    public void WhenImp_ThenMatch(string? status, bool expected)
    {
        NotificationType.IsImp(status).Should().Be(expected);
    }
}
