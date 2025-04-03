using MongoDB.Driver;

namespace Defra.TradeImportsDataApi.Data.Mongo;

public class MongoDbTransaction(IClientSessionHandle session) : IMongoDbTransaction
{
    public IClientSessionHandle? Session { get; private set; } = session;

    public async Task CommitTransaction(CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(Session);
        await Session.CommitTransactionAsync(cancellationToken: cancellationToken);
        Session = null!;
    }

    public async Task RollbackTransaction(CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(Session);
        await Session.AbortTransactionAsync(cancellationToken);
        Session = null!;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing && Session != null)
        {
            Session.Dispose();
        }
    }
}
