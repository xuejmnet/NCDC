using OpenConnector.ProxyServer.Session.Connection.Abstractions;

namespace NCDC.Configuration.Connection.Abstractions;

public interface IServerDbTransaction:IDisposable
{
    void CommitTransaction();
    void RollbackTransaction();
    IServerDbConnection GetServerDbConnection();
}