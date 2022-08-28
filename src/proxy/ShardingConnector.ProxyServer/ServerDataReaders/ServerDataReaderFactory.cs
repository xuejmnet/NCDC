using System.Data.Common;
using ShardingConnector.Extensions;
using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.Options.Context;
using ShardingConnector.ProxyServer.Session;
using ShardingConnector.ProxyServer.StreamMerges.Executors.Context;
using ShardingConnector.ShardingAdoNet;
using BasePrepareEngine = ShardingConnector.ProxyServer.StreamMerges.ExecutePrepares.Prepare.BasePrepareEngine;
using SimpleQueryPrepareEngine = ShardingConnector.ProxyServer.StreamMerges.ExecutePrepares.Prepare.SimpleQueryPrepareEngine;

namespace ShardingConnector.ProxyServer.ServerDataReaders;

public sealed class ServerDataReaderFactory:IServerDataReaderFactory
{
    public IServerDataReader Create(string sql, ConnectionSession connectionSession)
    {
        var shardingExecutionContext = DoShardingRoute(sql);
        if (shardingExecutionContext.GetExecutionUnits().IsEmpty())
        {
            return EmptyServerDataReader.Instance;
        }
        return new QueryServerDataReader(shardingExecutionContext, connectionSession);
    }
    private ShardingExecutionContext DoShardingRoute(string sql)
    {

        ShardingRuntimeContext runtimeContext = ProxyContext.ShardingRuntimeContext;
            
        BasePrepareEngine prepareEngine = new SimpleQueryPrepareEngine(
            runtimeContext.GetRule().ToRules(), runtimeContext.GetProperties(), runtimeContext.GetMetaData(),
            runtimeContext.GetSqlParserEngine());
        var parameterContext =
            new ParameterContext(Array.Empty<DbParameter>());
            
        ShardingExecutionContext result = prepareEngine.Prepare(sql, parameterContext);
        //TODO
        // _commandExecutor.Init(result);
        // //_commandExecutor.Commands.for
        // _commandExecutor.Commands.ForEach(ReplyTargetMethodInvoke);
        return result;
    }
}