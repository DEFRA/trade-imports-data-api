using Defra.TradeImportsDataApi.Domain.Gvms;

namespace Defra.TradeImportsDataApi.Data.Entities;

public class GmrEntity : IDataEntity
{
    public required string Id { get; set; }

    public List<string> MrnIdentifiers { get; set; } = new(); // add index on this

    public string ETag { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    public required Gmr Gmr { get; set; }

    public void OnSave()
    {
        MrnIdentifiers.Clear();

        var transits =
            Gmr.Declarations?.Transits?.Select(x => x.Id?.ToLowerInvariant() ?? string.Empty)
                .Where(x => !string.IsNullOrWhiteSpace(x)) ?? [];
        var customs =
            Gmr.Declarations?.Customs?.Select(x => x.Id?.ToLowerInvariant() ?? string.Empty)
                .Where(x => !string.IsNullOrWhiteSpace(x)) ?? [];

        MrnIdentifiers.AddRange(transits.Concat(customs).Distinct());
    }
}
