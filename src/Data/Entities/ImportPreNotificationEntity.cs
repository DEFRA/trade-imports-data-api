using Defra.TradeImportsDataApi.Data.Configuration;
using Defra.TradeImportsDataApi.Data.Extensions;
using Defra.TradeImportsDataApi.Domain.Ipaffs;

namespace Defra.TradeImportsDataApi.Data.Entities;

[DbCollection("ImportPreNotification")]
public class ImportPreNotificationEntity : IDataEntity
{
    public required string Id { get; set; }

    public string CustomsDeclarationIdentifier { get; set; } = null!;

    public string ETag { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    public required ImportPreNotification ImportPreNotification { get; set; }

    public List<string> Tags { get; set; } = [];

    public void OnSave()
    {
        CustomsDeclarationIdentifier = new ChedIdReference(Id).GetIdentifier();
        var validator = new MrnValidator();
        if (ImportPreNotification.ExternalReferences != null)
        {
            foreach (
                var externalReference in ImportPreNotification.ExternalReferences.Where(x =>
                    x is { System: "NCTS", Reference: not null } && validator.IsValid(x.Reference)
                )
            )
            {
                AddTag(externalReference.Reference);
            }
        }
    }

    private void AddTag(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return;

        var lower = value.ToLower();

        if (!Tags.Contains(lower))
        {
            Tags.Add(lower);
        }
    }
}
