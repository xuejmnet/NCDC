using System.Data;
using System.Data.Common;
using ShardingConnector.ProxyServer.Abstractions;

namespace ShardingConnector.ProxyServer.Commons;

public sealed class ServerDbConnection:IServerDbConnection
{
    private readonly DbConnection _dbConnection;

    private DbTransaction? _transaction;
    public ServerDbConnection(DbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }
    public void BeginTransaction(IsolationLevel isolationLevel)
    {
        _transaction = _dbConnection.BeginTransaction(isolationLevel);
    }

    public bool TransactionOpened => _transaction != null;

    public void CommitTransaction()
    {
        throw new NotImplementedException();
    }

    public void RollbackTransaction()
    {
        throw new NotImplementedException();
    }

    public DbCommand CreateCommand()
    {
        throw new NotImplementedException();
    }

    public DbConnection GetDbConnection()
    {
        return _dbConnection;
    }

    public DbCommand GetDbCommand()
    {
        throw new NotImplementedException();
    }

    public DbDataReader GetDbDataReader()
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _dbConnection.Dispose();
    }
}