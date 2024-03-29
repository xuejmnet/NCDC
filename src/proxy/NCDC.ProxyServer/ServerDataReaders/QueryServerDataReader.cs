using NCDC.Enums;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Binaries;
using NCDC.ProxyServer.Connection.Abstractions;
using NCDC.ProxyServer.Executors;
using NCDC.ProxyServer.ServerHandlers.Results;
using NCDC.ProxyServer.StreamMerges;
using NCDC.ProxyServer.StreamMerges.Results;
using NCDC.StreamDataReaders;

namespace NCDC.ProxyServer.ServerDataReaders;

public sealed class QueryServerDataReader:AbstractExecuteServerDataReader
{

    protected IStreamDataReader StreamDataReader { get; private set; }
    protected QueryServerResult? QueryServerResult { get; private set; }
    public QueryServerDataReader(ShardingExecutionContext shardingExecutionContext, IQueryContext queryContext) : base(shardingExecutionContext, queryContext)
    {
    }

    protected override ValueTask<List<IServerDbConnection>> GetServerDbConnectionsAsync(ConnectionModeEnum connectionMode, string dataSourceName, int connectionSize)
    {
        return ConnectionSession.ServerConnection.GetConnections(connectionMode, dataSourceName, connectionSize);
    }
    public override bool Read()
    {
        return StreamDataReader!.Read();
    }

    public override BinaryRow GetRowData()
    {
        var columnCount = StreamDataReader!.ColumnCount;
        var cells = new List<BinaryCell>(columnCount);
        for (int i = 0; i < columnCount; i++)
        {
            cells.Add(new BinaryCell(StreamDataReader.GetValue(i),QueryServerResult!.DbColumns[i].DataType!));
        }
        return new BinaryRow(cells);
    }

    protected override IServerResult Merge(IExecuteResult executeResult)
    {
        
        if (executeResult is QueryExecuteResult queryExecuteResult)
        {
            StreamDataReader = queryExecuteResult.StreamDataReader;
            QueryServerResult= new QueryServerResult(queryExecuteResult.DbColumns.Select(o=>o).ToList());
            // queryExecuteResult.Dispose();
            return QueryServerResult;
        }
        else
        {
            var affectRowExecuteResult = (AffectedRowsExecuteResult)executeResult;
            return new RecordsAffectedServerResult(new List<AffectRowUnitResult>(){new AffectRowUnitResult(affectRowExecuteResult.RecordsAffected,affectRowExecuteResult.LastInsertId)});
        }
    }
    public override void Dispose()
    {
        StreamDataReader?.Dispose();
    }

}