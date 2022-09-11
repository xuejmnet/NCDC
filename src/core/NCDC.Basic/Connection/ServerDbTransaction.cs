using System.Data;
using System.Data.Common;
using NCDC.Basic.Connection.Abstractions;

namespace NCDC.Basic.Connection;

public sealed class ServerDbTransaction:IServerDbTransaction
{
    private readonly IServerDbConnection _serverDbConnection;
    private readonly DbTransaction _dbTransaction;

    public ServerDbTransaction(IServerDbConnection serverDbConnection,IsolationLevel isolationLevel)
    {
        _serverDbConnection = serverDbConnection;
        _dbTransaction = serverDbConnection.GetDbConnection().BeginTransaction(isolationLevel);
    }
    public void Dispose()
    {
        _dbTransaction.Dispose();
        _serverDbConnection.ServerDbTransaction = null;
    }

    public void CommitTransaction()
    {
        _dbTransaction.Commit();
    }

    public void RollbackTransaction()
    {
        _dbTransaction.Rollback();
    }

    public IServerDbConnection GetServerDbConnection()
    {
        return _serverDbConnection;
    }
}