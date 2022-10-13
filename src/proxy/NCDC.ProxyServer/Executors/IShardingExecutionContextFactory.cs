using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Common.Command;
using NCDC.ProxyServer.Connection.Abstractions;

namespace NCDC.ProxyServer.Executors;

public interface IShardingExecutionContextFactory
{
    ShardingExecutionContext Create(IConnectionSession connectionSession,string sql);
    ShardingExecutionContext Create(string sql,ISqlCommand sqlCommand);
}