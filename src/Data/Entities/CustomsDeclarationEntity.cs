using Defra.TradeImportsData.Domain.CustomsDeclaration;

namespace Defra.TradeImportsData.Data.Entities
{
    public class CustomsDeclarationEntity : IDataEntity
    {
        public required string Id { get; set; }

        public List<string> ImportNotificationIdentifiers { get; set; } = new();

        public string ETag { get; set; } = null!;

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public required CustomsDeclaration Data { get; set; }

        public void OnSave()
        {
            ImportNotificationIdentifiers.Clear();
            var items = Data?.ClearanceRequest?.Items;
            if (items == null)
                return;

            foreach (var documents in items.Select(item => item.Documents))
            {
                if (documents == null)
                    continue;
                foreach (var documentReference in documents.Select(doc => doc.DocumentReference))
                {
                    if (documentReference == null)
                        continue;
                    if (documentReference.IsValid())
                    {
                        ImportNotificationIdentifiers.Add(documentReference.GetIdentifier());
                    }
                }
            }
        }
    }
}
