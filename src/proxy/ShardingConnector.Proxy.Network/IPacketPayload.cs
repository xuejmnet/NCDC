using DotNetty.Buffers;

namespace ShardingConnector.Proxy.Network;

public interface IPacketPayload:IDisposable
{
    IByteBuffer GetByteBuffer();
}