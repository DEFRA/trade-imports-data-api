using System.Collections;
using System.Linq.Expressions;
using Defra.TradeImportsDataApi.Data.Entities;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Defra.TradeImportsDataApi.Data.Mongo;

public class MongoCollectionSet<T>(MongoDbContext dbContext, string collectionName = null!) : IMongoCollectionSet<T>
    where T : class, IDataEntity
{
    private readonly List<T> _entitiesToInsert = [];
    private readonly List<(T Item, string Etag)> _entitiesToUpdate = [];
    private readonly List<(string Id, UpdateDefinition<T> Patch, string Etag)> _entitiesToPatch = [];

    private IQueryable<T> EntityQueryable => Collection.AsQueryable();

    public IEnumerator<T> GetEnumerator() => EntityQueryable.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => EntityQueryable.GetEnumerator();

    public Type ElementType => EntityQueryable.ElementType;
    public Expression Expression => EntityQueryable.Expression;
    public IQueryProvider Provider => EntityQueryable.Provider;

    public IMongoCollection<T> Collection { get; } =
        string.IsNullOrEmpty(collectionName)
            ? dbContext.Database.GetCollection<T>(typeof(T).DataEntityName())
            : dbContext.Database.GetCollection<T>(collectionName);

    public async Task<T?> Find(string id, CancellationToken cancellationToken) =>
        await EntityQueryable.SingleOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);

    public async Task<List<T>> FindMany(Expression<Func<T, bool>> query, CancellationToken cancellationToken) =>
        await EntityQueryable.Where(query).ToListAsync(cancellationToken);

    public async Task Save(CancellationToken cancellationToken)
    {
        await Insert(cancellationToken);
        await Update(cancellationToken);
    }

    private async Task Update(CancellationToken cancellationToken)
    {
        var builder = Builders<T>.Filter;

        if (_entitiesToUpdate.Count != 0)
        {
            var transaction = GetActiveTransaction();

            foreach (var item in _entitiesToUpdate)
            {
                var filter = builder.Eq(x => x.Id, item.Item.Id) & builder.Eq(x => x.ETag, item.Etag);

                var updateResult = await Collection.ReplaceOneAsync(
                    transaction.Session,
                    filter,
                    item.Item,
                    cancellationToken: cancellationToken
                );

                if (updateResult.ModifiedCount == 0)
                    throw new ConcurrencyException(item.Item.Id, item.Etag);
            }

            _entitiesToUpdate.Clear();
        }

        if (_entitiesToPatch.Count != 0)
        {
            var transaction = GetActiveTransaction();

            foreach (var item in _entitiesToPatch)
            {
                var filter = builder.Eq(x => x.Id, item.Id) & builder.Eq(x => x.ETag, item.Etag);

                var updateResult = await Collection.UpdateOneAsync(
                    transaction.Session,
                    filter,
                    item.Patch,
                    cancellationToken: cancellationToken
                );

                if (updateResult.ModifiedCount == 0)
                    throw new ConcurrencyException(item.Id, item.Etag);
            }

            _entitiesToPatch.Clear();
        }
    }

    private async Task Insert(CancellationToken cancellationToken)
    {
        if (_entitiesToInsert.Count != 0)
        {
            var transaction = GetActiveTransaction();

            foreach (var item in _entitiesToInsert)
            {
                await Collection.InsertOneAsync(transaction.Session, item, cancellationToken: cancellationToken);
            }

            _entitiesToInsert.Clear();
        }
    }

    private MongoDbTransaction GetActiveTransaction()
    {
        if (dbContext.ActiveTransaction is null)
            throw new InvalidOperationException("Transaction has not been started");

        return dbContext.ActiveTransaction;
    }

    public void Insert(T item)
    {
        // Update in memory item now but will only be saved if Save is called
        item.Created = item.Updated = DateTime.UtcNow;
        item.ETag = BsonObjectIdGenerator.Instance.GenerateId(null, null).ToString()!;
        item.OnSave();

        _entitiesToInsert.Add(item);
    }

    public void Update(T item, string etag)
    {
        if (_entitiesToInsert.Exists(x => x.Id == item.Id))
            return;

        ArgumentNullException.ThrowIfNull(etag);

        _entitiesToUpdate.RemoveAll(x => x.Item.Id == item.Id);

        // Update in memory item now but will only be saved if Save is called
        item.Updated = DateTime.UtcNow;
        item.ETag = BsonObjectIdGenerator.Instance.GenerateId(null, null).ToString()!;
        item.OnSave();

        _entitiesToUpdate.Add(new ValueTuple<T, string>(item, etag));
    }

    public void Update(T item, Action<IFieldUpdateBuilder<T>> patch, string etag)
    {
        if (_entitiesToInsert.Exists(x => x.Id == item.Id))
            throw new InvalidOperationException("Cannot patch an entity due for insert");

        if (_entitiesToUpdate.Exists(x => x.Item.Id == item.Id))
            throw new InvalidOperationException("Cannot patch an entity due for update");

        ArgumentNullException.ThrowIfNull(etag);

        _entitiesToPatch.RemoveAll(x => x.Id == item.Id);

        var fieldUpdateBuilder = new MongoFieldUpdateBuilder<T>();
        patch(fieldUpdateBuilder);

        // Update in memory item now but will only be saved if Save is called
        item.Updated = DateTime.UtcNow;
        item.ETag = BsonObjectIdGenerator.Instance.GenerateId(null, null).ToString()!;

        _entitiesToPatch.Add(
            new ValueTuple<string, UpdateDefinition<T>, string>(
                item.Id,
                Builders<T>
                    .Update.Combine(fieldUpdateBuilder.Build())
                    // Update fields based on in memory item values
                    .Set(x => x.Updated, item.Updated)
                    .Set(x => x.ETag, item.ETag),
                etag
            )
        );
    }
}
