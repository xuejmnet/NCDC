using NCDC.DataSource;
using NCDC.ProxyServer.Connection.Abstractions;

namespace NCDC.ProxyServer.Connection.Metadatas;

public sealed class ProxyDatabase:ProxyServer.Connection.Metadatas.IProxyDatabase
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