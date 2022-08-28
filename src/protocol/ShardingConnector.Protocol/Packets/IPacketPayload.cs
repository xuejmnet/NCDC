using DotNetty.Buffers;

namespace ShardingConnector.Protocol.Packets;

public interface IPacketPayload:IDisposable
{
    IByteBuffer GetByteBuffer();
}