using System.Data.Common;
using MySqlConnector;
using ShardingConnector.AdoNet.AdoNet.Core.Command;
using ShardingConnector.AdoNet.AdoNet.Core.Context;
using ShardingConnector.AdoNet.Executor;
using ShardingConnector.AdoNet.Executor.Abstractions;
using ShardingConnector.Exceptions;
using ShardingConnector.Pluggable.Merge;
using ShardingConnector.Pluggable.Prepare;
using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.Response;
using ShardingConnector.ProxyServer.Response.Data;
using ShardingConnector.ProxyServer.Response.Query;
using ShardingConnector.ProxyServer.Session;
using ShardingConnector.ShardingAdoNet;
using ExecutionContext = ShardingConnector.Executor.Context.ExecutionContext;
using ShardingDataReader = ShardingConnector.AdoNet.AdoNet.Core.DataReader.ShardingDataReader;
using ShardingRuntimeContext = ShardingConnector.AdoNet.AdoNet.Core.Context.ShardingRuntimeContext;

namespace ShardingConnector.ProxyServer.TextCommandHandlers;

public sealed class QueryTextCommandHandler:ITextCommandHandler
{
    private readonly IServerConnector _serverConnector;
    private readonly string _sql;
    private readonly ConnectionSession _connectionSession;
    private readonly ICommandExecutor _commandExecutor;
    public IStreamDataReader? StreamDataReader { get; private set; }
    public QueryResponse? QueryResponse { get; private set; }

    public QueryTextCommandHandler(IServerConnector serverConnector,string sql,ConnectionSession connectionSession)
    {
        _serverConnector = serverConnector;
        _sql = sql;
        _connectionSession = connectionSession;
        _commandExecutor= CreateCommandExecutor(10);
    }

    private ICommandExecutor CreateCommandExecutor(int maxQueryConnectionsLimit)
    {
        var commandExecutor = new DefaultCommandExecutor(maxQueryConnectionsLimit);
        commandExecutor.OnGetConnections += (c, s, i) =>
        {
            var mySqlConnection = new MySqlConnection("server=127.0.0.1;port=3306;database=test;userid=root;password=root;");
            mySqlConnection.Open();
            return new List<DbConnection>(){mySqlConnection};
            // if (_shardingConnection != null)
            // {
            //     return _shardingConnection.GetConnections(c, s, i);
            // }
            //
            // throw new ShardingException(
            //     $"{nameof(ShardingCommand)} {nameof(_shardingConnection)} is null");
        };
        return commandExecutor;
    }
    public IServerResponse Execute()
    {
        // if (_connectionSession.GetDatabaseName() == null)
        // {
        //     throw new Exception("123123");
        // }

        var executionContext = Prepare(_sql);
        var dataReaders = _commandExecutor.ExecuteDbDataReader(false,executionContext);
                
        StreamDataReader = MergeQuery(executionContext,dataReaders);
        var result = new ShardingDataReader(_commandExecutor.GetDataReaders(), StreamDataReader,executionContext);
       var columns = result.DataReaders[0].GetColumnSchema().ToList();
       // MySqlDbColumn
       
       //
        // var mySqlConnection = new MySqlConnection("server=127.0.0.1;port=3306;database=test;userid=root;password=root;");
        // var mySqlCommand = mySqlConnection.CreateCommand();
        // mySqlCommand.CommandText = _sql;
        // var mySqlDataReader = mySqlCommand.ExecuteReader();
        QueryResponse= new QueryResponse(columns);
        return QueryResponse;
        // return _serverConnector.Execute();
    }
    private IStreamDataReader MergeQuery(ExecutionContext executionContext,List<IStreamDataReader> streamDataReaders)
    {
        ShardingRuntimeContext runtimeContext = ProxyContext.ShardingRuntimeContext;
        MergeEngine mergeEngine = new MergeEngine(runtimeContext.GetRule().ToRules(),
            runtimeContext.GetProperties(), runtimeContext.GetDatabaseType(), runtimeContext.GetMetaData().Schema);
        return mergeEngine.Merge(streamDataReaders, executionContext.GetSqlCommandContext());
    }

    private ExecutionContext Prepare(string sql)
    {

        ShardingRuntimeContext runtimeContext = ProxyContext.ShardingRuntimeContext;
            
        BasePrepareEngine prepareEngine = new PreparedQueryPrepareEngine(
            runtimeContext.GetRule().ToRules(), runtimeContext.GetProperties(), runtimeContext.GetMetaData(),
            runtimeContext.GetSqlParserEngine());
        var parameterContext =
            new ParameterContext(Array.Empty<DbParameter>());
            
        ExecutionContext result = prepareEngine.Prepare(sql, parameterContext);
        //TODO
        // _commandExecutor.Init(result);
        // //_commandExecutor.Commands.for
        // _commandExecutor.Commands.ForEach(ReplyTargetMethodInvoke);
        return result;
    }
    public bool MoveNext()
    {
        return StreamDataReader!.Read();
    }

    public QueryResponseRow GetRowData()
    {
        var columnCount = StreamDataReader!.ColumnCount;
        var cells = new List<QueryResponseCell>(columnCount);
        for (int i = 0; i < columnCount; i++)
        {
            cells.Add(new QueryResponseCell(QueryResponse.DbColumns[i].DataType,StreamDataReader.GetValue(i)));
        }
        return new QueryResponseRow(cells);
        
        // for (int columnIndex = 1; columnIndex <= queryResultMetaData.getColumnCount(); columnIndex++) {
        //     result.add(new QueryResponseCell(queryResultMetaData.getColumnType(columnIndex), mergedResult.getValue(columnIndex, Object.class)));
        // }
        // return new QueryResponseRow(result);
    }
}