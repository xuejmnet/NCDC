using System.Data;
using System.Data.Common;

namespace NCDC.ProxyServer.Connection.Abstractions;

public interface IServerDbConnection:IDisposable
{
    ValueTask BeginAsync(IsolationLevel isolationLevel);
    ValueTask CommitAsync();
    ValueTask RollbackAsync();
    DbConnection GetDbConnection();
    DbCommand CreateCommand(string sql,ICollection<DbParameter>? dbParameters);
    bool IsBeginTransaction();
}