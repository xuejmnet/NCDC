using System.Data.Common;
using NCDC.Basic.Connection.Abstractions;
using OpenConnector.Base;

namespace NCDC.Basic.Connection;

public sealed class ServerDbCommand:IServerDbCommand
{
    private readonly IServerDbConnection _serverDbConnection;
    private readonly DbCommand _dbCommand;

    public ServerDbCommand(IServerDbConnection serverDbConnection,string sql,ICollection<DbParameter>? dbParameters)
    {
        _serverDbConnection = serverDbConnection;
        _dbCommand = serverDbConnection.GetDbConnection().CreateCommand();
        _dbCommand.CommandText = sql;
        if (dbParameters != null)
        {
            foreach (var shardingParameter in dbParameters)
            {
                var dbParameter = _dbCommand.CreateParameter();
                dbParameter.ParameterName = shardingParameter.ParameterName;
                dbParameter.Value = shardingParameter.Value;
                dbParameter.DbType = shardingParameter.DbType;
                dbParameter.Direction = shardingParameter.Direction;
                dbParameter.IsNullable = shardingParameter.IsNullable;
                dbParameter.SourceColumn = shardingParameter.SourceColumn;
                dbParameter.SourceColumnNullMapping = shardingParameter.SourceColumnNullMapping;
                dbParameter.Size = shardingParameter.Size;
                _dbCommand.Parameters.Add(dbParameter);
            }
        }
    }
    public void Dispose()
    {
        _dbCommand.Dispose();
        _serverDbConnection.ServerDbCommand = null;
    }

    public DbCommand GetDbCommand()
    {
        return _dbCommand;
    }

    public IServerDbDataReader ExecuteReader()
    {
        ShardingAssert.ShouldBeNull( _serverDbConnection.ServerDbDataReader,nameof( _serverDbConnection.ServerDbDataReader));
        var dbDataReader = _dbCommand.ExecuteReader();
        var serverDbDataReader = new ServerDbDataReader(_serverDbConnection,dbDataReader);
        _serverDbConnection.ServerDbDataReader = serverDbDataReader;
        return serverDbDataReader;
    }

    public IServerDbConnection GetServerDbConnection()
    {
        return _serverDbConnection;
    }
}