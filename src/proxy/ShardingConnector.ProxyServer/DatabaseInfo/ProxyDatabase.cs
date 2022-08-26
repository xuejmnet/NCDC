using ShardingConnector.NewConnector.DataSource;
using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.Commons;
using ShardingConnector.ProxyServer.ProxyAdoNets;
using ShardingConnector.ProxyServer.ProxyAdoNets.Abstractions;

namespace ShardingConnector.ProxyServer.DatabaseInfo;

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