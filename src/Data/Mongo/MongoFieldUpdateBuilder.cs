using System.Linq.Expressions;
using MongoDB.Driver;

namespace Defra.TradeImportsDataApi.Data.Mongo;

public class MongoFieldUpdateBuilder<T> : IFieldUpdateBuilder<T>
{
    private readonly List<UpdateDefinition<T>> _updates = [];

    public IFieldUpdateBuilder<T> Set<TField>(Expression<Func<T, TField>> field, TField value)
    {
        _updates.Add(Builders<T>.Update.Set(field, value));

        return this;
    }

    public UpdateDefinition<T> Build() => Builders<T>.Update.Combine(_updates);
}
