using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;

namespace ShardingConnector.Proxy.Network;

public sealed class PackDecoder:ByteToMessageDecoder
{
    protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
    {
        throw new NotImplementedException();
    }
}