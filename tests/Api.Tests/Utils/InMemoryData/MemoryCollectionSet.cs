using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using MongoDB.Driver;

namespace Defra.TradeImportsDataApi.Api.Tests.Utils.InMemoryData;

public class MemoryCollectionSet<T> : IMongoCollectionSet<T>
    where T : IDataEntity
{
    private readonly List<T> _data = [];

    private IQueryable<T> EntityQueryable => _data.AsQueryable();

    public IEnumerator<T> GetEnumerator()
    {
        return _data.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public Type ElementType => EntityQueryable.ElementType;
    public Expression Expression => EntityQueryable.Expression;
    public IQueryProvider Provider => EntityQueryable.Provider;

    public IMongoCollection<T> Collection => throw new NotImplementedException();

    internal void AddTestData(T item)
    {
        _data.Add(item);
    }

    public Task<T?> Find(string id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_data.Find(x => x.Id == id));
    }

    public Task<T?> Find(Expression<Func<T, bool>> query, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_data.Find(i => query.Compile()(i)));
    }

    public Task<List<T>> FindMany(Expression<Func<T, bool>> query, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_data.FindAll(i => query.Compile()(i)));
    }

    public Task Insert(T item, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    [SuppressMessage(
        "SonarLint",
        "S2955",
        Justification = "IEquatable<T> would need to be implemented on every data entity just to stop sonar complaining about a null check. Nope."
    )]
    public Task Update(T item, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task Update(List<T> items, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    [SuppressMessage(
        "SonarLint",
        "S2955",
        Justification = "IEquatable<T> would need to be implemented on every data entity just to stop sonar complaining about a null check. Nope."
    )]
    public Task Update(T item, string etag, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task Save(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
