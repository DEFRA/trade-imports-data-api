using MongoDB.Driver;

namespace Defra.TradeImportsDataApi.Data.Mongo;

public class MongoDbTransaction(IClientSessionHandle session) : IDbTransaction
{
    public IClientSessionHandle? Session { get; private set; } = session;

    public async Task Commit(CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(Session);

        await Session.CommitTransactionAsync(cancellationToken: cancellationToken);

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
