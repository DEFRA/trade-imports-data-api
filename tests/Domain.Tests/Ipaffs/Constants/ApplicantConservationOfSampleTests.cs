using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class ApplicantConservationOfSampleTests
{
    [Theory]
    [InlineData("Ambient", true)]
    [InlineData("ambient", true)]
    [InlineData(null, false)]
    public void WhenAmbient_ThenMatch(string? status, bool expected)
    {
        ApplicantConservationOfSample.IsAmbient(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Chilled", true)]
    [InlineData("chilled", true)]
    [InlineData(null, false)]
    public void WhenChilled_ThenMatch(string? status, bool expected)
    {
        ApplicantConservationOfSample.IsChilled(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Frozen", true)]
    [InlineData("frozen", true)]
    [InlineData(null, false)]
    public void WhenFrozen_ThenMatch(string? status, bool expected)
    {
        ApplicantConservationOfSample.IsFrozen(status).Should().Be(expected);
    }
}
