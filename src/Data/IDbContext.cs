using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Data;

public interface IDbContext
{
    IMongoCollectionSet<ImportPreNotificationEntity> ImportPreNotifications { get; }

    IMongoCollectionSet<ImportPreNotificationUpdateEntity> ImportPreNotificationUpdates { get; }

    IMongoCollectionSet<CustomsDeclarationEntity> CustomsDeclarations { get; }

    IMongoCollectionSet<GmrEntity> Gmrs { get; }

    IMongoCollectionSet<ProcessingErrorEntity> ProcessingErrors { get; }

    IMongoCollectionSet<ResourceEventEntity> ResourceEvents { get; }

    IMongoCollectionSet<ReportClearanceRequestEntity> ReportClearanceRequests { get; }

    IMongoCollectionSet<ReportClearanceDecisionEntity> ReportClearanceDecisions { get; }

    IMongoCollectionSet<ReportFinalisationEntity> ReportFinalisations { get; }

    IMongoCollectionSet<ReportImportPreNotificationEntity> ReportImportPreNotifications { get; }

    Task SaveChanges(CancellationToken cancellationToken);

    Task StartTransaction(CancellationToken cancellationToken);

    Task CommitTransaction(CancellationToken cancellationToken);
}
