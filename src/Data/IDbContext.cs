using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Data;

public interface IDbContext
{
    IMongoCollectionSet<ImportPreNotificationEntity> Notifications { get; }

    IMongoCollectionSet<CustomsDeclarationEntity> CustomDeclarations { get; }

    IMongoCollectionSet<GmrEntity> Gmrs { get; }
    Task SaveChangesAsync(CancellationToken cancellation = default);
}
