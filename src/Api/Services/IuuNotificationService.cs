using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Services;

public class IuuNotificationService(IDbContext dbContext, IIuuNotificationRepository iuuNotificationRepository)
    : IIuuNotificationService
{
    public async Task<IuuNotificationEntity?> Get(string iuuReference, CancellationToken cancellationToken) =>
        await iuuNotificationRepository.Get(iuuReference, cancellationToken);

    public async Task<IuuNotificationEntity> Insert(IuuNotificationEntity entity, CancellationToken cancellationToken)
    {
        await dbContext.StartTransaction(cancellationToken);
        var inserted = iuuNotificationRepository.Insert(entity);

        await dbContext.SaveChanges(cancellationToken);
        await dbContext.CommitTransaction(cancellationToken);

        return inserted;
    }

    public async Task<IuuNotificationEntity> Update(
        IuuNotificationEntity entity,
        string etag,
        CancellationToken cancellationToken
    )
    {
        await dbContext.StartTransaction(cancellationToken);

        var (_, updated) = await iuuNotificationRepository.Update(entity, etag, cancellationToken);

        await dbContext.SaveChanges(cancellationToken);
        await dbContext.CommitTransaction(cancellationToken);

        return updated;
    }
}
