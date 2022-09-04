using System.Data.Common;

namespace OpenConnector.ProxyServer.Session.Connection.Abstractions;

public interface IServerDbCommand:IDisposable
{
    DbCommand GetDbCommand();
    IServerDbDataReader ExecuteReader();
    IServerDbConnection GetServerDbConnection();
}