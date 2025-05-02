namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class ChedppNotAcceptableReasonReason
{
    public const string DocPhmdm = "doc-phmdm";
    public const string DocPhmdii = "doc-phmdii";
    public const string DocPa = "doc-pa";
    public const string DocPic = "doc-pic";
    public const string DocPill = "doc-pill";
    public const string DocPed = "doc-ped";
    public const string DocPmod = "doc-pmod";
    public const string DocPfi = "doc-pfi";
    public const string DocPnol = "doc-pnol";
    public const string DocPcne = "doc-pcne";
    public const string DocPadm = "doc-padm";
    public const string DocPadi = "doc-padi";
    public const string DocPpni = "doc-ppni";
    public const string DocPf = "doc-pf";
    public const string DocPo = "doc-po";
    public const string DocNcevd = "doc-ncevd";
    public const string DocNcpqefi = "doc-ncpqefi";
    public const string DocNcpqebec = "doc-ncpqebec";
    public const string DocNcts = "doc-ncts";
    public const string DocNco = "doc-nco";
    public const string DocOrii = "doc-orii";
    public const string DocOrsr = "doc-orsr";
    public const string OriOrrnu = "ori-orrnu";
    public const string PhyOrpp = "phy-orpp";
    public const string PhyOrho = "phy-orho";
    public const string PhyIs = "phy-is";
    public const string PhyOrsr = "phy-orsr";
    public const string OthCnl = "oth-cnl";
    public const string OthO = "oth-o";

    public static bool IsDocPhmdm(string? status) => Equals(DocPhmdm, status);

    public static bool IsDocPhmdii(string? status) => Equals(DocPhmdii, status);

    public static bool IsDocPa(string? status) => Equals(DocPa, status);

    public static bool IsDocPic(string? status) => Equals(DocPic, status);

    public static bool IsDocPill(string? status) => Equals(DocPill, status);

    public static bool IsDocPed(string? status) => Equals(DocPed, status);

    public static bool IsDocPmod(string? status) => Equals(DocPmod, status);

    public static bool IsDocPfi(string? status) => Equals(DocPfi, status);

    public static bool IsDocPnol(string? status) => Equals(DocPnol, status);

    public static bool IsDocPcne(string? status) => Equals(DocPcne, status);

    public static bool IsDocPadm(string? status) => Equals(DocPadm, status);

    public static bool IsDocPadi(string? status) => Equals(DocPadi, status);

    public static bool IsDocPpni(string? status) => Equals(DocPpni, status);

    public static bool IsDocPf(string? status) => Equals(DocPf, status);

    public static bool IsDocPo(string? status) => Equals(DocPo, status);

    public static bool IsDocNcevd(string? status) => Equals(DocNcevd, status);

    public static bool IsDocNcpqefi(string? status) => Equals(DocNcpqefi, status);

    public static bool IsDocNcpqebec(string? status) => Equals(DocNcpqebec, status);

    public static bool IsDocNcts(string? status) => Equals(DocNcts, status);

    public static bool IsDocNco(string? status) => Equals(DocNco, status);

    public static bool IsDocOrii(string? status) => Equals(DocOrii, status);

    public static bool IsDocOrsr(string? status) => Equals(DocOrsr, status);

    public static bool IsOriOrrnu(string? status) => Equals(OriOrrnu, status);

    public static bool IsPhyOrpp(string? status) => Equals(PhyOrpp, status);

    public static bool IsPhyOrho(string? status) => Equals(PhyOrho, status);

    public static bool IsPhyIs(string? status) => Equals(PhyIs, status);

    public static bool IsPhyOrsr(string? status) => Equals(PhyOrsr, status);

    public static bool IsOthCnl(string? status) => Equals(OthCnl, status);

    public static bool IsOthO(string? status) => Equals(OthO, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}
