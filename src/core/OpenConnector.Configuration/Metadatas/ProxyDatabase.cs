using OpenConnector.DataSource;
using OpenConnector.ProxyServer.Session.Connection;
using OpenConnector.ProxyServer.Session.Connection.Abstractions;

namespace OpenConnector.Configuration.Metadatas;

public sealed class ProxyDatabase:IProxyDatabase
{
    private readonly IDataSource _dataSource;
    public ProxyDatabase(IDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public string DataSourceName =>_dataSource.DataSourceName;
    public bool IsDefault => _dataSource.IsDefault();

    public IServerDbConnection CreateServerDbConnection()
    {
        var dbConnection = _dataSource.CreateConnection();
        dbConnection.Open();
        return new ServerDbConnection(dbConnection);
    }
}