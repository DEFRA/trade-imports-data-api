using System.Diagnostics.CodeAnalysis;
using Defra.TradeImportsDataApi.Api.Exceptions;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Events;
using MongoDB.Driver.Linq;

namespace Defra.TradeImportsDataApi.Api.Services;

[ExcludeFromCodeCoverage] // see integration tests
public class ImportPreNotificationService(IDbContext dbContext, IResourceEventPublisher resourceEventPublisher)
    : IImportPreNotificationService
{
    public async Task<ImportPreNotificationEntity?> GetImportPreNotification(
        string chedId,
        CancellationToken cancellationToken
    )
    {
        return await dbContext.ImportPreNotifications.Find(chedId, cancellationToken);
    }

    public async Task<List<ImportPreNotificationEntity>> GetImportPreNotificationsByMrn(
        string mrn,
        CancellationToken cancellationToken
    )
    {
        var identifiers = await dbContext
            .CustomsDeclarations.Where(x => x.Id == mrn)
            .SelectMany(x => x.ImportPreNotificationIdentifiers)
            .ToListAsync(cancellationToken);

        return await dbContext
            .ImportPreNotifications.Where(x => identifiers.Contains(x.CustomsDeclarationIdentifier))
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<ImportPreNotificationEntity> Insert(
        ImportPreNotificationEntity importPreNotificationEntity,
        CancellationToken cancellationToken
    )
    {
        await dbContext.ImportPreNotifications.Insert(importPreNotificationEntity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        await resourceEventPublisher.Publish(
            importPreNotificationEntity.ToResourceEvent(
                ResourceEventOperations.Created,
                includeEntityAsResource: false
            ),
            cancellationToken
        );

        return importPreNotificationEntity;
    }

    public async Task<ImportPreNotificationEntity> Update(
        ImportPreNotificationEntity importPreNotificationEntity,
        string etag,
        CancellationToken cancellationToken
    )
    {
        var existing = await dbContext.ImportPreNotifications.Find(importPreNotificationEntity.Id, cancellationToken);
        if (existing == null)
        {
            throw new EntityNotFoundException(nameof(ImportPreNotificationEntity), importPreNotificationEntity.Id);
        }

        importPreNotificationEntity.Created = existing.Created;

        await dbContext.ImportPreNotifications.Update(importPreNotificationEntity, etag, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        await resourceEventPublisher.Publish(
            importPreNotificationEntity.ToResourceEvent(
                ResourceEventOperations.Updated,
                includeEntityAsResource: false
            ),
            cancellationToken
        );

        return importPreNotificationEntity;
    }

    public async Task<List<ImportPreNotificationUpdate>> GetImportPreNotificationUpdates(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken
    )
    {
        var gmrsQuery = GetGmrUpdatesQuery(from, to);
        var customsDeclarationsQuery = GetCustomsDeclarationUpdatesQuery(from, to);
        var importPreNotificationsQuery = GetImportPreNotificationUpdatesQuery(from, to);

        var gmrsTask = gmrsQuery.ToListAsync(cancellationToken);
        var customsDeclarationsTask = customsDeclarationsQuery.ToListAsync(cancellationToken);
        var importPreNotificationsTask = importPreNotificationsQuery.ToListAsync(cancellationToken);

        await Task.WhenAll(gmrsTask, customsDeclarationsTask, importPreNotificationsTask);

        var gmrs = gmrsTask.Result;
        var customsDeclarations = customsDeclarationsTask.Result;
        var importPreNotifications = importPreNotificationsTask.Result;

        return gmrs.Concat(customsDeclarations)
            .Concat(importPreNotifications)
            .GroupBy(x => x.ReferenceNumber)
            .Select(x => new ImportPreNotificationUpdate(x.Key, x.Max(y => y.Updated)))
            .ToList();
    }

    private IQueryable<ImportPreNotificationUpdate> GetImportPreNotificationUpdatesQuery(DateTime from, DateTime to)
    {
        var query = dbContext
            .ImportPreNotifications.AsQueryable()
            .Where(x => x.Updated >= from && x.Updated < to)
            .Select(x => new ImportPreNotificationUpdate(x.Id, x.Updated));

        return query;
    }

    private IQueryable<ImportPreNotificationUpdate> GetCustomsDeclarationUpdatesQuery(DateTime from, DateTime to)
    {
        var query = dbContext
            .CustomsDeclarations.AsQueryable()
            // Filter on Updated date and only interested in customs declarations with identifiers (as they could link to import pre notifications)
            .Where(x => x.Updated >= from && x.Updated < to && x.ImportPreNotificationIdentifiers.Count > 0)
            .Select(x => new { x.Updated, x.ImportPreNotificationIdentifiers })
            // Join with import pre notifications
            .Lookup(
                dbContext.ImportPreNotifications.Collection,
                (customsDeclaration, importPreNotifications) =>
                    importPreNotifications.Where(x =>
                        customsDeclaration.ImportPreNotificationIdentifiers.Contains(x.CustomsDeclarationIdentifier)
                    )
            )
            // Exclude if no import pre notifications
            .Where(x => x.Results.Any())
            .Select(x => new ImportPreNotificationUpdate(
                x.Results.FirstOrDefault()!.Id,
                new[] { x.Local.Updated, x.Results.Max(y => y.Updated) }.Max()
            ));

        return query;
    }

    private IQueryable<ImportPreNotificationUpdate> GetGmrUpdatesQuery(DateTime from, DateTime to)
    {
        var query = dbContext
            .Gmrs.AsQueryable()
            // Filter on Updated date and only interested in GMRs with MRN IDs (as they could link to customs declarations)
            .Where(x => x.Updated >= from && x.Updated < to && x.MrnIdentifiers.Count > 0)
            .Select(x => new { x.Updated, MrnIds = x.MrnIdentifiers })
            // Join with customs declarations
            .Lookup(
                dbContext.CustomsDeclarations.Collection,
                (gmr, customsDeclarations) => customsDeclarations.Where(x => gmr.MrnIds.Contains(x.Id))
            )
            // Exclude if no customs declarations
            .Where(x => x.Results.Any())
            .Select(x => new
            {
                GmrUpdated = x.Local.Updated,
                CustomsDeclarationUpdated = x.Results.Max(y => y.Updated),
                ImportPreNotificationIdentifiers = x.Results.SelectMany(y => y.ImportPreNotificationIdentifiers),
            })
            // Join with import pre notifications
            .Lookup(
                dbContext.ImportPreNotifications.Collection,
                (gmrWithDeclarations, importPreNotifications) =>
                    importPreNotifications.Where(x =>
                        gmrWithDeclarations.ImportPreNotificationIdentifiers.Contains(x.CustomsDeclarationIdentifier)
                    )
            )
            // Exclude if no import pre notifications
            .Where(x => x.Results.Any())
            .Select(x => new ImportPreNotificationUpdate(
                x.Results.FirstOrDefault()!.Id,
                new[] { x.Local.GmrUpdated, x.Local.CustomsDeclarationUpdated, x.Results.Max(y => y.Updated) }.Max()
            ));

        return query;
    }
}
