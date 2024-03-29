using System.Data.Common;
using NCDC.Enums;
using NCDC.ProxyServer.Executors;
using NCDC.ProxyServer.StreamMerges.Results;
using NCDC.StreamDataReaders;

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
        var dbDataReader = serverDbCommand.ExecuteReader();
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
            try
            {
                return new AffectedRowsExecuteResult(dbDataReader.RecordsAffected, lastInsertId);
            }
            finally
            {
                dbDataReader.Dispose();
            }
        }
    }
    
}