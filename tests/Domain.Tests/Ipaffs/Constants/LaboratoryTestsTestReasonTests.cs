using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class LaboratoryTestsTestReasonTests
{
    [Theory]
    [InlineData("Random", true)]
    [InlineData("random", true)]
    [InlineData(null, false)]
    public void WhenRandom_ThenMatch(string? status, bool expected)
    {
        LaboratoryTestsTestReason.IsRandom(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Suspicious", true)]
    [InlineData("suspicious", true)]
    [InlineData(null, false)]
    public void WhenSuspicious_ThenMatch(string? status, bool expected)
    {
        LaboratoryTestsTestReason.IsSuspicious(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Re-enforced", true)]
    [InlineData("re-enforced", true)]
    [InlineData(null, false)]
    public void WhenReEnforced_ThenMatch(string? status, bool expected)
    {
        LaboratoryTestsTestReason.IsReEnforced(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Intensified controls", true)]
    [InlineData("intensified controls", true)]
    [InlineData(null, false)]
    public void WhenIntensifiedControls_ThenMatch(string? status, bool expected)
    {
        LaboratoryTestsTestReason.IsIntensifiedControls(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Required", true)]
    [InlineData("required", true)]
    [InlineData(null, false)]
    public void WhenRequired_ThenMatch(string? status, bool expected)
    {
        LaboratoryTestsTestReason.IsRequired(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Latent infection sampling", true)]
    [InlineData("latent infection sampling", true)]
    [InlineData(null, false)]
    public void WhenLatentInfectionSampling_ThenMatch(string? status, bool expected)
    {
        LaboratoryTestsTestReason.IsLatentInfectionSampling(status).Should().Be(expected);
    }
}
