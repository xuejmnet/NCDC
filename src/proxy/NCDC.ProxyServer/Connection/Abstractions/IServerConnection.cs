using NCDC.Enums;

namespace NCDC.ProxyServer.Connection.Abstractions;

public interface IServerConnection : IAdoMethodReplier, IDisposable
{
    IDictionary<string /*data source*/, List<IServerDbConnection>> CachedConnections { get; }
    IConnectionSession ConnectionSession { get; }

    ValueTask<List<IServerDbConnection>> GetConnections(ConnectionModeEnum connectionMode, string dataSourceName,
        int connectionSize);

    ValueTask<LinkedList<Exception>> ReleaseConnectionsAsync(bool forceRollback);
    // void CloseCurrentCommandReader();

    IServerDbConnection GetServerDbConnection(CreateServerDbConnectionStrategyEnum strategy, string dataSourceName);
    void Reset();
}