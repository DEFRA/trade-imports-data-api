using Defra.TradeImportsData.Data;
using Defra.TradeImportsData.Data.Entities;

namespace Defra.TradeImportsData.Api.Services;

public class GmrService(IDbContext dbContext) : IGmrService
{
    public Task<GmrEntity?> GetGmr(string gmrId) => dbContext.Gmrs.Find(gmrId);
}
