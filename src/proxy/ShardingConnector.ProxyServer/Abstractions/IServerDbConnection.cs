using System.Data;
using System.Data.Common;

namespace ShardingConnector.ProxyServer.Abstractions;

public interface IServerDbConnection:IDisposable
{
    void BeginTransaction(IsolationLevel isolationLevel);
    void CommitTransaction();
    void RollbackTransaction();
    DbCommand CreateCommand();
    DbConnection GetDbConnection();
    DbCommand GetDbCommand();
    DbDataReader GetDbDataReader();
}