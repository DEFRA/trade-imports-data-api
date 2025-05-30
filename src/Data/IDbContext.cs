using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Data;

public interface IDbContext
{
    IMongoCollectionSet<ImportPreNotificationEntity> ImportPreNotifications { get; }

    IMongoCollectionSet<ImportPreNotificationUpdateEntity> ImportPreNotificationUpdates { get; }

    IMongoCollectionSet<CustomsDeclarationEntity> CustomsDeclarations { get; }

    IMongoCollectionSet<GmrEntity> Gmrs { get; }

    IMongoCollectionSet<ProcessingErrorEntity> ProcessingErrors { get; }

    Task SaveChangesAsync(CancellationToken cancellation = default);
}
