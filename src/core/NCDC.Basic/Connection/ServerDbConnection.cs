using System.Data;
using System.Data.Common;
using NCDC.Basic.Connection.Abstractions;
using NCDC.Base;

namespace NCDC.Basic.Connection;

public sealed class ServerDbConnection:IServerDbConnection
{
    private readonly DbConnection _dbConnection;
    public ServerDbConnection(DbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }
    public void BeginTransaction(IsolationLevel isolationLevel)
    {
        ShardingAssert.ShouldBeNull(ServerDbTransaction,nameof(ServerDbTransaction));
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
        ShardingAssert.ShouldBeNull(ServerDbCommand,nameof(ServerDbCommand));
        ServerDbCommand= new ServerDbCommand(this, sql, dbParameters);
        return ServerDbCommand;
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