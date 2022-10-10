using System.Data;
using System.Data.Common;
using NCDC.Base;
using NCDC.Exceptions;
using NCDC.ProxyServer.Connection.Abstractions;

namespace NCDC.ProxyServer.Connection;

public sealed class ServerDbConnection:IServerDbConnection
{
    private readonly DbConnection _dbConnection;
    private DbTransaction? _dbTransaction;
    public ServerDbConnection(DbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }
    public async ValueTask BeginAsync(IsolationLevel isolationLevel)
    {
        if (IsBeginTransaction())
        {
            throw new ShardingInvalidOperationException("transaction is already begin");
        }
        _dbTransaction =await GetDbConnection().BeginTransactionAsync(isolationLevel);
    }


    public async ValueTask CommitAsync()
    {
        if (!IsBeginTransaction())
        {
            throw new ShardingInvalidOperationException("transaction is not begin");
        }

        await _dbTransaction!.CommitAsync();
        _dbTransaction = null;
    }

    public async ValueTask RollbackAsync()
    {
        if (!IsBeginTransaction())
        {
            throw new ShardingInvalidOperationException("transaction is not begin");
        }
        await _dbTransaction!.RollbackAsync();
        _dbTransaction = null;
    }

    public DbConnection GetDbConnection()
    {
        return _dbConnection;
    }

    public DbCommand CreateCommand(string sql, ICollection<DbParameter>? dbParameters)
    {
        var dbCommand = GetDbConnection().CreateCommand();
        dbCommand.CommandText = sql;
        if (dbParameters != null)
        {
            foreach (var shardingParameter in dbParameters)
            {
                var dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = shardingParameter.ParameterName;
                dbParameter.Value = shardingParameter.Value;
                dbParameter.DbType = shardingParameter.DbType;
                dbParameter.Direction = shardingParameter.Direction;
                dbParameter.IsNullable = shardingParameter.IsNullable;
                dbParameter.SourceColumn = shardingParameter.SourceColumn;
                dbParameter.SourceColumnNullMapping = shardingParameter.SourceColumnNullMapping;
                dbParameter.Size = shardingParameter.Size;
                dbCommand.Parameters.Add(dbParameter);
            }
        }

        return dbCommand;
    }

    public bool IsBeginTransaction() => _dbTransaction != null;

    public void Dispose()
    {
        if (IsBeginTransaction())
        {
            _dbTransaction!.Dispose();
        }
        _dbConnection.Dispose();
    }
}