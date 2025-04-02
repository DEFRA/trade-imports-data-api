using Defra.TradeImportsData.Data.Entities;

namespace Defra.TradeImportsData.Data;

public interface IDbContext
{
    IMongoCollectionSet<ImportNotificationEntity> Notifications { get; }
    Task SaveChangesAsync(CancellationToken cancellation = default);
}