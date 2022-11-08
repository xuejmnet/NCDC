using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Common.Command;
using NCDC.Extensions;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Connection.Abstractions;

namespace NCDC.ProxyServer.ServerDataReaders;

public sealed class ServerDataReaderFactory:IServerDataReaderFactory
{
    public IServerDataReader Create(string sql,ISqlCommand sqlCommand, IConnectionSession connectionSession)
    {
        var shardingExecutionContextFactory = connectionSession.RuntimeContext!.GetShardingExecutionContextFactory();
        var shardingExecutionContext =shardingExecutionContextFactory.Create(connectionSession);
        if (shardingExecutionContext.GetExecutionUnits().IsEmpty())
        {
            return EmptyServerDataReader.Instance;
        }
        return new QueryServerDataReader(shardingExecutionContext, connectionSession);
    }

    public IServerDataReader Create(string sql, IConnectionSession connectionSession)
    {
        var shardingExecutionContextFactory = connectionSession.RuntimeContext!.GetShardingExecutionContextFactory();
        var shardingExecutionContext =shardingExecutionContextFactory.Create(connectionSession);
        if (shardingExecutionContext.GetExecutionUnits().IsEmpty())
        {
            return EmptyServerDataReader.Instance;
        }
        return new QueryServerDataReader(shardingExecutionContext, connectionSession);
    }
}