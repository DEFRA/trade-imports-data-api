using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class PurposeForImportOrAdmissionTests
{
    [Theory]
    [InlineData("Definitive import", true)]
    [InlineData("definitive import", true)]
    [InlineData(null, false)]
    public void WhenDefinitiveImport_ThenMatch(string? status, bool expected)
    {
        PurposeForImportOrAdmission.IsDefinitiveImport(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Horses Re-entry", true)]
    [InlineData("horses re-entry", true)]
    [InlineData(null, false)]
    public void WhenHorsesReEntry_ThenMatch(string? status, bool expected)
    {
        PurposeForImportOrAdmission.IsHorsesReEntry(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("Temporary admission horses", true)]
    [InlineData("temporary admission horses", true)]
    [InlineData(null, false)]
    public void WhenTemporaryAdmissionHorses_ThenMatch(string? status, bool expected)
    {
        PurposeForImportOrAdmission.IsTemporaryAdmissionHorses(status).Should().Be(expected);
    }
}
