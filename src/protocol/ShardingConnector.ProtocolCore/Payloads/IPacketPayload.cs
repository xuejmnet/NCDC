using DotNetty.Buffers;

namespace ShardingConnector.ProtocolCore.Payloads;

public interface IPacketPayload:IDisposable
{
    IByteBuffer GetByteBuffer();
}