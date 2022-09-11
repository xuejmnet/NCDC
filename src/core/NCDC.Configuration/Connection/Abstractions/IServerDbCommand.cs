using System.Data.Common;
using OpenConnector.ProxyServer.Session.Connection.Abstractions;

namespace NCDC.Configuration.Connection.Abstractions;

public interface IServerDbCommand:IDisposable
{
    DbCommand GetDbCommand();
    IServerDbDataReader ExecuteReader();
    IServerDbConnection GetServerDbConnection();
}