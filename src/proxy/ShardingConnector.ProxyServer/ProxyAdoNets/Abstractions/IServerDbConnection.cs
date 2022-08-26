using System.Data;
using System.Data.Common;

namespace ShardingConnector.ProxyServer.ProxyAdoNets.Abstractions;

public interface IServerDbConnection:IDisposable
{
    void BeginTransaction(IsolationLevel isolationLevel);
    void CommitTransaction();
    void RollbackTransaction();
    IServerDbCommand CreateCommand(string sql,ICollection<DbParameter>? dbParameters);
    DbConnection GetDbConnection();
    IServerDbTransaction? ServerDbTransaction { get; set; }
    IServerDbCommand? ServerDbCommand { get; set; }
    IServerDbDataReader? ServerDbDataReader { get; set; }
}