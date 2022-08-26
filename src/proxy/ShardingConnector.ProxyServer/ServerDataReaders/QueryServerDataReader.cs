using ShardingConnector.Executor.Context;
using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyServer.ServerDataReaders;

public sealed class QueryServerDataReader:AbstractAdoServerDataReader
{
    public QueryServerDataReader(StreamMergeContext streamMergeContext, ConnectionSession connectionSession) : base(streamMergeContext, connectionSession)
    {
    }
}