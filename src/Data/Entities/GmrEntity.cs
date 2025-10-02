using Defra.TradeImportsDataApi.Data.Extensions;
using Defra.TradeImportsDataApi.Domain.Gvms;

namespace Defra.TradeImportsDataApi.Data.Entities;

public class GmrEntity : IDataEntity
{
    public required string Id { get; set; }

    public List<string> CustomsDeclarationIdentifiers { get; set; } = [];

    public string ETag { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    public required Gmr Gmr { get; set; }

    public void OnSave()
    {
        CustomsDeclarationIdentifiers.Clear();

        var customs = Gmr.Declarations?.Customs?.Select(x => x.Id) ?? [];
        var transits = Gmr.Declarations?.Transits?.Select(x => x.Id) ?? [];
        var unique = customs.Concat(transits).Where(x => !string.IsNullOrWhiteSpace(x)).NotNull().Distinct();

        CustomsDeclarationIdentifiers.AddRange(unique);
    }
}
