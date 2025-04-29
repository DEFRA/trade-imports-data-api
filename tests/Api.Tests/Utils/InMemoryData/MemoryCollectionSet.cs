using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Defra.TradeImportsDataApi.Api.Tests.Utils.InMemoryData;

public class MemoryCollectionSet<T> : IMongoCollectionSet<T>
    where T : IDataEntity
{
    private readonly List<T> data = [];

    private IQueryable<T> EntityQueryable => data.AsQueryable();

    public IEnumerator<T> GetEnumerator()
    {
        return data.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public Type ElementType => EntityQueryable.ElementType;
    public Expression Expression => EntityQueryable.Expression;
    public IQueryProvider Provider => EntityQueryable.Provider;

    public int PendingChanges => 0;

    public Task<T?> Find(string id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(data.Find(x => x.Id == id));
    }

    public Task<T?> Find(Expression<Func<T, bool>> query, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(data.Find(i => query.Compile()(i)));
    }

    public Task Insert(T item, CancellationToken cancellationToken = default)
    {
        item.ETag = BsonObjectIdGenerator.Instance.GenerateId(null, null).ToString()!;
        data.Add(item);
        return Task.CompletedTask;
    }

    [SuppressMessage(
        "SonarLint",
        "S2955",
        Justification = "IEquatable<T> would need to be implemented on every data entity just to stop sonar complaining about a null check. Nope."
    )]
    public Task Update(T item, CancellationToken cancellationToken = default)
    {
        return Update(item, item.ETag, cancellationToken);
    }

    public async Task Update(List<T> items, CancellationToken cancellationToken = default)
    {
        foreach (var item in items)
        {
            await Update(item, cancellationToken);
        }
    }

    [SuppressMessage(
        "SonarLint",
        "S2955",
        Justification = "IEquatable<T> would need to be implemented on every data entity just to stop sonar complaining about a null check. Nope."
    )]
    public Task Update(T item, string etag, CancellationToken cancellationToken = default)
    {
        etag = etag ?? item.ETag;

        var existingItem = data.Find(x => x.Id == item.Id);
        if (existingItem == null)
            return Task.CompletedTask;

        if ((existingItem.ETag) != etag)
        {
            throw new ConcurrencyException(item.Id!, etag);
        }

        item.ETag = BsonObjectIdGenerator.Instance.GenerateId(null, null).ToString()!;
        data[data.IndexOf(existingItem)] = item;
        return Task.CompletedTask;
    }

    public Task PersistAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
