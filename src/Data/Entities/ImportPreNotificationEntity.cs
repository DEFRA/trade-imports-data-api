using Defra.TradeImportsDataApi.Domain.Ipaffs;

namespace Defra.TradeImportsDataApi.Data.Entities;

public class ImportPreNotificationEntity : IDataEntity
{
    public required string Id { get; set; }

    public string CustomsDeclarationIdentifier { get; set; } = null!;

    public List<string> MrnIdentifiers { get; set; } = new(); // add index on this

    public string ETag { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    public required ImportPreNotification ImportPreNotification { get; set; }

    public void OnSave()
    {
        CustomsDeclarationIdentifier = new ChedIdReference(Id).GetIdentifier();

        MrnIdentifiers.Clear();

        foreach (
            var externalReference in ImportPreNotification.ExternalReferences?.Where(x =>
                x.System != null
                && x.System.Equals("NCTS", StringComparison.OrdinalIgnoreCase)
                && !string.IsNullOrWhiteSpace(x.Reference)
            ) ?? []
        )
        {
            MrnIdentifiers.Add(externalReference.Reference!);
        }
    }
}
