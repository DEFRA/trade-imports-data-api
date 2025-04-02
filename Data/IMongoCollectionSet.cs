using System.Linq.Expressions;
using Defra.TradeImportsData.Data.Entities;

namespace Defra.TradeImportsData.Data;

public interface IMongoCollectionSet<T> : IQueryable<T> where T : IDataEntity
{
    int PendingChanges { get; }
    Task<T?> Find(string id, CancellationToken cancellationToken = default);
    Task<T?> Find(Expression<Func<T, bool>> query, CancellationToken cancellationToken = default);

    Task Insert(T item, CancellationToken cancellationToken = default);

    Task Update(T item, CancellationToken cancellationToken = default);

    Task Update(List<T> items, CancellationToken cancellationToken = default);

    Task Update(T item, string etag, CancellationToken cancellationToken = default);

    Task PersistAsync(CancellationToken cancellationToken);
}