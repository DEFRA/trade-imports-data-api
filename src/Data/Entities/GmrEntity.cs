using Defra.TradeImportsDataApi.Data.Configuration;
using Defra.TradeImportsDataApi.Data.Extensions;
using Defra.TradeImportsDataApi.Domain.Gvms;

namespace Defra.TradeImportsDataApi.Data.Entities;

[DbCollection("Gmr")]
public class GmrEntity : IDataEntity
{
    public required string Id { get; set; }

    public List<string> CustomsDeclarationIdentifiers { get; set; } = [];

    public string ETag { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    public required Gmr Gmr { get; set; }

    public List<string> Tags { get; set; } = [];

    public void OnSave()
    {
        CustomsDeclarationIdentifiers.Clear();

        var customs = Gmr.Declarations?.Customs?.Select(x => x.Id) ?? [];
        var transits = Gmr.Declarations?.Transits?.Select(x => x.Id) ?? [];
        var unique = customs.Concat(transits).Where(x => !string.IsNullOrWhiteSpace(x)).NotNull().Distinct();

        CustomsDeclarationIdentifiers.AddRange(unique);

        if (Gmr.TrailerRegistrationNums is not null)
        {
            foreach (var value in Gmr.TrailerRegistrationNums)
            {
                AddTag(value);
            }
        }
        AddTag(Gmr.VehicleRegistrationNumber);
        AddTag(Id);
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
