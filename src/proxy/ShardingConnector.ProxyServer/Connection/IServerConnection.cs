using System.Data.Common;
using ShardingConnector.Executor.Constant;
using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyServer.Connection;

public interface IServerConnection
{
    ConnectionSession ConnectionSession { get; }
    List<IServerDbConnection> GetConnections(ConnectionModeEnum connectionMode,string dataSourceName, int connectionSize);
}