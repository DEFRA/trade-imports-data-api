using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;

namespace Defra.TradeImportsDataApi.Data.Entities;

public class CustomsDeclarationEntity : IDataEntity
{
    public required string Id { get; set; }

    public List<string> ImportPreNotificationIdentifiers { get; set; } = [];

    public string ETag { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    public ClearanceRequest? ClearanceRequest { get; set; }

    public ClearanceDecision? ClearanceDecision { get; set; }

    public Finalisation? Finalisation { get; set; }

    public InboundError? InboundError { get; set; }

    public void OnSave()
    {
        ImportPreNotificationIdentifiers.Clear();

        if (ClearanceRequest?.Commodities == null)
            return;

        var documents = ClearanceRequest.Commodities.SelectMany(x => x.Documents ?? []);
        var references = documents
            .Where(x => x.HasValidDocumentReference())
            .Select(x => x.GetDocumentReferenceIdentifier())
            .Where(x => !string.IsNullOrEmpty(x))
            .Distinct();

        ImportPreNotificationIdentifiers.AddRange(references);
    }
}
