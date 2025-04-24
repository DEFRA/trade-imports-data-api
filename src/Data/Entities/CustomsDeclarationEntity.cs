using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;

namespace Defra.TradeImportsDataApi.Data.Entities
{
    public class CustomsDeclarationEntity : IDataEntity
    {
        public required string Id { get; set; }

        public List<string> ImportPreNotificationIdentifiers { get; set; } = new();

        public string ETag { get; set; } = null!;

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public ClearanceRequest? ClearanceRequest { get; set; }

        public ClearanceDecision? ClearanceDecision { get; set; }

        public Finalisation? Finalisation { get; set; }

        public void OnSave()
        {
            ImportPreNotificationIdentifiers.Clear();
            var items = ClearanceRequest?.Commodities;
            if (items == null)
                return;

            foreach (var documents in items.Select(item => item.Documents))
            {
                if (documents == null)
                    continue;
                foreach (var documentReference in documents)
                {
                    if (documentReference == null)
                        continue;
                    if (documentReference.HasValidDocumentReference())
                    {
                        ImportPreNotificationIdentifiers.Add(documentReference.GetDocumentReferenceIdentifier());
                    }
                }
            }
        }
    }
}
