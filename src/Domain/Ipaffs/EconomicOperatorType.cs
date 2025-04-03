using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Defra.TradeImportsData.Domain.IPaffs;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EconomicOperatorType
{
    Consignee,

    Destination,

    Exporter,

    Importer,

    Charity,

    CommercialTransporter,

    CommercialTransporterUserAdded,

    PrivateTransporter,

    TemporaryAddress,

    PremisesOfOrigin,

    OrganisationBranchAddress,

    Packer,

    Pod,
}
