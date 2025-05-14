using System.Linq.Expressions;
using Defra.TradeImportsDataApi.Api.Endpoints.Search;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Data.Extensions;

namespace Defra.TradeImportsDataApi.Api.Services;

public class RelatedImportDeclarationsService(IDbContext dbContext) : IRelatedImportDeclarationsService
{
    public async Task<(
        CustomsDeclarationEntity[] CustomsDeclaration,
        ImportPreNotificationEntity[] ImportPreNotifications
    )> Search(RelatedImportDeclarationsRequest request, CancellationToken cancellationToken)
    {
        var maxDepth = 3;
        if (request.MaxLinkDepth.HasValue)
        {
            maxDepth = request.MaxLinkDepth.Value;
        }

        if (!string.IsNullOrEmpty(request.Ducr))
        {
            return await StartFromCustomsDeclaration(
                x => x.ClearanceRequest!.DeclarationUcr!.ToLowerInvariant() == request.Ducr.ToLowerInvariant(),
                maxDepth,
                cancellationToken
            );
        }

        if (!string.IsNullOrEmpty(request.Mrn))
        {
            return await StartFromCustomsDeclaration(x => x.Id.ToLowerInvariant() == request.Mrn.ToLowerInvariant(), maxDepth, cancellationToken);
        }

        if (!string.IsNullOrEmpty(request.ChedId))
        {
            return await StartFromImportPreNotification(request.ChedId, maxDepth, cancellationToken);
        }

        return new ValueTuple<CustomsDeclarationEntity[], ImportPreNotificationEntity[]>([], []);
    }

    private async Task<(
        CustomsDeclarationEntity[] CustomsDeclaration,
        ImportPreNotificationEntity[] ImportPreNotifications
    )> StartFromCustomsDeclaration(
        Expression<Func<CustomsDeclarationEntity, bool>> predicate,
        int maxDepth,
        CancellationToken cancellationToken
    )
    {
        var customsDeclarations = await dbContext
            .CustomsDeclarations.Where(predicate)
            .ToListAsync(cancellationToken: cancellationToken);
        var identifiers = customsDeclarations.SelectMany(x => x.ImportPreNotificationIdentifiers);
        var notifications = await dbContext
            .ImportPreNotifications.Where(x => identifiers.Contains(x.CustomsDeclarationIdentifier))
            .ToListAsync(cancellationToken: cancellationToken);

        return await IncludeIndirectLinks(
            new ValueTuple<CustomsDeclarationEntity[], ImportPreNotificationEntity[]>(
                customsDeclarations.ToArray(),
                notifications.ToArray()
            ),
            0,
            maxDepth,
            cancellationToken
        );
    }

    private async Task<(
        CustomsDeclarationEntity[] CustomsDeclaration,
        ImportPreNotificationEntity[] ImportPreNotifications
    )> StartFromImportPreNotification(string chedId, int maxDepth, CancellationToken cancellationToken)
    {
        var identifier = chedId.Substring(chedId.Length - 7);
        var notification = (
            await dbContext
                .ImportPreNotifications.Where(x => x.CustomsDeclarationIdentifier == identifier)
                .ToListAsync(cancellationToken)
        ).SingleOrDefault();

        if (notification == null)
        {
            return new ValueTuple<CustomsDeclarationEntity[], ImportPreNotificationEntity[]>([], []);
        }

        var customsDeclarations = await dbContext
            .CustomsDeclarations.Where(x =>
                x.ImportPreNotificationIdentifiers.Contains(notification.CustomsDeclarationIdentifier)
            )
            .ToListAsync(cancellationToken: cancellationToken);

        return await IncludeIndirectLinks(
            new ValueTuple<CustomsDeclarationEntity[], ImportPreNotificationEntity[]>(
                customsDeclarations.ToArray(),
                [notification]
            ),
            0,
            maxDepth,
            cancellationToken
        );
    }

    private async Task<(
        CustomsDeclarationEntity[] CustomsDeclaration,
        ImportPreNotificationEntity[] ImportPreNotifications
    )> IncludeIndirectLinks(
        (CustomsDeclarationEntity[] CustomsDeclaration, ImportPreNotificationEntity[] ImportPreNotifications) data,
        int currentDepth,
        int maxDepth,
        CancellationToken cancellationToken
    )
    {
        if (currentDepth >= maxDepth)
        {
            return data;
        }

        var customsDeclarations = data.CustomsDeclaration.ToList();
        var customsDeclarationIds = customsDeclarations.Select(x => x.Id);
        var importPreNotifications = data.ImportPreNotifications.ToList();
        var importPreNotificationIds = importPreNotifications.Select(x => x.Id);

        var identifiers = data
            .CustomsDeclaration.SelectMany(x => x.ImportPreNotificationIdentifiers)
            .Union(data.ImportPreNotifications.Select(x => x.CustomsDeclarationIdentifier))
            .Distinct()
            .ToList();

        if (identifiers.Any())
        {
            importPreNotifications.AddRange(
                await dbContext
                    .ImportPreNotifications.Where(x =>
                        identifiers.Contains(x.CustomsDeclarationIdentifier) && !importPreNotificationIds.Contains(x.Id)
                    )
                    .ToListAsync(cancellationToken: cancellationToken)
            );

            customsDeclarations.AddRange(
                await dbContext
                    .CustomsDeclarations.Where(x =>
                        x.ImportPreNotificationIdentifiers.Any(y => identifiers.Contains(y))
                        && !customsDeclarationIds.Contains(x.Id)
                    )
                    .ToListAsync(cancellationToken: cancellationToken)
            );
        }

        var response = new ValueTuple<CustomsDeclarationEntity[], ImportPreNotificationEntity[]>(
            customsDeclarations.ToArray(),
            importPreNotifications.ToArray()
        );

        // bail out of the recursive loop if there are no records loaded
        if (
            response.Item1.Length == data.CustomsDeclaration.Length
            && response.Item2.Length == data.ImportPreNotifications.Length
        )
        {
            return response;
        }

        return await IncludeIndirectLinks(
            new ValueTuple<CustomsDeclarationEntity[], ImportPreNotificationEntity[]>(
                customsDeclarations.ToArray(),
                importPreNotifications.ToArray()
            ),
            currentDepth + 1,
            maxDepth,
            cancellationToken
        );
    }
}
