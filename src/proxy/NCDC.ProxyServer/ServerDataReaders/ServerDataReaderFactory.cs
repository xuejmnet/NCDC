using NCDC.CommandParser.Abstractions;
using NCDC.Extensions;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Connection.Abstractions;

namespace NCDC.ProxyServer.ServerDataReaders;

public sealed class ServerDataReaderFactory:IServerDataReaderFactory
{
    public IServerDataReader Create(string sql,ISqlCommand sqlCommand, IConnectionSession connectionSession)
    {
        var shardingExecutionContextFactory = connectionSession.RuntimeContext!.GetShardingExecutionContextFactory();
        var shardingExecutionContext =shardingExecutionContextFactory.Create(sql,sqlCommand);
        if (shardingExecutionContext.GetExecutionUnits().IsEmpty())
        {
            return EmptyServerDataReader.Instance;
        }
        return new QueryServerDataReader(shardingExecutionContext, connectionSession);
    }

    public IServerDataReader Create(string sql, IConnectionSession connectionSession)
    {
        var shardingExecutionContextFactory = connectionSession.RuntimeContext!.GetShardingExecutionContextFactory();
        var shardingExecutionContext =shardingExecutionContextFactory.Create(sql);
        if (shardingExecutionContext.GetExecutionUnits().IsEmpty())
        {
            return EmptyServerDataReader.Instance;
        }
        return new QueryServerDataReader(shardingExecutionContext, connectionSession);
    }
}