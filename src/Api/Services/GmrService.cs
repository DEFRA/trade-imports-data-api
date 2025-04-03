using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Services;

public class GmrService(IDbContext dbContext) : IGmrService
{
    public Task<GmrEntity?> GetGmr(string gmrId) => dbContext.Gmrs.Find(gmrId);
}
