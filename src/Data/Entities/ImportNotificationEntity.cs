using Defra.TradeImportsData.Domain.CustomsDeclaration.ClearanceRequest;
using Defra.TradeImportsData.Domain.IPaffs;

namespace Defra.TradeImportsData.Data.Entities
{
    public class ImportNotificationEntity : IDataEntity
    {
        public required string Id { get; set; }

        public required string CustomDeclarationIdentifier { get; set; }

        public string ETag { get; set; } = null!;

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public required ImportNotification Data { get; set; }

        public void OnSave()
        {
            CustomDeclarationIdentifier = new DocumentReference(Id).GetIdentifier();
        }
    }
}
