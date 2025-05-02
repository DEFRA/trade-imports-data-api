using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class ImportNotificationTypeTests
{
    [Theory]
    [InlineData("CVEDA", true)]
    [InlineData("cveda", true)]
    [InlineData(null, false)]
    public void WhenCveda_ThenMatch(string? status, bool expected)
    {
        ImportNotificationType.IsCveda(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("CVEDP", true)]
    [InlineData("cvedp", true)]
    [InlineData(null, false)]
    public void WhenCvedp_ThenMatch(string? status, bool expected)
    {
        ImportNotificationType.IsCvedp(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("CHEDPP", true)]
    [InlineData("chedpp", true)]
    [InlineData(null, false)]
    public void WhenChedpp_ThenMatch(string? status, bool expected)
    {
        ImportNotificationType.IsChedpp(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("CED", true)]
    [InlineData("ced", true)]
    [InlineData(null, false)]
    public void WhenCed_ThenMatch(string? status, bool expected)
    {
        ImportNotificationType.IsCed(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("IMP", true)]
    [InlineData("imp", true)]
    [InlineData(null, false)]
    public void WhenImp_ThenMatch(string? status, bool expected)
    {
        ImportNotificationType.IsImp(status).Should().Be(expected);
    }
}
