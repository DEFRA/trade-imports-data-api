using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class ControlAuthorityIuuOptionTests
{
    [Theory]
    [InlineData("IUUOK", true)]
    [InlineData("iuuok", true)]
    [InlineData(null, false)]
    public void WhenIUUOK_ThenMatch(string? status, bool expected)
    {
        ControlAuthorityIuuOption.IsIUUOK(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("IUUNA", true)]
    [InlineData("iuuna", true)]
    [InlineData(null, false)]
    public void WhenIUUNA_ThenMatch(string? status, bool expected)
    {
        ControlAuthorityIuuOption.IsIUUNA(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("IUUNotCompliant", true)]
    [InlineData("iuunotcompliant", true)]
    [InlineData(null, false)]
    public void WhenIUUNotCompliant_ThenMatch(string? status, bool expected)
    {
        ControlAuthorityIuuOption.IsIUUNotCompliant(status).Should().Be(expected);
    }
}
