using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class ConsignmentCheckEuStandardTests
{
    [Theory]
    [InlineData("Satisfactory", true)]
    [InlineData("satisfactory", true)]
    [InlineData(null, false)]
    public void WhenSatisfactory_ThenMatch(string? status, bool expected)
    {
        ConsignmentCheckEuStandard.IsSatisfactory(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Satisfactory following official intervention", true)]
    [InlineData("satisfactory following official intervention", true)]
    [InlineData(null, false)]
    public void WhenSatisfactoryFollowingOfficialIntervention_ThenMatch(string? status, bool expected)
    {
        ConsignmentCheckEuStandard.IsSatisfactoryFollowingOfficialIntervention(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Not Satisfactory", true)]
    [InlineData("not satisfactory", true)]
    [InlineData(null, false)]
    public void WhenNotSatisfactory_ThenMatch(string? status, bool expected)
    {
        ConsignmentCheckEuStandard.IsNotSatisfactory(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Not Done", true)]
    [InlineData("not done", true)]
    [InlineData(null, false)]
    public void WhenNotDone_ThenMatch(string? status, bool expected)
    {
        ConsignmentCheckEuStandard.IsNotDone(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Derogation", true)]
    [InlineData("derogation", true)]
    [InlineData(null, false)]
    public void WhenDerogation_ThenMatch(string? status, bool expected)
    {
        ConsignmentCheckEuStandard.IsDerogation(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Not Set", true)]
    [InlineData("not set", true)]
    [InlineData(null, false)]
    public void WhenNotSet_ThenMatch(string? status, bool expected)
    {
        ConsignmentCheckEuStandard.IsNotSet(status).Should().Be(expected);
    }
}
