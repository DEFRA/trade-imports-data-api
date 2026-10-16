using Defra.TradeImportsDataApi.Api.Exceptions;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Data;

public class IuuNotificationRepository(IDbContext dbContext) : IIuuNotificationRepository
{
    public async Task<IuuNotificationEntity?> Get(string id, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(id))
            return null;

        return await dbContext.IuuNotifications.Find(id, cancellationToken);
    }

    public IuuNotificationEntity Insert(IuuNotificationEntity entity)
    {
        dbContext.IuuNotifications.Insert(entity);

        return entity;
    }

    public async Task<(IuuNotificationEntity Existing, IuuNotificationEntity Updated)> Update(
        IuuNotificationEntity entity,
        string etag,
        CancellationToken cancellationToken
    )
    {
        var existing = await dbContext.IuuNotifications.Find(entity.Id, cancellationToken);
        if (existing == null)
        {
            throw new EntityNotFoundException(nameof(IuuNotificationEntity), entity.Id);
        }

        entity.Created = existing.Created;

        dbContext.IuuNotifications.Update(entity, etag);

        return (existing, entity);
    }
}
