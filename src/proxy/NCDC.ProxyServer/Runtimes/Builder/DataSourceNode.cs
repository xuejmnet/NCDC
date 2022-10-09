namespace NCDC.ProxyServer.Runtimes.Builder;

public sealed class DataSourceNode
{
    /// <summary>
    /// 数据源名称
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// 数据源链接字符串
    /// </summary>
    public string ConnectionString { get; }
    /// <summary>
    /// 是否默认数据源
    /// </summary>
    public bool IsDefault { get; }

    public DataSourceNode(string name,string connectionString,bool isDefault)
    {
        Name = name;
        ConnectionString = connectionString;
        IsDefault = isDefault;
    }
}