using System.Data.Common;
using OpenConnector.Configuration;
using NCDC.ProxyServer.Commons;
using NCDC.ProxyServer.StreamMerges.Executors.Context;
using NCDC.ProxyServer.StreamMerges.Results;
using OpenConnector.StreamDataReaders;

namespace NCDC.ProxyServer.StreamMerges;

public sealed class ServerExecuteResultExecutor:AbstractStreamMergeExecutor<IExecuteResult>
{
    private ServerExecuteResultExecutor(){}
    public static ServerExecuteResultExecutor Instance = new ServerExecuteResultExecutor();
    public override IShardingMerger<IExecuteResult> GetShardingMerger()
    {
        return ServerExecuteResultShardingMerger.Instance;
    }

    protected override IExecuteResult ExecuteCommandUnit(CommandExecuteUnit commandExecuteUnit)
    {
        var connectionMode = commandExecuteUnit.ConnectionMode;
        var serverDbCommand = commandExecuteUnit.ServerDbCommand;
        var serverDbDataReader = serverDbCommand.ExecuteReader();
        var dbDataReader = serverDbDataReader.GetDbDataReader();
        var isSelect = dbDataReader.RecordsAffected<0;
        if (isSelect)
        {
            var dbColumns = dbDataReader.GetColumnSchema().ToList();
            return new QueryExecuteResult(dbColumns,
                ConnectionModeEnum.MEMORY_STRICTLY == connectionMode
                    ? new StreamQueryDataReader(dbDataReader)
                    : new MemoryQueryDataReader(dbDataReader));
        }
        else
        {
            var lastInsertId=dbDataReader.Read() ? dbDataReader.GetInt64(0) : 0L;
            return new AffectedRowsExecuteResult(dbDataReader.RecordsAffected, lastInsertId);
        }
    }
    
}