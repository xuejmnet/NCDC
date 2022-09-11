using NCDC.Basic.Connection;
using NCDC.Basic.Connection.Abstractions;
using OpenConnector.DataSource;

namespace NCDC.Basic.Metadatas;

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