using System.Data;
using System.Data.Common;
using ShardingConnector.ProxyServer.Abstractions;

namespace ShardingConnector.ProxyServer.Commons;

public sealed class ServerDbConnection:IServerDbConnection
{
    private readonly DbConnection _dbConnection;

    private DbTransaction? _transaction;
    private DbCommand? _dbCommand;
    private DbDataReader? _dbDataReader;
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
        _dbCommand= _dbConnection.CreateCommand();
        return _dbCommand;
    }

    public DbConnection GetDbConnection()
    {
        return _dbConnection;
    }

    public DbCommand? GetDbCommand()
    {
        return _dbCommand;
    }

    public DbDataReader ExecuteReader()
    {
        _dbDataReader= _dbCommand!.ExecuteReader();
        return _dbDataReader;
    }

    public DbDataReader? GetDbDataReader()
    {
        return _dbDataReader;
    }

    public int ExecuteNonQuery()
    {
       return _dbCommand!.ExecuteNonQuery();
    }

    public void Dispose()
    {
        _dbDataReader?.Dispose();
        _dbCommand?.Dispose();
        _transaction?.Dispose();
        _dbConnection.Dispose();
    }
}