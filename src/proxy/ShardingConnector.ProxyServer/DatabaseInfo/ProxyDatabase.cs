namespace ShardingConnector.ProxyServer.DatabaseInfo;

public sealed class ProxyDatabase
{
    /// <summary>
    /// 数据源名称
    /// </summary>
    public string DataSourceName { get; }

    /// <summary>
    /// 链接字符串
    /// </summary>
    public string ConnectionString { get; }

    /// <summary>
    /// 是否是默认数据源
    /// </summary>
    public bool IsDefault { get; }

    public ProxyDatabase(string dataSourceName, string connectionString, bool isDefault = false)
    {
        DataSourceName = dataSourceName;
        ConnectionString = connectionString;
        IsDefault = isDefault;
    }
}