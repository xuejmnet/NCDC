using System.Data.Common;
using ShardingConnector.AdoNet.AdoNet.Core.Command;
using ShardingConnector.Executor.Constant;
using ShardingConnector.ShardingExecute.Execute.DataReader;

namespace ShardingConnector.ProxyServer.StreamMerges;

public sealed class StreamDataReaderExecutor:AbstractStreamMergeExecutor<IStreamDataReader>
{
    public static StreamDataReaderExecutor Instance { get; } = new StreamDataReaderExecutor();
    public override IShardingMerger<IStreamDataReader> GetShardingMerger()
    {
        return StreamDataReaderShardingMerger.Instance;
    }

    protected override IStreamDataReader ExecuteConnectionUnit(ConnectionExecuteUnit connectionExecuteUnit)
    {
        var serverDbConnection = connectionExecuteUnit.Connection;
        var executionUnit = connectionExecuteUnit.ExecutionUnit;
        var connectionMode = connectionExecuteUnit.ConnectionMode;
        var dbCommand = serverDbConnection.CreateCommand();
                 var commandText = executionUnit.GetSqlUnit().GetSql();
             
         var shardingParameters = executionUnit.GetSqlUnit().GetParameterContext().GetDbParameters();
         //TODO取消手动执行改成replay
         dbCommand.CommandText = commandText;
         foreach (var shardingParameter in shardingParameters)
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
        DbDataReader resultSet = serverDbConnection.ExecuteReader();
        if (ConnectionModeEnum.MEMORY_STRICTLY == connectionMode)
            return new StreamQueryDataReader(resultSet);
        return new MemoryQueryDataReader(resultSet);
    }
}