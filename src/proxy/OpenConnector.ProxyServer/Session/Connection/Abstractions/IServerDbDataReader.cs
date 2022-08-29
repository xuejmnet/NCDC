using System.Data.Common;

namespace OpenConnector.ProxyServer.Session.Connection.Abstractions;

public interface IServerDbDataReader:IDisposable
{
    DbDataReader GetDbDataReader();
    IServerDbConnection GetServerDbConnection();
}