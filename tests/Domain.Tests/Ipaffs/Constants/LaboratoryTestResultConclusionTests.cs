using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class LaboratoryTestResultConclusionTests
{
    [Theory]
    [InlineData("Satisfactory", true)]
    [InlineData("satisfactory", true)]
    [InlineData(null, false)]
    public void WhenSatisfactory_ThenMatch(string? status, bool expected)
    {
        LaboratoryTestResultConclusion.IsSatisfactory(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Not satisfactory", true)]
    [InlineData("not satisfactory", true)]
    [InlineData(null, false)]
    public void WhenNotSatisfactory_ThenMatch(string? status, bool expected)
    {
        LaboratoryTestResultConclusion.IsNotSatisfactory(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Not interpretable", true)]
    [InlineData("not interpretable", true)]
    [InlineData(null, false)]
    public void WhenNotInterpretable_ThenMatch(string? status, bool expected)
    {
        LaboratoryTestResultConclusion.IsNotInterpretable(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Pending", true)]
    [InlineData("pending", true)]
    [InlineData(null, false)]
    public void WhenPending_ThenMatch(string? status, bool expected)
    {
        LaboratoryTestResultConclusion.IsPending(status).Should().Be(expected);
    }
}
