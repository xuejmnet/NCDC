using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Common.Command;
using NCDC.Extensions;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Connection.Abstractions;
using NCDC.ShardingParser.Abstractions;

namespace NCDC.ProxyServer.ServerDataReaders;

public sealed class ServerDataReaderFactory:IServerDataReaderFactory
{
    public IServerDataReader Create(IQueryContext queryContext)
    {
        var shardingExecutionContextFactory = queryContext.ConnectionSession.RuntimeContext.GetShardingExecutionContextFactory();
        var shardingExecutionContext =shardingExecutionContextFactory.Create(queryContext);
        if (shardingExecutionContext.GetExecutionUnits().IsEmpty())
        {
            return EmptyServerDataReader.Instance;
        }
        return new QueryServerDataReader(shardingExecutionContext, queryContext);
    }
}