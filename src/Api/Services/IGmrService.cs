using Defra.TradeImportsData.Data.Entities;

namespace Defra.TradeImportsData.Api.Services;

public interface IGmrService
{
    Task<GmrEntity?> GetGmr(string gmrId);
}
