using System.Collections;
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

    public IEnumerator<T> GetEnumerator() => _data.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public Type ElementType => EntityQueryable.ElementType;
    public Expression Expression => EntityQueryable.Expression;
    public IQueryProvider Provider => EntityQueryable.Provider;

    public IMongoCollection<T> Collection => throw new NotImplementedException();

    internal void AddTestData(T item) => _data.Add(item);

    public Task<T?> Find(string id, CancellationToken cancellationToken) =>
        Task.FromResult(_data.Find(x => x.Id == id));

    public Task<List<T>> FindMany(Expression<Func<T, bool>> query, CancellationToken cancellationToken) =>
        Task.FromResult(_data.FindAll(i => query.Compile()(i)));

    public void Insert(T item) => throw new NotImplementedException();

    public void Update(T item, string etag) => throw new NotImplementedException();

    public void Update(T item, Action<IFieldUpdateBuilder<T>> patch, string etag) =>
        throw new NotImplementedException();

    public Task Save(CancellationToken cancellationToken) => throw new NotImplementedException();
}
