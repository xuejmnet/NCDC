// using ShardingConnector.ParserBinder.Command.DML;
// using ShardingConnector.ProxyServer.Abstractions;
// using ShardingConnector.ProxyServer.Response;
// using ShardingConnector.ProxyServer.Response.Data;
// using ShardingConnector.ProxyServer.Response.EffectRow;
// using ShardingConnector.ProxyServer.Response.Query;
// using ExecutionContext = ShardingConnector.Executor.Context.ExecutionContext;
//
// namespace ShardingConnector.ProxyServer.Connectors;
//
// public sealed class AdoNetServerConnector:IServerConnector
// {
//     private readonly ITextCommandExecutor _textCommandExecutor;
//     private List<QueryResultColumnTitle>? _queryResultColumnTitles;
//     private IStreamDataReader? _streamDataReader;
//
//     public AdoNetServerConnector(ITextCommandExecutor textCommandExecutor)
//     {
//         _textCommandExecutor = textCommandExecutor;
//     }
//     public IServerResponse Execute(ExecutionContext executionContext)
//     {
//         if (executionContext.GetSqlCommandContext() is SelectCommandContext)
//         {
//             var serverResponses = _textCommandExecutor.ExecuteQuery(false,executionContext);
//             return null;
//         }
//         else
//         {
//             var executeNonQuery = _textCommandExecutor.ExecuteNonQuery(false,executionContext);
//             var effectRowServerExecuteResults = executeNonQuery.Select(o=>new EffectRowServerExecuteResult(o,0)).ToList();
//             return new EffectRowServerResponse(effectRowServerExecuteResults);
//         }
//     }
//
//     private IServerResponse MergeQueryServerResponse(List<QueryResponse> queryServerResponses)
//     {
//         new QueryResponse()
//     }
//     
//
//     public bool Read()
//     {
//         throw new NotImplementedException();
//     }
//
//     public QueryResponseRow GetQueryRow()
//     {
//         throw new NotImplementedException();
//     }
// }