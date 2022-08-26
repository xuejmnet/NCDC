using System.Data.Common;

namespace ShardingConnector.ProxyServer.ProxyAdoNets.Abstractions;

public interface IServerDbCommand:IDisposable
{
    DbCommand GetDbCommand();
    IServerDbDataReader ExecuteReader();
    IServerDbConnection GetServerDbConnection();
}