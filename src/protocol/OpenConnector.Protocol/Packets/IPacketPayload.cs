using DotNetty.Buffers;

namespace OpenConnector.Protocol.Packets;

public interface IPacketPayload:IDisposable
{
    IByteBuffer GetByteBuffer();
}