using OpenConnector.ProxyServer.Abstractions;
using OpenConnector.ProxyServer.Binaries;
using OpenConnector.ProxyServer.ServerHandlers.Results;
using OpenConnector.ProxyServer.Session;
using OpenConnector.ProxyServer.StreamMerges;
using OpenConnector.ProxyServer.StreamMerges.Executors.Context;
using OpenConnector.ProxyServer.StreamMerges.Results;
using OpenConnector.StreamDataReaders;

namespace OpenConnector.ProxyServer.ServerDataReaders;

public sealed class QueryServerDataReader:AbstractAdoServerDataReader
{

    protected IStreamDataReader StreamDataReader { get; private set; }
    protected QueryServerResult? QueryServerResult { get; private set; }
    public QueryServerDataReader(ShardingExecutionContext shardingExecutionContext, ConnectionSession connectionSession) : base(shardingExecutionContext, connectionSession)
    {
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
            QueryServerResult= new QueryServerResult(queryExecuteResult.DbColumns);
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
        ConnectionSession.CloseServerConnection();
    }

}