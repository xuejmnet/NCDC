using System.Data.Common;
using MySqlConnector;
using ShardingConnector.AdoNet.Executor;
using ShardingConnector.AdoNet.Executor.Abstractions;
using ShardingConnector.Executor.Context;
using ShardingConnector.Extensions;
using ShardingConnector.Pluggable.Merge;
using ShardingConnector.Pluggable.Prepare;
using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.Binaries;
using ShardingConnector.ProxyServer.ServerDataReaders;
using ShardingConnector.ProxyServer.ServerHandlers.Results;
using ShardingConnector.ProxyServer.Session;
using ShardingConnector.ShardingAdoNet;
using ShardingDataReader = ShardingConnector.AdoNet.AdoNet.Core.DataReader.ShardingDataReader;
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
            return new AffectRowServerResult();
        }
        var queryServerDataReader = new QueryServerDataReader(executionContext,ConnectionSession);
        if (executionContext.IsSelect)
        {
            StreamDataReader = queryServerDataReader.ExecuteDbDataReader();
            // StreamDataReader = MergeQuery(executionContext,dataReaders);
            var dbDataReaders = ConnectionSession.ServerConnection.CachedConnections.SelectMany(o=>o.Value.Select(x=>x.GetDbDataReader())).ToList();
            var result = new ShardingDataReader(dbDataReaders, StreamDataReader,executionContext);
            var columns = result.DataReaders[0].GetColumnSchema().ToList();
            // var resultDataReader = result.DataReaders[0];
            // var mySqlDataReader = (MySqlDataReader)resultDataReader;
            //
            // var mySqlConnection = new MySqlConnection("server=127.0.0.1;port=3306;database=test;userid=root;password=root;");
            // var mySqlCommand = mySqlConnection.CreateCommand();
            // mySqlCommand.CommandText = _sql;
            // var mySqlDataReader = mySqlCommand.ExecuteReader();
            QueryServerResult= new QueryServerResult(columns);
            return QueryServerResult;
        }
        else
        {
            var affectCount = queryServerDataReader.ExecuteNonQuery();
            return new AffectRowServerResult(new List<AffectRowUnitResult>(){new AffectRowUnitResult(affectCount,0)});
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