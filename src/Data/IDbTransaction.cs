namespace Defra.TradeImportsDataApi.Data;

public interface IDbTransaction : IDisposable
{
    Task Commit(CancellationToken cancellationToken);
}
