using System.Linq.Expressions;
using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Api.Endpoints.RelatedImportDeclarations;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using Defra.TradeImportsDataApi.Domain.Gvms;

namespace Defra.TradeImportsDataApi.Api.Services;

public class RelatedImportDeclarationsService(
    ICustomsDeclarationRepository customsDeclarationRepository,
    IImportPreNotificationRepository importPreNotificationRepository,
    IGmrRepository gmrRepository
) : IRelatedImportDeclarationsService
{
    public async Task<(
        CustomsDeclarationEntity[] CustomsDeclarations,
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
#pragma warning disable CA1862
                // MongoDB driver does not support string.Equals()
                x => x.ClearanceRequest!.DeclarationUcr!.ToLowerInvariant() == request.Ducr.ToLowerInvariant(),
#pragma warning restore CA1862
                maxDepth,
                cancellationToken
            );
        }

        if (!string.IsNullOrEmpty(request.Mrn))
        {
            return await StartFromCustomsDeclaration(
#pragma warning disable CA1862
                // MongoDB driver does not support string.Equals()
                x => x.Id.ToLowerInvariant() == request.Mrn.ToLowerInvariant(),
#pragma warning restore CA1862
                maxDepth,
                cancellationToken
            );
        }

        if (!string.IsNullOrEmpty(request.ChedId))
        {
            return await StartFromImportPreNotification(request.ChedId, maxDepth, cancellationToken);
        }

        if (!string.IsNullOrEmpty(request.GmrId))
        {
            return await StartFromGmrId(
#pragma warning disable CA1862
                // MongoDB driver does not support string.Equals()
                x => x.Id.ToLowerInvariant() == request.GmrId.ToLowerInvariant(),
#pragma warning restore CA1862
                maxDepth,
                cancellationToken
            );
        }

        return new ValueTuple<CustomsDeclarationEntity[], ImportPreNotificationEntity[]>([], []);
    }

    private async Task<(
        CustomsDeclarationEntity[] CustomsDeclarations,
        ImportPreNotificationEntity[] ImportPreNotifications
    )> StartFromCustomsDeclaration(
        Expression<Func<CustomsDeclarationEntity, bool>> predicate,
        int maxDepth,
        CancellationToken cancellationToken
    )
    {
        var customsDeclarations = await customsDeclarationRepository.GetAll(predicate, cancellationToken);
        var identifiers = customsDeclarations.SelectMany(x => x.ImportPreNotificationIdentifiers);
        var notifications = await importPreNotificationRepository.GetAll(identifiers.ToArray(), cancellationToken);

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
        CustomsDeclarationEntity[] CustomsDeclarations,
        ImportPreNotificationEntity[] ImportPreNotifications
    )> StartFromImportPreNotification(string chedId, int maxDepth, CancellationToken cancellationToken)
    {
        var identifier = ChedReferenceRegexes.DocumentReferenceIdentifier().Match(chedId).Value;

        var notification = await importPreNotificationRepository.GetByCustomsDeclarationIdentifier(
            identifier,
            cancellationToken
        );
        if (notification == null)
        {
            return new ValueTuple<CustomsDeclarationEntity[], ImportPreNotificationEntity[]>([], []);
        }

        var customsDeclarations = await customsDeclarationRepository.GetAll(
            notification.CustomsDeclarationIdentifier,
            cancellationToken
        );

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
        CustomsDeclarationEntity[] CustomsDeclarations,
        ImportPreNotificationEntity[] ImportPreNotifications
    )> StartFromGmrId(Expression<Func<GmrEntity, bool>> predicate, int maxDepth, CancellationToken cancellationToken)
    {
        var gmr = await gmrRepository.Get(predicate, cancellationToken);
        if (gmr == null)
        {
            return new ValueTuple<CustomsDeclarationEntity[], ImportPreNotificationEntity[]>([], []);
        }

        var customsDeclarations = await customsDeclarationRepository.GetAll(
            x => gmr.CustomsDeclarationIdentifiers.Contains(x.Id),
            cancellationToken
        );
        var customsImportDeclarationIdentifiers = customsDeclarations
            .SelectMany(x => x.ImportPreNotificationIdentifiers)
            .Distinct();
        var notifications = await importPreNotificationRepository.GetAll(
            [.. customsImportDeclarationIdentifiers],
            cancellationToken
        );

        return await IncludeIndirectLinks(
            new ValueTuple<CustomsDeclarationEntity[], ImportPreNotificationEntity[]>(
                [.. customsDeclarations],
                [.. notifications]
            ),
            0,
            maxDepth,
            cancellationToken
        );
    }

    private async Task<(
        CustomsDeclarationEntity[] CustomsDeclarations,
        ImportPreNotificationEntity[] ImportPreNotifications
    )> IncludeIndirectLinks(
        (CustomsDeclarationEntity[] CustomsDeclarations, ImportPreNotificationEntity[] ImportPreNotifications) data,
        int currentDepth,
        int maxDepth,
        CancellationToken cancellationToken
    )
    {
        if (currentDepth >= maxDepth)
        {
            return data;
        }

        var customsDeclarations = data.CustomsDeclarations.ToList();
        var customsDeclarationIds = customsDeclarations.Select(x => x.Id);
        var importPreNotifications = data.ImportPreNotifications.ToList();
        var importPreNotificationIds = importPreNotifications.Select(x => x.Id);

        var identifiers = data
            .CustomsDeclarations.SelectMany(x => x.ImportPreNotificationIdentifiers)
            .Union(data.ImportPreNotifications.Select(x => x.CustomsDeclarationIdentifier))
            .Where(x => !string.IsNullOrEmpty(x))
            .Distinct()
            .ToList();

        if (identifiers.Count != 0)
        {
            importPreNotifications.AddRange(
                await importPreNotificationRepository.GetAll(
                    x =>
                        identifiers.Contains(x.CustomsDeclarationIdentifier)
                        && !importPreNotificationIds.Contains(x.Id),
                    cancellationToken
                )
            );

            customsDeclarations.AddRange(
                await customsDeclarationRepository.GetAll(
                    x =>
                        x.ImportPreNotificationIdentifiers.Any(y => identifiers.Contains(y))
                        && !customsDeclarationIds.Contains(x.Id),
                    cancellationToken
                )
            );
        }

        var response = new ValueTuple<CustomsDeclarationEntity[], ImportPreNotificationEntity[]>(
            customsDeclarations.ToArray(),
            importPreNotifications.ToArray()
        );

        // bail out of the recursive loop if there are no records loaded
        if (
            response.Item1.Length == data.CustomsDeclarations.Length
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
