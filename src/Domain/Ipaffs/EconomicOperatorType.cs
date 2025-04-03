using System.Text.Json.Serialization;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

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
