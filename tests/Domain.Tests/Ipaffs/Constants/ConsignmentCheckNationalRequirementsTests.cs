using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class ConsignmentCheckNationalRequirementsTests
{
    [Theory]
    [InlineData("Satisfactory", true)]
    [InlineData("satisfactory", true)]
    [InlineData(null, false)]
    public void WhenSatisfactory_ThenMatch(string? status, bool expected)
    {
        ConsignmentCheckNationalRequirements.IsSatisfactory(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Satisfactory following official intervention", true)]
    [InlineData("satisfactory following official intervention", true)]
    [InlineData(null, false)]
    public void WhenSatisfactoryFollowingOfficialIntervention_ThenMatch(string? status, bool expected)
    {
        ConsignmentCheckNationalRequirements.IsSatisfactoryFollowingOfficialIntervention(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Not Satisfactory", true)]
    [InlineData("not satisfactory", true)]
    [InlineData(null, false)]
    public void WhenNotSatisfactory_ThenMatch(string? status, bool expected)
    {
        ConsignmentCheckNationalRequirements.IsNotSatisfactory(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Not Done", true)]
    [InlineData("not done", true)]
    [InlineData(null, false)]
    public void WhenNotDone_ThenMatch(string? status, bool expected)
    {
        ConsignmentCheckNationalRequirements.IsNotDone(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Derogation", true)]
    [InlineData("derogation", true)]
    [InlineData(null, false)]
    public void WhenDerogation_ThenMatch(string? status, bool expected)
    {
        ConsignmentCheckNationalRequirements.IsDerogation(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Not Set", true)]
    [InlineData("not set", true)]
    [InlineData(null, false)]
    public void WhenNotSet_ThenMatch(string? status, bool expected)
    {
        ConsignmentCheckNationalRequirements.IsNotSet(status).Should().Be(expected);
    }
}
