
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;


namespace Defra.TradeImportsData.Domain.IPaffs;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ChedppNotAcceptableReasonReasonEnum
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