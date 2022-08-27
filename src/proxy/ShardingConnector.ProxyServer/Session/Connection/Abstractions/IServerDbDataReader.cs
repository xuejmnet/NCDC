using System.Data.Common;

namespace ShardingConnector.ProxyServer.Session.Connection.Abstractions;

public interface IServerDbDataReader:IDisposable
{
    DbDataReader GetDbDataReader();
    IServerDbConnection GetServerDbConnection();
}