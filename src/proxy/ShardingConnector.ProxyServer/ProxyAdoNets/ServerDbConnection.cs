using System.Data;
using System.Data.Common;
using ShardingConnector.Base;
using ShardingConnector.ProxyServer.ProxyAdoNets.Abstractions;

namespace ShardingConnector.ProxyServer.ProxyAdoNets;

public sealed class ServerDbConnection:IServerDbConnection
{
    private readonly DbConnection _dbConnection;
    public ServerDbConnection(DbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }
    public void BeginTransaction(IsolationLevel isolationLevel)
    {
        ServerDbTransaction = new ServerDbTransaction(this, isolationLevel);
    }


    public void CommitTransaction()
    {
        ShardingAssert.ShouldBeNotNull(ServerDbTransaction,nameof(ServerDbTransaction));
        ServerDbTransaction!.CommitTransaction();
    }

    public void RollbackTransaction()
    {
        ShardingAssert.ShouldBeNotNull(ServerDbTransaction,nameof(ServerDbTransaction));
        ServerDbTransaction!.RollbackTransaction();
    }

    public IServerDbCommand CreateCommand(string sql, ICollection<DbParameter>? dbParameters)
    {
        return new ServerDbCommand(this, sql, dbParameters);
    }

    public DbConnection GetDbConnection()
    {
        return _dbConnection;
    }

    public IServerDbTransaction? ServerDbTransaction { get; set; }
    public IServerDbCommand? ServerDbCommand { get; set; }
    public IServerDbDataReader? ServerDbDataReader { get; set; }

    public void Dispose()
    {
        ServerDbCommand?.Dispose();
        ServerDbDataReader?.Dispose();
        ServerDbTransaction?.Dispose();
        _dbConnection.Dispose();
    }
}