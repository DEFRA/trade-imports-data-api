using Defra.TradeImportsData.Api.Domain;

namespace Defra.TradeImportsData.Api.Services;

public interface IGmrService
{
    Task<Gmr?> GetGmr(string gmrId);
}
