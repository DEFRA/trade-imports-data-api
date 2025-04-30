using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Tests.Utils.InMemoryData;

public class MemoryDbContext : IDbContext
{
    public IMongoCollectionSet<ImportPreNotificationEntity> ImportPreNotifications { get; } =
        new MemoryCollectionSet<ImportPreNotificationEntity>();
    public IMongoCollectionSet<CustomsDeclarationEntity> CustomsDeclarations { get; } =
        new MemoryCollectionSet<CustomsDeclarationEntity>();
    public IMongoCollectionSet<GmrEntity> Gmrs { get; } = new MemoryCollectionSet<GmrEntity>();
    public IMongoCollectionSet<ProcessingErrorEntity> ProcessingErrors { get; } =
        new MemoryCollectionSet<ProcessingErrorEntity>();

    public Task SaveChangesAsync(CancellationToken cancellation = default)
    {
        throw new NotImplementedException();
    }
}
