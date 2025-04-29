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
        if (!string.IsNullOrEmpty(request.Ducr))
        {
            return await StartFromCustomsDeclaration(
                x => x.ClearanceRequest!.DeclarationUcr == request.Ducr,
                cancellationToken
            );
        }

        if (!string.IsNullOrEmpty(request.Mrn))
        {
            return await StartFromCustomsDeclaration(x => x.Id == request.Mrn, cancellationToken);
        }

        if (!string.IsNullOrEmpty(request.ChedId))
        {
            return await StartFromImportPreNotification(request.ChedId, cancellationToken);
        }

        return new ValueTuple<CustomsDeclarationEntity[], ImportPreNotificationEntity[]>([], []);
    }

    private async Task<(
        CustomsDeclarationEntity[] CustomsDeclaration,
        ImportPreNotificationEntity[] ImportPreNotifications
    )> StartFromCustomsDeclaration(
        Expression<Func<CustomsDeclarationEntity, bool>> predicate,
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

        return new ValueTuple<CustomsDeclarationEntity[], ImportPreNotificationEntity[]>(
            customsDeclarations.ToArray(),
            notifications.ToArray()
        );
    }

    private async Task<(
        CustomsDeclarationEntity[] CustomsDeclaration,
        ImportPreNotificationEntity[] ImportPreNotifications
    )> StartFromImportPreNotification(string chedId, CancellationToken cancellationToken)
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

        return new ValueTuple<CustomsDeclarationEntity[], ImportPreNotificationEntity[]>(
            customsDeclarations.ToArray(),
            [notification]
        );
    }
}
