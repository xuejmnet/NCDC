using System.Data.Common;

namespace ShardingConnector.ProxyServer.ProxyAdoNets.Abstractions;

public interface IServerDbDataReader:IDisposable
{
    DbDataReader GetDbDataReader();
    IServerDbConnection GetServerDbConnection();
}