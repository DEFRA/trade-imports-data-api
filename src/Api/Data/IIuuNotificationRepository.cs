using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Data;

public interface IIuuNotificationRepository
{
    Task<IuuNotificationEntity?> Get(string id, CancellationToken cancellationToken);

    IuuNotificationEntity Insert(IuuNotificationEntity entity);

    Task<(IuuNotificationEntity Existing, IuuNotificationEntity Updated)> Update(
        IuuNotificationEntity entity,
        string etag,
        CancellationToken cancellationToken
    );
}
