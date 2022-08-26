using ShardingConnector.ProxyServer.StreamMerges;

namespace ShardingConnector.ProxyServer.Abstractions;

public interface IServerDataReader
{
    IExecuteResult ExecuteDbDataReader(CancellationToken cancellationToken=new CancellationToken());

}