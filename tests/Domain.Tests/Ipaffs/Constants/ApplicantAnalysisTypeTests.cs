using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class ApplicantAnalysisTypeTests
{
    [Theory]
    [InlineData("Initial analysis", true)]
    [InlineData("initial analysis", true)]
    [InlineData(null, false)]
    public void WhenInitialAnalysis_ThenMatch(string? status, bool expected)
    {
        ApplicantAnalysisType.IsInitialAnalysis(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Counter analysis", true)]
    [InlineData("counter analysis", true)]
    [InlineData(null, false)]
    public void WhenCounterAnalysis_ThenMatch(string? status, bool expected)
    {
        ApplicantAnalysisType.IsCounterAnalysis(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Second expert analysis", true)]
    [InlineData("second expert analysis", true)]
    [InlineData(null, false)]
    public void WhenSecondExpertAnalysis_ThenMatch(string? status, bool expected)
    {
        ApplicantAnalysisType.IsSecondExpertAnalysis(status).Should().Be(expected);
    }
}
