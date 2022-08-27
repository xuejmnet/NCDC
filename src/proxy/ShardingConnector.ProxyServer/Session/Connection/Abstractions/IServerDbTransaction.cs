namespace ShardingConnector.ProxyServer.Session.Connection.Abstractions;

public interface IServerDbTransaction:IDisposable
{
    void CommitTransaction();
    void RollbackTransaction();
    IServerDbConnection GetServerDbConnection();
}