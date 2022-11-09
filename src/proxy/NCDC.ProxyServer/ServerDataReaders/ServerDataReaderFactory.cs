using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Common.Command;
using NCDC.Extensions;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Connection.Abstractions;
using NCDC.ShardingParser.Abstractions;

namespace NCDC.ProxyServer.ServerDataReaders;

public sealed class ServerDataReaderFactory:IServerDataReaderFactory
{
    private readonly ISqlCommandContextFactory _sqlCommandContextFactory;

    public ServerDataReaderFactory(ISqlCommandContextFactory sqlCommandContextFactory)
    {
        _sqlCommandContextFactory = sqlCommandContextFactory;
    }
    public IServerDataReader Create(IConnectionSession connectionSession)
    {
        var shardingExecutionContextFactory = connectionSession.RuntimeContext!.GetShardingExecutionContextFactory();
        var shardingExecutionContext =shardingExecutionContextFactory.Create(connectionSession.QueryContext!);
        if (shardingExecutionContext.GetExecutionUnits().IsEmpty())
        {
            return EmptyServerDataReader.Instance;
        }
        return new QueryServerDataReader(shardingExecutionContext, connectionSession);
    }
}