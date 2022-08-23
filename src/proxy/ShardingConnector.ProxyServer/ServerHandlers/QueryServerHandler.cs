using System.Data.Common;
using MySqlConnector;
using ShardingConnector.AdoNet.Executor;
using ShardingConnector.AdoNet.Executor.Abstractions;
using ShardingConnector.Pluggable.Merge;
using ShardingConnector.Pluggable.Prepare;
using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.Binaries;
using ShardingConnector.ProxyServer.ServerHandlers.Results;
using ShardingConnector.ProxyServer.Session;
using ShardingConnector.ShardingAdoNet;
using ExecutionContext = ShardingConnector.Executor.Context.ExecutionContext;
using ShardingDataReader = ShardingConnector.AdoNet.AdoNet.Core.DataReader.ShardingDataReader;
using ShardingRuntimeContext = ShardingConnector.AdoNet.AdoNet.Core.Context.ShardingRuntimeContext;

namespace ShardingConnector.ProxyServer.ServerHandlers;

public sealed class QueryServerHandler:IServerHandler
{
    public string Sql { get; }
    public ConnectionSession ConnectionSession { get; }
    private readonly ICommandExecutor _commandExecutor;
    public IStreamDataReader? StreamDataReader { get; private set; }
    public QueryServerResult? QueryServerResult { get; private set; }

    public QueryServerHandler(string sql,ConnectionSession connectionSession)
    {
        Sql = sql;
        ConnectionSession = connectionSession;
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
    public IServerResult Send()
    {
        var executionContext = Prepare(Sql);
        var dataReaders = _commandExecutor.ExecuteDbDataReader(false,executionContext);
        StreamDataReader = MergeQuery(executionContext,dataReaders);
       
        var result = new ShardingDataReader(_commandExecutor.GetDataReaders(), StreamDataReader,executionContext);
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