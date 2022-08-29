using OpenConnector.ProxyServer.Commons;

namespace OpenConnector.ProxyServer.Session.Connection.Abstractions;

public interface IServerConnection
{
    ConnectionSession ConnectionSession { get; }
    List<IServerDbConnection> GetConnections(ConnectionModeEnum connectionMode,string dataSourceName, int connectionSize);
}