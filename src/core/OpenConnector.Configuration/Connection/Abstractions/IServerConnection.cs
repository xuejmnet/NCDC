using OpenConnector.ProxyServer.Session.Connection.Abstractions;

namespace OpenConnector.Configuration.Connection.Abstractions;

public interface IServerConnection:IDisposable
{
    IConnectionSession ConnectionSession { get; }
    List<IServerDbConnection> GetConnections(ConnectionModeEnum connectionMode,string dataSourceName, int connectionSize);
    void CloseCurrentCommandReader();
}