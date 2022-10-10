using System.Data;
using System.Data.Common;

namespace NCDC.ProxyServer.Connection.Abstractions;

public interface IServerDbConnection:IDisposable
{
    Task BeginAsync(IsolationLevel isolationLevel);
    Task CommitAsync();
    Task RollbackAsync();
    DbConnection GetDbConnection();
    DbCommand CreateCommand(string sql,ICollection<DbParameter>? dbParameters);
    bool IsBeginTransaction();
}