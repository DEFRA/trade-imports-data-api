using Defra.TradeImportsData.Api.Domain;

namespace Defra.TradeImportsData.Api.Services;

public class GmrService : IGmrService
{
    public Task<Gmr?> GetGmr(string gmrId) => Task.FromResult<Gmr?>(null);
}
