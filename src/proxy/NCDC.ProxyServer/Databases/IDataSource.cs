using System.Data.Common;
using NCDC.ProxyServer.Connection.Abstractions;

namespace NCDC.ProxyServer.Databases;

public interface IDataSource
{
    /// <summary>
    /// data source name
    /// </summary>
    string DataSourceName { get; }
    /// <summary>
    /// 数据源链接
    /// </summary>
    string ConnectionString { get; }
    /// <summary>
    /// 是否是默认的数据源
    /// </summary>
    bool IsDefault { get; }

    DbConnection CreateDbConnection();
    IServerDbConnection CreateServerDbConnection();
}