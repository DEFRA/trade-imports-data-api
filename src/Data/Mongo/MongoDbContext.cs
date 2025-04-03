using Defra.TradeImportsData.Data.Entities;
using MongoDB.Driver;

namespace Defra.TradeImportsData.Data.Mongo;

public class MongoDbContext : IDbContext
{
    public MongoDbContext(IMongoDatabase database)
    {
        Database = database;
        Notifications = new MongoCollectionSet<ImportNotificationEntity>(this);
        CustomDeclarations = new MongoCollectionSet<CustomDeclarationEntity>(this);
    }

    internal IMongoDatabase Database { get; }
    internal MongoDbTransaction? ActiveTransaction { get; private set; }

    public IMongoCollectionSet<ImportNotificationEntity> Notifications { get; }
    public IMongoCollectionSet<CustomDeclarationEntity> CustomDeclarations { get; }

    public async Task<IMongoDbTransaction> StartTransaction(CancellationToken cancellationToken = default)
    {
        var session = await Database.Client.StartSessionAsync(cancellationToken: cancellationToken);
        session.StartTransaction();
        ActiveTransaction = new MongoDbTransaction(session);
        return ActiveTransaction;
    }

    public async Task SaveChangesAsync(CancellationToken cancellation = default)
    {
        if (GetChangedRecordsCount() == 0)
        {
            return;
        }

        if (GetChangedRecordsCount() == 1)
        {
            await InternalSaveChangesAsync(cancellation);
            return;
        }

        using var transaction = await StartTransaction(cancellation);
        try
        {
            await InternalSaveChangesAsync(cancellation);
            await transaction.CommitTransaction(cancellation);
        }
        catch (Exception)
        {
            await transaction.RollbackTransaction(cancellation);
            throw;
        }
    }

    private int GetChangedRecordsCount()
    {
        return Notifications.PendingChanges;
    }

    private async Task InternalSaveChangesAsync(CancellationToken cancellation = default)
    {
        await Notifications.PersistAsync(cancellation);
    }
}
