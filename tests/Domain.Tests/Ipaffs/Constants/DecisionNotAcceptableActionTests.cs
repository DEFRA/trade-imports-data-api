using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class DecisionNotAcceptableActionTests
{
    [Theory]
    [InlineData("slaughter", true)]
    [InlineData("SLAUGHTER", true)]
    [InlineData(null, false)]
    public void WhenSlaughter_ThenMatch(string? status, bool expected)
    {
        DecisionNotAcceptableAction.IsSlaughter(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("reexport", true)]
    [InlineData("REEXPORT", true)]
    [InlineData(null, false)]
    public void WhenReexport_ThenMatch(string? status, bool expected)
    {
        DecisionNotAcceptableAction.IsReexport(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("euthanasia", true)]
    [InlineData("EUTHANASIA", true)]
    [InlineData(null, false)]
    public void WhenEuthanasia_ThenMatch(string? status, bool expected)
    {
        DecisionNotAcceptableAction.IsEuthanasia(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("redispatching", true)]
    [InlineData("REDISPATCHING", true)]
    [InlineData(null, false)]
    public void WhenRedispatching_ThenMatch(string? status, bool expected)
    {
        DecisionNotAcceptableAction.IsRedispatching(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("destruction", true)]
    [InlineData("DESTRUCTION", true)]
    [InlineData(null, false)]
    public void WhenDestruction_ThenMatch(string? status, bool expected)
    {
        DecisionNotAcceptableAction.IsDestruction(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("transformation", true)]
    [InlineData("TRANSFORMATION", true)]
    [InlineData(null, false)]
    public void WhenTransformation_ThenMatch(string? status, bool expected)
    {
        DecisionNotAcceptableAction.IsTransformation(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("other", true)]
    [InlineData("OTHER", true)]
    [InlineData(null, false)]
    public void WhenOther_ThenMatch(string? status, bool expected)
    {
        DecisionNotAcceptableAction.IsOther(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("entry-refusal", true)]
    [InlineData("ENTRY-REFUSAL", true)]
    [InlineData(null, false)]
    public void WhenEntryRefusal_ThenMatch(string? status, bool expected)
    {
        DecisionNotAcceptableAction.IsEntryRefusal(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("quarantine-imposed", true)]
    [InlineData("QUARANTINE-IMPOSED", true)]
    [InlineData(null, false)]
    public void WhenQuarantineImposed_ThenMatch(string? status, bool expected)
    {
        DecisionNotAcceptableAction.IsQuarantineImposed(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("special-treatment", true)]
    [InlineData("SPECIAL-TREATMENT", true)]
    [InlineData(null, false)]
    public void WhenSpecialTreatment_ThenMatch(string? status, bool expected)
    {
        DecisionNotAcceptableAction.IsSpecialTreatment(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("industrial-processing", true)]
    [InlineData("INDUSTRIAL-PROCESSING", true)]
    [InlineData(null, false)]
    public void WhenIndustrialProcessing_ThenMatch(string? status, bool expected)
    {
        DecisionNotAcceptableAction.IsIndustrialProcessing(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("re-dispatch", true)]
    [InlineData("RE-DISPATCH", true)]
    [InlineData(null, false)]
    public void WhenReDispatch_ThenMatch(string? status, bool expected)
    {
        DecisionNotAcceptableAction.IsReDispatch(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("use-for-other-purposes", true)]
    [InlineData("USE-FOR-OTHER-PURPOSES", true)]
    [InlineData(null, false)]
    public void WhenUseForOtherPurposes_ThenMatch(string? status, bool expected)
    {
        DecisionNotAcceptableAction.IsUseForOtherPurposes(status).Should().Be(expected);
    }
}
