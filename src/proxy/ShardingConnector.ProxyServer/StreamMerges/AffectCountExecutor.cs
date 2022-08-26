namespace ShardingConnector.ProxyServer.StreamMerges;

public sealed class AffectCountExecutor:AbstractStreamMergeExecutor<int>
{
    public static AffectCountExecutor Instance { get; } = new AffectCountExecutor();
    public override IShardingMerger<int> GetShardingMerger()
    {
        return AffectCountShardingMerger.Instance;
    }

    protected override int ExecuteConnectionUnit(ConnectionExecuteUnit connectionExecuteUnit)
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
        return serverDbConnection.ExecuteNonQuery();
    }
}