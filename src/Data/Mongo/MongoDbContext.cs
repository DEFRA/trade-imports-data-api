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
        ResourceEvents = new MongoCollectionSet<ResourceEventEntity>(this);
        ReportClearanceRequests = new MongoCollectionSet<ReportClearanceRequestEntity>(this);
        ReportClearanceDecisions = new MongoCollectionSet<ReportClearanceDecisionEntity>(this);
        ReportFinalisations = new MongoCollectionSet<ReportFinalisationEntity>(this);
        ReportImportPreNotifications = new MongoCollectionSet<ReportImportPreNotificationEntity>(this);
    }

    internal IMongoDatabase Database { get; }
    internal MongoDbTransaction? ActiveTransaction { get; private set; }

    public IMongoCollectionSet<ImportPreNotificationEntity> ImportPreNotifications { get; }
    public IMongoCollectionSet<ImportPreNotificationUpdateEntity> ImportPreNotificationUpdates { get; }
    public IMongoCollectionSet<CustomsDeclarationEntity> CustomsDeclarations { get; }
    public IMongoCollectionSet<GmrEntity> Gmrs { get; }
    public IMongoCollectionSet<ProcessingErrorEntity> ProcessingErrors { get; }
    public IMongoCollectionSet<ResourceEventEntity> ResourceEvents { get; }
    public IMongoCollectionSet<ReportClearanceRequestEntity> ReportClearanceRequests { get; }
    public IMongoCollectionSet<ReportClearanceDecisionEntity> ReportClearanceDecisions { get; }
    public IMongoCollectionSet<ReportFinalisationEntity> ReportFinalisations { get; }
    public IMongoCollectionSet<ReportImportPreNotificationEntity> ReportImportPreNotifications { get; }

    public async Task StartTransaction(CancellationToken cancellationToken)
    {
        if (ActiveTransaction is not null)
            throw new InvalidOperationException("Transaction already started");

        var session = await Database.Client.StartSessionAsync(cancellationToken: cancellationToken);
        session.StartTransaction();

        ActiveTransaction = new MongoDbTransaction(session);
    }

    public async Task CommitTransaction(CancellationToken cancellationToken)
    {
        if (ActiveTransaction is null)
            throw new InvalidOperationException("No active transaction");

        await ActiveTransaction.Commit(cancellationToken);

        ActiveTransaction = null;
    }

    public async Task SaveChanges(CancellationToken cancellationToken)
    {
        try
        {
            await ImportPreNotifications.Save(cancellationToken);
            await CustomsDeclarations.Save(cancellationToken);
            await Gmrs.Save(cancellationToken);
            await ProcessingErrors.Save(cancellationToken);
            await ResourceEvents.Save(cancellationToken);

            await ReportClearanceRequests.Save(cancellationToken);
            await ReportClearanceDecisions.Save(cancellationToken);
            await ReportFinalisations.Save(cancellationToken);
            await ReportImportPreNotifications.Save(cancellationToken);

            // Keep this last as upserts above will impact those below
            await ImportPreNotificationUpdates.Save(cancellationToken);
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
