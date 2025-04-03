using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Services;

public interface IGmrService
{
    Task<GmrEntity?> GetGmr(string gmrId);
}