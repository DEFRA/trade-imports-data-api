using Defra.TradeImportsData.Data.Entities;

namespace Defra.TradeImportsData.Data;

public interface IDbContext
{
    IMongoCollectionSet<ImportNotificationEntity> Notifications { get; }

    IMongoCollectionSet<CustomsDeclarationEntity> CustomDeclarations { get; }

    IMongoCollectionSet<GmrEntity> Gmrs { get; }
    Task SaveChangesAsync(CancellationToken cancellation = default);
}
