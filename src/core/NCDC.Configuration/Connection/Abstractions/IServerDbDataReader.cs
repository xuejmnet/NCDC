using System.Data.Common;
using OpenConnector.ProxyServer.Session.Connection.Abstractions;

namespace NCDC.Configuration.Connection.Abstractions;

public interface IServerDbDataReader:IDisposable
{
    DbDataReader GetDbDataReader();
    IServerDbConnection GetServerDbConnection();
}