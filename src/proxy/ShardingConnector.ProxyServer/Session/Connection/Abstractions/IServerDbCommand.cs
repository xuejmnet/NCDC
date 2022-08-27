using System.Data.Common;

namespace ShardingConnector.ProxyServer.Session.Connection.Abstractions;

public interface IServerDbCommand:IDisposable
{
    DbCommand GetDbCommand();
    IServerDbDataReader ExecuteReader();
    IServerDbConnection GetServerDbConnection();
}