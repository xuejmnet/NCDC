using DotNetty.Buffers;

namespace NCDC.Protocol.Packets;

public interface IPacketPayload:IDisposable
{
    IByteBuffer GetByteBuffer();
}