using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Common.Command;
using NCDC.ProxyServer.Connection;
using NCDC.ProxyServer.Connection.Abstractions;

namespace NCDC.ProxyServer.Executors;

public interface IShardingExecutionContextFactory
{
    ShardingExecutionContext Create(QueryContext queryContext);
}