
namespace Defra.TradeImportsData.Domain.IPaffs;

public partial class ImportNotification 
{


     public Commodities CommoditiesSummary { get; set; } = default!;

     public CommodityComplement[] Commodities { get; set; } = default!;
}


public class ImportNotificationStatus
{
    public static ImportNotificationStatus Default()
    {
        return new ImportNotificationStatus()
        {
            LinkStatus = LinkStatus.NotLinked
        };
    }

    
    [System.ComponentModel.Description("")]
    [MongoDB.Bson.Serialization.Attributes.BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public TypeAndLinkStatus? TypeAndLinkStatus { get; set; }

    
    [System.ComponentModel.Description("")]
    [MongoDB.Bson.Serialization.Attributes.BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public required LinkStatus LinkStatus { get; set; }
}