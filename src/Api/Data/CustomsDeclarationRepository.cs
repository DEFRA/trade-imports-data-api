using System.Linq.Expressions;
using Defra.TradeImportsDataApi.Api.Exceptions;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Data.Extensions;
using MongoDB.Driver.Linq;

namespace Defra.TradeImportsDataApi.Api.Data;

public class CustomsDeclarationRepository(IDbContext dbContext) : ICustomsDeclarationRepository
{
    public async Task<CustomsDeclarationEntity?> Get(string id, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(id))
            return null;

        return await dbContext.CustomsDeclarations.Find(id, cancellationToken);
    }

    public async Task<List<CustomsDeclarationEntity>> GetAll(
        string importPreNotificationIdentifier,
        CancellationToken cancellationToken
    )
    {
        if (string.IsNullOrWhiteSpace(importPreNotificationIdentifier))
            return [];

        return await dbContext
            .CustomsDeclarations.Where(x =>
                x.ImportPreNotificationIdentifiers.Contains(importPreNotificationIdentifier)
            )
            .ToListWithFallbackAsync(cancellationToken);
    }

    public Task<List<CustomsDeclarationEntity>> GetAll(
        Expression<Func<CustomsDeclarationEntity, bool>> predicate,
        CancellationToken cancellationToken
    ) => dbContext.CustomsDeclarations.Where(predicate).ToListWithFallbackAsync(cancellationToken);

    public async Task<List<string>> GetAllIds(
        string importPreNotificationIdentifier,
        CancellationToken cancellationToken
    )
    {
        if (string.IsNullOrWhiteSpace(importPreNotificationIdentifier))
            return [];

        return await dbContext
            .CustomsDeclarations.Where(x =>
                x.ImportPreNotificationIdentifiers.Contains(importPreNotificationIdentifier)
            )
            .Select(x => x.Id)
            .Distinct()
            .ToListWithFallbackAsync(cancellationToken);
    }

    public async Task<List<string>> GetAllImportPreNotificationIdentifiers(
        string id,
        CancellationToken cancellationToken
    )
    {
        if (string.IsNullOrWhiteSpace(id))
            return [];

        return await dbContext
            .CustomsDeclarations.Where(x => x.Id == id)
            .SelectMany(x => x.ImportPreNotificationIdentifiers)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<string>> GetAllImportPreNotificationIdentifiers(
        string[] ids,
        CancellationToken cancellationToken
    )
    {
        if (ids.Length == 0)
            return [];

        return await dbContext
            .CustomsDeclarations.Where(x => ids.Contains(x.Id))
            .SelectMany(x => x.ImportPreNotificationIdentifiers)
            .ToListAsync(cancellationToken);
    }

    public CustomsDeclarationEntity Insert(CustomsDeclarationEntity entity)
    {
        dbContext.CustomsDeclarations.Insert(entity);

        return entity;
    }

    public async Task<(CustomsDeclarationEntity Existing, CustomsDeclarationEntity Updated)> Update(
        CustomsDeclarationEntity entity,
        string etag,
        CancellationToken cancellationToken
    )
    {
        var existing = await Get(entity.Id, cancellationToken);
        if (existing == null)
        {
            throw new EntityNotFoundException(nameof(CustomsDeclarationEntity), entity.Id);
        }

        entity.Created = existing.Created;

        dbContext.CustomsDeclarations.Update(entity, etag);

        return (existing, entity);
    }
}
