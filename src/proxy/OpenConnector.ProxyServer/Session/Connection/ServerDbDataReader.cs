using System.Data.Common;
using OpenConnector.ProxyServer.Session.Connection.Abstractions;

namespace OpenConnector.ProxyServer.Session.Connection;

public sealed class ServerDbDataReader:IServerDbDataReader
{
    private readonly IServerDbConnection _serverDbConnection;
    private readonly DbDataReader _dataReader;

    public ServerDbDataReader(IServerDbConnection serverDbConnection,DbDataReader dataReader)
    {
        _serverDbConnection = serverDbConnection;
        _dataReader = dataReader;
    }
    public void Dispose()
    {
        _dataReader.Dispose();
        _serverDbConnection.ServerDbDataReader = null;
    }

    public DbDataReader GetDbDataReader()
    {
        return _dataReader;
    }

    public IServerDbConnection GetServerDbConnection()
    {
        return _serverDbConnection;
    }
}