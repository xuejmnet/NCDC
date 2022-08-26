using System.Data.Common;
using MySqlConnector;
using ShardingConnector.Executor.Context;
using ShardingConnector.Extensions;
using ShardingConnector.Pluggable.Merge;
using ShardingConnector.Pluggable.Prepare;
using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.Binaries;
using ShardingConnector.ProxyServer.ServerDataReaders;
using ShardingConnector.ProxyServer.ServerHandlers.Results;
using ShardingConnector.ProxyServer.Session;
using ShardingConnector.ProxyServer.StreamMerges.Results;
using ShardingConnector.ShardingAdoNet;
using ShardingRuntimeContext = ShardingConnector.AdoNet.AdoNet.Core.Context.ShardingRuntimeContext;

namespace ShardingConnector.ProxyServer.ServerHandlers;

public sealed class QueryServerHandler:IServerHandler
{
    public string Sql { get; }
    public ConnectionSession ConnectionSession { get; }
    public IStreamDataReader? StreamDataReader { get; private set; }
    public QueryServerResult? QueryServerResult { get; private set; }

    public QueryServerHandler(string sql,ConnectionSession connectionSession)
    {
        Sql = sql;
        ConnectionSession = connectionSession;
    }
    public IServerResult Execute()
    {
        var executionContext = Prepare(Sql);
        if (executionContext.GetExecutionUnits().IsEmpty())
        {
            return new RecordsAffectedServerResult();
        }
        var queryServerDataReader = new QueryServerDataReader(executionContext,ConnectionSession);
        var executeResult = queryServerDataReader.ExecuteDbDataReader();
        if (executeResult is QueryExecuteResult queryExecuteResult)
        {
            StreamDataReader = queryExecuteResult.StreamDataReader;
            QueryServerResult= new QueryServerResult(queryExecuteResult.DbColumns);
            return QueryServerResult;
        }
        else
        {
            var affectRowExecuteResult = (AffectRowExecuteResult)executeResult;
            return new RecordsAffectedServerResult(new List<AffectRowUnitResult>(){new AffectRowUnitResult(affectRowExecuteResult.RecordsAffected,affectRowExecuteResult.LastInsertId)});
        }
        
       
    }



    private StreamMergeContext Prepare(string sql)
    {

        ShardingRuntimeContext runtimeContext = ProxyContext.ShardingRuntimeContext;
            
        BasePrepareEngine prepareEngine = new PreparedQueryPrepareEngine(
            runtimeContext.GetRule().ToRules(), runtimeContext.GetProperties(), runtimeContext.GetMetaData(),
            runtimeContext.GetSqlParserEngine());
        var parameterContext =
            new ParameterContext(Array.Empty<DbParameter>());
            
        StreamMergeContext result = prepareEngine.Prepare(sql, parameterContext);
        //TODO
        // _commandExecutor.Init(result);
        // //_commandExecutor.Commands.for
        // _commandExecutor.Commands.ForEach(ReplyTargetMethodInvoke);
        return result;
    }

    public bool Read()
    {
        return StreamDataReader!.Read();
    }

    public BinaryRow GetRowData()
    {
        var columnCount = StreamDataReader!.ColumnCount;
        var cells = new List<BinaryCell>(columnCount);
        for (int i = 0; i < columnCount; i++)
        {
            cells.Add(new BinaryCell(StreamDataReader.GetValue(i),QueryServerResult!.DbColumns[i].DataType!));
        }
        return new BinaryRow(cells);

    }

}