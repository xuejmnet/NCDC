using System.Text.RegularExpressions;
using NCDC.CommandParser.Common.Command;
using NCDC.CommandParser.Common.Command.DML;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Connection;
using NCDC.ProxyServer.Connection.Abstractions;
using NCDC.ProxyServer.Executors;
using NCDC.ShardingAdoNet;
using NCDC.ShardingParser.Abstractions;

namespace NCDC.ProxyClientMySql.ClientConnections.DataReaders.Query.ServerHandlers;

public sealed class MySqlMultiServerHandler:IServerHandler
{
    private static readonly Regex MULTI_UPDATE_STATEMENTS = new Regex(";(?=\\s*update)");
    private static readonly Regex MULTI_DELETE_STATEMENTS = new Regex(";(?=\\s*delete)");
    private readonly IConnectionSession _connectionSession;
    private readonly ISqlCommandContextFactory _sqlCommandContextFactory;

    private readonly Dictionary<string, List<ExecutionUnit>> _dataSourceToExecutionUnits =
        new Dictionary<string, List<ExecutionUnit>>(); 
    private ShardingExecutionContext anyExecutionContext;

    public MySqlMultiServerHandler(IConnectionSession connectionSession,ISqlCommand sqlCommand,string sql,ISqlCommandContextFactory sqlCommandContextFactory)
    {
        _connectionSession = connectionSession;
        _sqlCommandContextFactory = sqlCommandContextFactory;
        var pattern=sqlCommand is UpdateCommand ? MULTI_UPDATE_STATEMENTS : MULTI_DELETE_STATEMENTS;

        var extractMultiCommands = ExtractMultiCommands(pattern,sql);
        var sqlCommandParser = connectionSession.GetSqlCommandParser();
        foreach (var extractSql in extractMultiCommands)
        {
            var extractCommand = sqlCommandParser.Parse(extractSql,false);
            var sqlCommandContext = sqlCommandContextFactory.Create(ParameterContext.Empty, extractCommand);
            var queryContext = new QueryContext(connectionSession,sqlCommandContext,extractSql,ParameterContext.Empty);
            var shardingExecutionContextFactory = queryContext.ConnectionSession.RuntimeContext.GetShardingExecutionContextFactory();
            var shardingExecutionContext =shardingExecutionContextFactory.Create(queryContext);
            if (anyExecutionContext == null)
            {
                anyExecutionContext = shardingExecutionContext;
            }
            foreach (var executionUnit in shardingExecutionContext.GetExecutionUnits())
            {
                if (!_dataSourceToExecutionUnits.ContainsKey(executionUnit.GetDataSourceName()))
                {
                    _dataSourceToExecutionUnits.Add(executionUnit.GetDataSourceName(),new List<ExecutionUnit>());
                }
                _dataSourceToExecutionUnits[executionUnit.GetDataSourceName()].Add(executionUnit);
            }
        }
    }

    private List<string> ExtractMultiCommands(Regex pattern, string sql)
    {
        return pattern.Split(sql).ToList();
    }
    public Task<IServerResult> ExecuteAsync()
    {
        throw new NotImplementedException();
    }
}