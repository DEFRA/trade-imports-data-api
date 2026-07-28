using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Api.Endpoints.RelatedImportDeclarations;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using Defra.TradeImportsDataApi.Domain.Ipaffs;

namespace Defra.TradeImportsDataApi.Api.Services;

public class RelatedImportDeclarationsService(
    ICustomsDeclarationRepository customsDeclarationRepository,
    IImportPreNotificationRepository importPreNotificationRepository,
    IGmrRepository gmrRepository,
    ITracesChedRepository tracesChedRepository
) : IRelatedImportDeclarationsService
{
    private readonly ValueTuple<
        CustomsDeclarationEntity[],
        ImportPreNotificationEntity[],
        GmrEntity[],
        ImportPreNotificationEntity[],
        TracesChedEntity[]
    > _empty = new ValueTuple<
        CustomsDeclarationEntity[],
        ImportPreNotificationEntity[],
        GmrEntity[],
        ImportPreNotificationEntity[],
        TracesChedEntity[]
    >([], [], [], [], []);

    public async Task<(
        CustomsDeclarationEntity[] CustomsDeclarations,
        ImportPreNotificationEntity[] ImportPreNotifications,
        GmrEntity[] Gmrs,
        ImportPreNotificationEntity[] TransientNotifications,
        TracesChedEntity[] Cheds
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

        return _empty;
    }

    [ExcludeFromCodeCoverage]
    private async Task<(
        CustomsDeclarationEntity[] CustomsDeclarations,
        ImportPreNotificationEntity[] ImportPreNotifications,
        GmrEntity[] Gmrs,
        ImportPreNotificationEntity[] TransientNotifications,
        TracesChedEntity[] Cheds
    )> StartFromCustomsDeclaration(
        Expression<Func<CustomsDeclarationEntity, bool>> predicate,
        int maxDepth,
        CancellationToken cancellationToken
    )
    {
        var customsDeclarations = await customsDeclarationRepository.GetAll(predicate, cancellationToken);

        if (customsDeclarations is null || !customsDeclarations.Any())
        {
            return _empty;
        }

        var identifiers = customsDeclarations.SelectMany(x => x.ImportPreNotificationIdentifiers).ToArray();
        var fullCheds = identifiers.Where(x => new ChedIdReference(x).IsValid()).ToArray();
        var shortCheds = identifiers.Where(x => !new ChedIdReference(x).IsValid()).ToArray();
        var notifications = await importPreNotificationRepository.GetAll(shortCheds, cancellationToken);
        var cheds = await tracesChedRepository.GetAll(fullCheds, cancellationToken);

        //put this line behind a feature flag that needs to be opt-in to
        var transientNotifications = await importPreNotificationRepository.GetAllByTags(
            customsDeclarations.Select(x => x.Id.ToLower()).ToArray(),
            cancellationToken
        );

        transientNotifications = transientNotifications
            .Where(notification => notifications.All(x => x.Id != notification.Id))
            .ToList();

        var result = await IncludeIndirectLinks(
            new ValueTuple<CustomsDeclarationEntity[], ImportPreNotificationEntity[], TracesChedEntity[]>(
                customsDeclarations.DistinctBy(x => x.Id).ToArray(),
                notifications.DistinctBy(x => x.Id).ToArray(),
                cheds.DistinctBy(x => x.Id).ToArray()
            ),
            0,
            maxDepth,
            cancellationToken
        );

        var allRelatedCustomsDeclarationIdentifiers = result.CustomsDeclarations.Select(x => x.Id);
        var gmrs = await gmrRepository.GetAll(allRelatedCustomsDeclarationIdentifiers.ToArray(), cancellationToken);

        return new ValueTuple<
            CustomsDeclarationEntity[],
            ImportPreNotificationEntity[],
            GmrEntity[],
            ImportPreNotificationEntity[],
            TracesChedEntity[]
        >(
            result.CustomsDeclarations,
            result.ImportPreNotifications,
            gmrs.ToArray(),
            transientNotifications.ToArray(),
            []
        );
    }

    private async Task<(
        CustomsDeclarationEntity[] CustomsDeclarations,
        ImportPreNotificationEntity[] ImportPreNotifications,
        GmrEntity[] Gmrs,
        ImportPreNotificationEntity[] TransientNotifications,
        TracesChedEntity[] Cheds
    )> StartFromImportPreNotification(string chedId, int maxDepth, CancellationToken cancellationToken)
    {
        var chedRef = new ChedIdReference(chedId);
        TracesChedEntity[] cheds = [];
        ImportPreNotificationEntity[] preNotifications = [];
        string cdLookup = chedId;

        if (chedRef.IsValid())
        {
            var ched = await tracesChedRepository.Get(chedId, cancellationToken);
            if (ched is not null)
            {
                cheds = [ched];
            }
        }

        if (cheds.Length == 0)
        {
            var identifier = chedRef.GetIdentifier();
            cdLookup = identifier;
            var notification = await importPreNotificationRepository.GetByCustomsDeclarationIdentifier(
                identifier,
                cancellationToken
            );

            if (notification == null)
            {
                return _empty;
            }

            preNotifications = [notification];
        }

        var customsDeclarations = await customsDeclarationRepository.GetAll(cdLookup, cancellationToken);

        var result = await IncludeIndirectLinks(
            new ValueTuple<CustomsDeclarationEntity[], ImportPreNotificationEntity[], TracesChedEntity[]>(
                customsDeclarations.DistinctBy(x => x.Id).ToArray(),
                preNotifications.DistinctBy(x => x.Id).ToArray(),
                cheds.DistinctBy(x => x.Id).ToArray()
            ),
            0,
            maxDepth,
            cancellationToken
        );

        var allRelatedCustomsDeclarationIdentifiers = result.CustomsDeclarations.Select(x => x.Id);
        var gmrs = await gmrRepository.GetAll(allRelatedCustomsDeclarationIdentifiers.ToArray(), cancellationToken);

        return new ValueTuple<
            CustomsDeclarationEntity[],
            ImportPreNotificationEntity[],
            GmrEntity[],
            ImportPreNotificationEntity[],
            TracesChedEntity[]
        >(result.CustomsDeclarations, result.ImportPreNotifications, gmrs.ToArray(), [], result.Cheds);
    }

    private async Task<(
        CustomsDeclarationEntity[] CustomsDeclarations,
        ImportPreNotificationEntity[] ImportPreNotifications,
        GmrEntity[] Gmrs,
        ImportPreNotificationEntity[] TransientNotifications,
        TracesChedEntity[] Cheds
    )> StartFromGmrId(Expression<Func<GmrEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        var gmr = await gmrRepository.Get(predicate, cancellationToken);
        if (gmr == null)
        {
            return _empty;
        }

        var customsDeclarations = await customsDeclarationRepository.GetAll(
            x => gmr.CustomsDeclarationIdentifiers.Contains(x.Id),
            cancellationToken
        );

        return new ValueTuple<
            CustomsDeclarationEntity[],
            ImportPreNotificationEntity[],
            GmrEntity[],
            ImportPreNotificationEntity[],
            TracesChedEntity[]
        >([.. customsDeclarations], [], [gmr], [], []);
    }

    private async Task<(
        CustomsDeclarationEntity[] CustomsDeclarations,
        ImportPreNotificationEntity[] ImportPreNotifications,
        GmrEntity[] Gmrs,
        ImportPreNotificationEntity[] TransientNotifications,
        TracesChedEntity[] Cheds
    )> StartFromGmrVrnOrTrn(Expression<Func<GmrEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        var gmrs = await gmrRepository.GetAll(predicate, cancellationToken);
        if (!gmrs.Any())
        {
            return _empty;
        }

        var customsDeclarationIdentifiers = gmrs.SelectMany(x => x.CustomsDeclarationIdentifiers).ToList();

        var customsDeclarations = await customsDeclarationRepository.GetAll(
            x => customsDeclarationIdentifiers.Contains(x.Id),
            cancellationToken
        );

        return new ValueTuple<
            CustomsDeclarationEntity[],
            ImportPreNotificationEntity[],
            GmrEntity[],
            ImportPreNotificationEntity[],
            TracesChedEntity[]
        >([.. customsDeclarations], [], [.. gmrs], [], []);
    }

    private async Task<(
        CustomsDeclarationEntity[] CustomsDeclarations,
        ImportPreNotificationEntity[] ImportPreNotifications,
        TracesChedEntity[] Cheds
    )> IncludeIndirectLinks(
        (
            CustomsDeclarationEntity[] CustomsDeclarations,
            ImportPreNotificationEntity[] ImportPreNotifications,
            TracesChedEntity[] Cheds
        ) data,
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
        var cheds = data.Cheds.ToList();

        var identifiers = data
            .CustomsDeclarations.SelectMany(x => x.ImportPreNotificationIdentifiers)
            .Union(data.ImportPreNotifications.Select(x => x.CustomsDeclarationIdentifier))
            .Where(x => !string.IsNullOrEmpty(x))
            .Distinct()
            .ToList();

        var fullCheds = identifiers.Where(x => new ChedIdReference(x).IsValid()).ToList();
        var shortCheds = identifiers.Where(x => !new ChedIdReference(x).IsValid()).ToList();

        if (fullCheds.Count != 0)
        {
            var foundCheds = await tracesChedRepository.GetAll(fullCheds.ToArray(), cancellationToken);

            shortCheds.AddRange(
                foundCheds.Where(x => !fullCheds.Contains(x.Id)).Select(x => new ChedIdReference(x.Id).GetIdentifier())
            );

            cheds.AddRange(foundCheds);

            customsDeclarations.AddRange(
                await customsDeclarationRepository.GetAll(
                    x =>
                        x.ImportPreNotificationIdentifiers.Any(y => identifiers.Contains(y))
                        && !customsDeclarationIds.Contains(x.Id),
                    cancellationToken
                )
            );
        }

        if (shortCheds.Count != 0)
        {
            importPreNotifications.AddRange(
                await importPreNotificationRepository.GetAll(
                    x =>
                        identifiers.Contains(x.CustomsDeclarationIdentifier)
                        && !importPreNotificationIds.Contains(x.Id),
                    cancellationToken
                )
            );

            importPreNotifications.RemoveAll(x =>
                cheds.Exists(ched => ched.Id == x.ImportPreNotification.ReferenceNumber)
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

        var response = new ValueTuple<CustomsDeclarationEntity[], ImportPreNotificationEntity[], TracesChedEntity[]>(
            customsDeclarations.DistinctBy(x => x.Id).ToArray(),
            importPreNotifications.DistinctBy(x => x.Id).ToArray(),
            cheds.DistinctBy(x => x.Id).ToArray()
        );

        // bail out of the recursive loop if there are no records loaded
        if (
            response.Item1.Length == data.CustomsDeclarations.Length
            && response.Item2.Length == data.ImportPreNotifications.Length
            && response.Item3.Length == data.Cheds.Length
        )
        {
            return response;
        }

        return await IncludeIndirectLinks(
            new ValueTuple<CustomsDeclarationEntity[], ImportPreNotificationEntity[], TracesChedEntity[]>(
                customsDeclarations.DistinctBy(x => x.Id).ToArray(),
                importPreNotifications.DistinctBy(x => x.Id).ToArray(),
                cheds.DistinctBy(x => x.Id).ToArray()
            ),
            currentDepth + 1,
            maxDepth,
            cancellationToken
        );
    }
}
