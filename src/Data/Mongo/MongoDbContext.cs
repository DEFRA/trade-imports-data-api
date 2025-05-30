using System.Diagnostics.CodeAnalysis;
using Defra.TradeImportsDataApi.Data.Entities;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Defra.TradeImportsDataApi.Data.Mongo;

[ExcludeFromCodeCoverage]
public class MongoDbContext : IDbContext
{
    private readonly ILogger<MongoDbContext> _logger;

    public MongoDbContext(IMongoDatabase database, ILogger<MongoDbContext> logger)
    {
        _logger = logger;
        Database = database;
        ImportPreNotifications = new MongoCollectionSet<ImportPreNotificationEntity>(this);
        ImportPreNotificationUpdates = new MongoCollectionSet<ImportPreNotificationUpdateEntity>(this);
        CustomsDeclarations = new MongoCollectionSet<CustomsDeclarationEntity>(this);
        Gmrs = new MongoCollectionSet<GmrEntity>(this);
        ProcessingErrors = new MongoCollectionSet<ProcessingErrorEntity>(this);
    }

    internal IMongoDatabase Database { get; }
    internal MongoDbTransaction? ActiveTransaction { get; private set; }

    public IMongoCollectionSet<ImportPreNotificationEntity> ImportPreNotifications { get; }
    public IMongoCollectionSet<ImportPreNotificationUpdateEntity> ImportPreNotificationUpdates { get; }
    public IMongoCollectionSet<CustomsDeclarationEntity> CustomsDeclarations { get; }
    public IMongoCollectionSet<GmrEntity> Gmrs { get; }
    public IMongoCollectionSet<ProcessingErrorEntity> ProcessingErrors { get; }

    private async Task<IMongoDbTransaction> StartTransaction(CancellationToken cancellationToken = default)
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
        // This logic needs to be reviewed as it's easy to forget to include any new collection sets
        return ImportPreNotifications.PendingChanges
            + ImportPreNotificationUpdates.PendingChanges
            + CustomsDeclarations.PendingChanges
            + Gmrs.PendingChanges
            + ProcessingErrors.PendingChanges;
    }

    private async Task InternalSaveChangesAsync(CancellationToken cancellation = default)
    {
        try
        {
            // This logic needs to be reviewed as it's easy to forget to include any new collection sets
            await ImportPreNotifications.PersistAsync(cancellation);
            await CustomsDeclarations.PersistAsync(cancellation);
            await Gmrs.PersistAsync(cancellation);
            await ProcessingErrors.PersistAsync(cancellation);

            // Keep this last as upserts above will impact those below
            await ImportPreNotificationUpdates.PersistAsync(cancellation);
        }
        catch (MongoCommandException mongoCommandException) when (mongoCommandException.Code == 112)
        {
            const string message = "Mongo write conflict - consumer will retry";
            _logger.LogWarning(mongoCommandException, message);

            // WriteConflict error: this operation conflicted with another operation. Please retry your operation or multi-document transaction
            // - retries are built into consumers of the data API
            throw new ConcurrencyException(message, mongoCommandException);
        }
        catch (MongoWriteException mongoWriteException) when (mongoWriteException.WriteError.Code == 11000)
        {
            const string message = "Mongo write error - consumer will retry";
            _logger.LogWarning(mongoWriteException, message);

            // A write operation resulted in an error. WriteError: { Category : "DuplicateKey", Code : 11000 }
            // - retries are built into consumers of the data API
            throw new ConcurrencyException(message, mongoWriteException);
        }
    }
}
