using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Services;

public interface IIuuNotificationService
{
    Task<IuuNotificationEntity?> Get(string iuuReference, CancellationToken cancellationToken);

    Task<IuuNotificationEntity> Insert(IuuNotificationEntity entity, CancellationToken cancellationToken);

    Task<IuuNotificationEntity> Update(IuuNotificationEntity entity, string etag, CancellationToken cancellationToken);
}
