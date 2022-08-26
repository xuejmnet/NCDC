using System.Data;

namespace ShardingConnector.ProxyServer.ProxyAdoNets.Abstractions;

public interface IServerDbTransaction:IDisposable
{
    void CommitTransaction();
    void RollbackTransaction();
    IServerDbConnection GetServerDbConnection();
}