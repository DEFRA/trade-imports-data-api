using System.Linq.Expressions;

namespace Defra.TradeImportsDataApi.Data;

public interface IFieldUpdateBuilder<T>
{
    IFieldUpdateBuilder<T> Set<TField>(Expression<Func<T, TField>> field, TField value);
}
