using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class ChedppNotAcceptableReasonReasonTests
{
    [Theory]
    [InlineData("doc-phmdm", true)]
    [InlineData("DOC-PHMDM", true)]
    [InlineData(null, false)]
    public void WhenDocPhmdm_ThenMatch(string? status, bool expected)
    {
        ChedppNotAcceptableReasonReason.IsDocPhmdm(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("doc-phmdii", true)]
    [InlineData("DOC-PHMDII", true)]
    [InlineData(null, false)]
    public void WhenDocPhmdii_ThenMatch(string? status, bool expected)
    {
        ChedppNotAcceptableReasonReason.IsDocPhmdii(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("doc-pa", true)]
    [InlineData("DOC-PA", true)]
    [InlineData(null, false)]
    public void WhenDocPa_ThenMatch(string? status, bool expected)
    {
        ChedppNotAcceptableReasonReason.IsDocPa(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("doc-pic", true)]
    [InlineData("DOC-PIC", true)]
    [InlineData(null, false)]
    public void WhenDocPic_ThenMatch(string? status, bool expected)
    {
        ChedppNotAcceptableReasonReason.IsDocPic(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("doc-pill", true)]
    [InlineData("DOC-PILL", true)]
    [InlineData(null, false)]
    public void WhenDocPill_ThenMatch(string? status, bool expected)
    {
        ChedppNotAcceptableReasonReason.IsDocPill(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("doc-ped", true)]
    [InlineData("DOC-PED", true)]
    [InlineData(null, false)]
    public void WhenDocPed_ThenMatch(string? status, bool expected)
    {
        ChedppNotAcceptableReasonReason.IsDocPed(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("doc-pmod", true)]
    [InlineData("DOC-PMOD", true)]
    [InlineData(null, false)]
    public void WhenDocPmod_ThenMatch(string? status, bool expected)
    {
        ChedppNotAcceptableReasonReason.IsDocPmod(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("doc-pfi", true)]
    [InlineData("DOC-PFI", true)]
    [InlineData(null, false)]
    public void WhenDocPfi_ThenMatch(string? status, bool expected)
    {
        ChedppNotAcceptableReasonReason.IsDocPfi(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("doc-pnol", true)]
    [InlineData("DOC-PNOL", true)]
    [InlineData(null, false)]
    public void WhenDocPnol_ThenMatch(string? status, bool expected)
    {
        ChedppNotAcceptableReasonReason.IsDocPnol(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("doc-pcne", true)]
    [InlineData("DOC-PCNE", true)]
    [InlineData(null, false)]
    public void WhenDocPcne_ThenMatch(string? status, bool expected)
    {
        ChedppNotAcceptableReasonReason.IsDocPcne(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("doc-padm", true)]
    [InlineData("DOC-PADM", true)]
    [InlineData(null, false)]
    public void WhenDocPadm_ThenMatch(string? status, bool expected)
    {
        ChedppNotAcceptableReasonReason.IsDocPadm(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("doc-padi", true)]
    [InlineData("DOC-PADI", true)]
    [InlineData(null, false)]
    public void WhenDocPadi_ThenMatch(string? status, bool expected)
    {
        ChedppNotAcceptableReasonReason.IsDocPadi(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("doc-ppni", true)]
    [InlineData("DOC-PPNI", true)]
    [InlineData(null, false)]
    public void WhenDocPpni_ThenMatch(string? status, bool expected)
    {
        ChedppNotAcceptableReasonReason.IsDocPpni(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("doc-pf", true)]
    [InlineData("DOC-PF", true)]
    [InlineData(null, false)]
    public void WhenDocPf_ThenMatch(string? status, bool expected)
    {
        ChedppNotAcceptableReasonReason.IsDocPf(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("doc-po", true)]
    [InlineData("DOC-PO", true)]
    [InlineData(null, false)]
    public void WhenDocPo_ThenMatch(string? status, bool expected)
    {
        ChedppNotAcceptableReasonReason.IsDocPo(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("doc-ncevd", true)]
    [InlineData("DOC-NCEVD", true)]
    [InlineData(null, false)]
    public void WhenDocNcevd_ThenMatch(string? status, bool expected)
    {
        ChedppNotAcceptableReasonReason.IsDocNcevd(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("doc-ncpqefi", true)]
    [InlineData("DOC-NCPQEFI", true)]
    [InlineData(null, false)]
    public void WhenDocNcpqefi_ThenMatch(string? status, bool expected)
    {
        ChedppNotAcceptableReasonReason.IsDocNcpqefi(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("doc-ncpqebec", true)]
    [InlineData("DOC-NCPQEBEC", true)]
    [InlineData(null, false)]
    public void WhenDocNcpqebec_ThenMatch(string? status, bool expected)
    {
        ChedppNotAcceptableReasonReason.IsDocNcpqebec(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("doc-ncts", true)]
    [InlineData("DOC-NCTS", true)]
    [InlineData(null, false)]
    public void WhenDocNcts_ThenMatch(string? status, bool expected)
    {
        ChedppNotAcceptableReasonReason.IsDocNcts(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("doc-nco", true)]
    [InlineData("DOC-NCO", true)]
    [InlineData(null, false)]
    public void WhenDocNco_ThenMatch(string? status, bool expected)
    {
        ChedppNotAcceptableReasonReason.IsDocNco(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("doc-orii", true)]
    [InlineData("DOC-ORII", true)]
    [InlineData(null, false)]
    public void WhenDocOrii_ThenMatch(string? status, bool expected)
    {
        ChedppNotAcceptableReasonReason.IsDocOrii(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("doc-orsr", true)]
    [InlineData("DOC-ORSR", true)]
    [InlineData(null, false)]
    public void WhenDocOrsr_ThenMatch(string? status, bool expected)
    {
        ChedppNotAcceptableReasonReason.IsDocOrsr(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("ori-orrnu", true)]
    [InlineData("ORI-ORRNU", true)]
    [InlineData(null, false)]
    public void WhenOriOrrnu_ThenMatch(string? status, bool expected)
    {
        ChedppNotAcceptableReasonReason.IsOriOrrnu(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("phy-orpp", true)]
    [InlineData("PHY-ORPP", true)]
    [InlineData(null, false)]
    public void WhenPhyOrpp_ThenMatch(string? status, bool expected)
    {
        ChedppNotAcceptableReasonReason.IsPhyOrpp(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("phy-orho", true)]
    [InlineData("PHY-ORHO", true)]
    [InlineData(null, false)]
    public void WhenPhyOrho_ThenMatch(string? status, bool expected)
    {
        ChedppNotAcceptableReasonReason.IsPhyOrho(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("phy-is", true)]
    [InlineData("PHY-IS", true)]
    [InlineData(null, false)]
    public void WhenPhyIs_ThenMatch(string? status, bool expected)
    {
        ChedppNotAcceptableReasonReason.IsPhyIs(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("phy-orsr", true)]
    [InlineData("PHY-ORSR", true)]
    [InlineData(null, false)]
    public void WhenPhyOrsr_ThenMatch(string? status, bool expected)
    {
        ChedppNotAcceptableReasonReason.IsPhyOrsr(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("oth-cnl", true)]
    [InlineData("OTH-CNL", true)]
    [InlineData(null, false)]
    public void WhenOthCnl_ThenMatch(string? status, bool expected)
    {
        ChedppNotAcceptableReasonReason.IsOthCnl(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("oth-o", true)]
    [InlineData("OTH-O", true)]
    [InlineData(null, false)]
    public void WhenOthO_ThenMatch(string? status, bool expected)
    {
        ChedppNotAcceptableReasonReason.IsOthO(status).Should().Be(expected);
    }
}
