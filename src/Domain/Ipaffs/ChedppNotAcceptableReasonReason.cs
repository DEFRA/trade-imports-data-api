using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ChedppNotAcceptableReasonReason
{
    DocPhmdm,

    DocPhmdii,

    DocPa,

    DocPic,

    DocPill,

    DocPed,

    DocPmod,

    DocPfi,

    DocPnol,

    DocPcne,

    DocPadm,

    DocPadi,

    DocPpni,

    DocPf,

    DocPo,

    DocNcevd,

    DocNcpqefi,

    DocNcpqebec,

    DocNcts,

    DocNco,

    DocOrii,

    DocOrsr,

    OriOrrnu,

    PhyOrpp,

    PhyOrho,

    PhyIs,

    PhyOrsr,

    OthCnl,

    OthO,
}
