using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Tests.Utils.InMemoryData;

public class MemoryDbContext : IDbContext
{
    public IMongoCollectionSet<ImportPreNotificationEntity> ImportPreNotifications { get; } =
        new MemoryCollectionSet<ImportPreNotificationEntity>();

    public IMongoCollectionSet<ImportPreNotificationUpdateEntity> ImportPreNotificationUpdates { get; } =
        new MemoryCollectionSet<ImportPreNotificationUpdateEntity>();

    public IMongoCollectionSet<CustomsDeclarationEntity> CustomsDeclarations { get; } =
        new MemoryCollectionSet<CustomsDeclarationEntity>();

    public IMongoCollectionSet<GmrEntity> Gmrs { get; } = new MemoryCollectionSet<GmrEntity>();

    public IMongoCollectionSet<ProcessingErrorEntity> ProcessingErrors { get; } =
        new MemoryCollectionSet<ProcessingErrorEntity>();

    public IMongoCollectionSet<ResourceEventEntity> ResourceEvents { get; } =
        new MemoryCollectionSet<ResourceEventEntity>();

    public IMongoCollectionSet<ReportClearanceRequestEntity> ReportClearanceRequests { get; } =
        new MemoryCollectionSet<ReportClearanceRequestEntity>();

    public IMongoCollectionSet<ReportClearanceDecisionEntity> ReportClearanceDecisions { get; } =
        new MemoryCollectionSet<ReportClearanceDecisionEntity>();

    public IMongoCollectionSet<ReportFinalisationEntity> ReportFinalisations { get; } =
        new MemoryCollectionSet<ReportFinalisationEntity>();

    public IMongoCollectionSet<ReportImportPreNotificationEntity> ReportImportPreNotifications { get; } =
        new MemoryCollectionSet<ReportImportPreNotificationEntity>();

    public Task SaveChanges(CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task StartTransaction(CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task CommitTransaction(CancellationToken cancellationToken) => throw new NotImplementedException();
}
