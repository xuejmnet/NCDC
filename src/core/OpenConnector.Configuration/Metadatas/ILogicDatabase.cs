using OpenConnector.CommandParserBinder.MetaData.Schema;
using OpenConnector.ProxyServer.Session.Connection.Abstractions;
using OpenConnector.Transaction;

namespace OpenConnector.Configuration.Metadatas;

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

     List<IServerDbConnection> GetServerDbConnections(ConnectionModeEnum connectionMode, string dataSourceName,
         int connectionSize, TransactionTypeEnum transactionType);
     SchemaMetaData SchemaMetaData { get; }
}