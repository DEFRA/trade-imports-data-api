using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using Defra.TradeImportsDataApi.Domain.Ipaffs;

namespace Defra.TradeImportsDataApi.Data.Entities
{
    public class ImportPreNotificationEntity : IDataEntity
    {
        public required string Id { get; set; }

        public string CustomsDeclarationIdentifier { get; set; } = null!;

        public string ETag { get; set; } = null!;

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public required ImportPreNotification ImportPreNotification { get; set; }

        public void OnSave()
        {
            CustomsDeclarationIdentifier = new ImportDocumentReference(Id).GetIdentifier();
        }
    }
}
