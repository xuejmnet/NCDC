using ShardingConnector.ProxyServer.StreamMerges;

namespace ShardingConnector.ProxyServer.Abstractions;

public interface IServerDataReader
{
    IStreamDataReader ExecuteDbDataReader(CancellationToken cancellationToken=new CancellationToken());
    int ExecuteNonQuery(CancellationToken cancellationToken=new CancellationToken());

}