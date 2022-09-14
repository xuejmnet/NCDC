using System.Data.Common;
using NCDC.Enums;
using NCDC.ProxyServer.Connection.Abstractions;

namespace NCDC.ProxyServer.Connection.Metadatas;

/// <summary>
/// 逻辑数据库
/// </summary>
public interface ILogicDatabase
{
    /// <summary>
    /// 逻辑数据库名称
    /// </summary>
    string Name { get; }
    
     string DefaultDataSourceName { get; }
     string DefaultConnectionString { get;}

      bool AddConnectorUser(string username);

      bool UserNameAuthorize(string username);

      bool AddDataSource(string dataSourceName, string connectionString, DbProviderFactory dbProviderFactory,
          bool isDefault);

     List<IServerDbConnection> GetServerDbConnections(ConnectionModeEnum connectionMode, string dataSourceName,
         int connectionSize, TransactionTypeEnum transactionType);
     // SchemaMetaData SchemaMetaData { get; }
}