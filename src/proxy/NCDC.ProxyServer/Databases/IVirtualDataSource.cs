using NCDC.Basic.Configurations;
using NCDC.Enums;
using NCDC.ProxyServer.Connection.Abstractions;

namespace NCDC.ProxyServer.Databases;

public interface IVirtualDataSource
{
    string GetDatabaseName();
    /// <summary>
    /// 默认的数据源名称
    /// </summary>
    string DefaultDataSourceName { get; }
    /// <summary>
    /// 默认连接字符串
    /// </summary>
    string DefaultConnectionString { get;}

    string GetConnectionString(string dataSourceName);
    IReadOnlyCollection<string> GetAllDataSourceNames();
    bool IsDefault(string dataSourceName);
    bool AddDataSource(string dataSourceName, string connectionString);
    bool Exists(string dataSourceName);
    IDataSource GetDataSource(string dataSourceName);

    List<IServerDbConnection> GetServerDbConnections(ConnectionModeEnum connectionMode, string dataSourceName,
        int connectionSize, TransactionTypeEnum transactionType);
}