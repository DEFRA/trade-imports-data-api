using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class ConsignmentCheckIdentityCheckTypeTests
{
    [Theory]
    [InlineData("Seal Check", true)]
    [InlineData("seal check", true)]
    [InlineData(null, false)]
    public void WhenSealCheck_ThenMatch(string? status, bool expected)
    {
        ConsignmentCheckIdentityCheckType.IsSealCheck(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Full Identity Check", true)]
    [InlineData("full identity check", true)]
    [InlineData(null, false)]
    public void WhenFullIdentityCheck_ThenMatch(string? status, bool expected)
    {
        ConsignmentCheckIdentityCheckType.IsFullIdentityCheck(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Not Done", true)]
    [InlineData("not done", true)]
    [InlineData(null, false)]
    public void WhenNotDone_ThenMatch(string? status, bool expected)
    {
        ConsignmentCheckIdentityCheckType.IsNotDone(status).Should().Be(expected);
    }
}
