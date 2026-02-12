using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Api.Endpoints.RelatedImportDeclarations;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;

namespace Defra.TradeImportsDataApi.Api.Services;

public class RelatedImportDeclarationsService(
    ICustomsDeclarationRepository customsDeclarationRepository,
    IImportPreNotificationRepository importPreNotificationRepository,
    IGmrRepository gmrRepository
) : IRelatedImportDeclarationsService
{
    public async Task<(
        CustomsDeclarationEntity[] CustomsDeclarations,
        ImportPreNotificationEntity[] ImportPreNotifications,
        GmrEntity[] Gmrs
    )> Search(RelatedImportDeclarationsRequest request, CancellationToken cancellationToken)
    {
        var maxDepth = 3;
        if (request.MaxLinkDepth.HasValue)
        {
            maxDepth = request.MaxLinkDepth.Value;
        }

        if (!string.IsNullOrEmpty(request.Ducr))
        {
            var search = request.Ducr.ToLower();
            return await StartFromCustomsDeclaration(x => x.Tags.Contains(search), maxDepth, cancellationToken);
        }

        if (!string.IsNullOrEmpty(request.Mrn))
        {
            var search = request.Mrn.ToLower();
            return await StartFromCustomsDeclaration(x => x.Tags.Contains(search), maxDepth, cancellationToken);
        }

        if (!string.IsNullOrEmpty(request.ChedId))
        {
            return await StartFromImportPreNotification(request.ChedId, maxDepth, cancellationToken);
        }

        if (!string.IsNullOrEmpty(request.GmrId))
        {
            var search = request.GmrId.ToLower();
            return await StartFromGmrId(x => x.Tags.Contains(search), cancellationToken);
        }

        if (!string.IsNullOrEmpty(request.VrnOrTrn))
        {
            var search = request.VrnOrTrn.ToLower();
            return await StartFromGmrVrnOrTrn(x => x.Tags.Contains(search), cancellationToken);
        }

        return new ValueTuple<CustomsDeclarationEntity[], ImportPreNotificationEntity[], GmrEntity[]>([], [], []);
    }

    [ExcludeFromCodeCoverage]
    private async Task<(
        CustomsDeclarationEntity[] CustomsDeclarations,
        ImportPreNotificationEntity[] ImportPreNotifications,
        GmrEntity[] Gmrs
    )> StartFromCustomsDeclaration(
        Expression<Func<CustomsDeclarationEntity, bool>> predicate,
        int maxDepth,
        CancellationToken cancellationToken
    )
    {
        var customsDeclarations = await customsDeclarationRepository.GetAll(predicate, cancellationToken);

        if (customsDeclarations is null || !customsDeclarations.Any())
        {
            return new ValueTuple<CustomsDeclarationEntity[], ImportPreNotificationEntity[], GmrEntity[]>([], [], []);
        }

        var identifiers = customsDeclarations.SelectMany(x => x.ImportPreNotificationIdentifiers);
        var notifications = await importPreNotificationRepository.GetAll(identifiers.ToArray(), cancellationToken);

        var result = await IncludeIndirectLinks(
            new ValueTuple<CustomsDeclarationEntity[], ImportPreNotificationEntity[]>(
                customsDeclarations.ToArray(),
                notifications.ToArray()
            ),
            0,
            maxDepth,
            cancellationToken
        );

        var allRelatedCustomsDeclarationIdentifiers = result.CustomsDeclarations.Select(x => x.Id);
        var gmrs = await gmrRepository.GetAll(allRelatedCustomsDeclarationIdentifiers.ToArray(), cancellationToken);

        return new ValueTuple<CustomsDeclarationEntity[], ImportPreNotificationEntity[], GmrEntity[]>(
            result.CustomsDeclarations,
            result.ImportPreNotifications,
            gmrs.ToArray()
        );
    }

    private async Task<(
        CustomsDeclarationEntity[] CustomsDeclarations,
        ImportPreNotificationEntity[] ImportPreNotifications,
        GmrEntity[] Gmrs
    )> StartFromImportPreNotification(string chedId, int maxDepth, CancellationToken cancellationToken)
    {
        var identifier = ChedReferenceRegexes.DocumentReferenceIdentifier().Match(chedId).Value;

        var notification = await importPreNotificationRepository.GetByCustomsDeclarationIdentifier(
            identifier,
            cancellationToken
        );
        if (notification == null)
        {
            return new ValueTuple<CustomsDeclarationEntity[], ImportPreNotificationEntity[], GmrEntity[]>([], [], []);
        }

        var customsDeclarations = await customsDeclarationRepository.GetAll(
            notification.CustomsDeclarationIdentifier,
            cancellationToken
        );

        var result = await IncludeIndirectLinks(
            new ValueTuple<CustomsDeclarationEntity[], ImportPreNotificationEntity[]>(
                customsDeclarations.ToArray(),
                [notification]
            ),
            0,
            maxDepth,
            cancellationToken
        );

        var allRelatedCustomsDeclarationIdentifiers = result.CustomsDeclarations.Select(x => x.Id);
        var gmrs = await gmrRepository.GetAll(allRelatedCustomsDeclarationIdentifiers.ToArray(), cancellationToken);

        return new ValueTuple<CustomsDeclarationEntity[], ImportPreNotificationEntity[], GmrEntity[]>(
            result.CustomsDeclarations,
            result.ImportPreNotifications,
            gmrs.ToArray()
        );
    }

    private async Task<(
        CustomsDeclarationEntity[] CustomsDeclarations,
        ImportPreNotificationEntity[] ImportPreNotifications,
        GmrEntity[] Gmrs
    )> StartFromGmrId(Expression<Func<GmrEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        var gmr = await gmrRepository.Get(predicate, cancellationToken);
        if (gmr == null)
        {
            return new ValueTuple<CustomsDeclarationEntity[], ImportPreNotificationEntity[], GmrEntity[]>([], [], []);
        }

        var customsDeclarations = await customsDeclarationRepository.GetAll(
            x => gmr.CustomsDeclarationIdentifiers.Contains(x.Id),
            cancellationToken
        );

        return new ValueTuple<CustomsDeclarationEntity[], ImportPreNotificationEntity[], GmrEntity[]>(
            [.. customsDeclarations],
            [],
            [gmr]
        );
    }

    private async Task<(
        CustomsDeclarationEntity[] CustomsDeclarations,
        ImportPreNotificationEntity[] ImportPreNotifications,
        GmrEntity[] Gmrs
    )> StartFromGmrVrnOrTrn(Expression<Func<GmrEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        var gmrs = await gmrRepository.GetAll(predicate, cancellationToken);
        if (!gmrs.Any())
        {
            return new ValueTuple<CustomsDeclarationEntity[], ImportPreNotificationEntity[], GmrEntity[]>([], [], []);
        }

        var customsDeclarationIdentifiers = gmrs.SelectMany(x => x.CustomsDeclarationIdentifiers).ToList();

        var customsDeclarations = await customsDeclarationRepository.GetAll(
            x => customsDeclarationIdentifiers.Contains(x.Id),
            cancellationToken
        );

        return new ValueTuple<CustomsDeclarationEntity[], ImportPreNotificationEntity[], GmrEntity[]>(
            [.. customsDeclarations],
            [],
            [.. gmrs]
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
