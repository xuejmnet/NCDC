using NCDC.Enums;

namespace NCDC.ProxyServer.Connection.Abstractions;

public interface IServerConnection:IDisposable
{
    IConnectionSession ConnectionSession { get; }
    List<IServerDbConnection> GetConnections(ConnectionModeEnum connectionMode,string dataSourceName, int connectionSize);
    void CloseCurrentCommandReader();
}