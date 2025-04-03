using Defra.TradeImportsData.Domain.CustomsDeclaration.ClearanceRequest;
using Defra.TradeImportsData.Domain.CustomsDeclaration.Finalisation;
using Decision = Defra.TradeImportsData.Domain.CustomsDeclaration.Decision.Decision;

namespace Defra.TradeImportsData.Data.Entities
{
    public class CustomDeclarationEntity : IDataEntity
    {
        public required string Id { get; set; }

        public List<string> ImportNotificationIdentifiers { get; set; } = new();

        public string ETag { get; set; } = null!;

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public ClearanceRequest? ClearanceRequest { get; set; }

        public Decision? Decision { get; set; }

        public Finalisation? Finalisation { get; set; }

        public void OnSave()
        {
            ImportNotificationIdentifiers.Clear();
            if (ClearanceRequest?.Items == null)
                return;

            foreach (var documents in ClearanceRequest.Items.Select(item => item.Documents))
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
