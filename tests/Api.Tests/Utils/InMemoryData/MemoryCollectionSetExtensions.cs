using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;

namespace Defra.TradeImportsDataApi.Api.Tests.Utils.InMemoryData;

public static class MemoryCollectionSetExtensions
{
    public static void AddTestData<T>(this IMongoCollectionSet<T> set, T item) where T : IDataEntity
    {
        if (set is MemoryCollectionSet<T> memoryCollectionSet)
        {
            set.AddTestData(item);
        }
    }
}