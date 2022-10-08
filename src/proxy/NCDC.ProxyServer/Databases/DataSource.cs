using System.Data.Common;
using NCDC.ProxyServer.Connection;
using NCDC.ProxyServer.Connection.Abstractions;

namespace NCDC.ProxyServer.Databases;


public sealed class DataSource:IDataSource
{
    private readonly DbProviderFactory _dbProviderFactory;

    public DataSource(string dataSourceName, string connectionString, bool isDefault,DbProviderFactory dbProviderFactory)
    {
        _dbProviderFactory = dbProviderFactory;
        DataSourceName = dataSourceName;
        ConnectionString = connectionString;
        IsDefault = isDefault;
    }

    public string DataSourceName { get; }
    public string ConnectionString { get; }
    public bool IsDefault { get; }
    public DbConnection CreateDbConnection()
    {
        var dbConnection = _dbProviderFactory.CreateConnection();
        dbConnection!.ConnectionString = ConnectionString;
        dbConnection.Open();
        return dbConnection;
    }

    public IServerDbConnection CreateServerDbConnection()
    {
        return new ServerDbConnection(CreateDbConnection());
    }
}