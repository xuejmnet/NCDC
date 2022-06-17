using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;

namespace ShardingConnector.Proxy.Network;

public class PackEncoder:MessageToByteEncoder<DatabasePacket<IPacketPayload>>
{
    protected override void Encode(IChannelHandlerContext context, DatabasePacket<IPacketPayload> message, IByteBuffer output)
    {
        throw new NotImplementedException();
    }
}