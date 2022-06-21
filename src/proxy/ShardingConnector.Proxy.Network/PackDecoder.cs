using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;

namespace ShardingConnector.Proxy.Network;

public sealed class PackDecoder:ByteToMessageDecoder
{
    protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
    {
        var readableBytes = input.ReadableBytes;
        var payLoadLength = input.MarkReaderIndex().ReadMediumLE();
        var realPacketLength = payLoadLength+MySqlPacketConstant.PAYLOAD_LENGTH+MySqlPacketConstant.SEQUENCE_LENGTH;
        if (readableBytes < realPacketLength)
        {
            input.ResetReaderIndex();
            return;
        }
        output.Add(input.ReadRetainedSlice(payLoadLength+MySqlPacketConstant.SEQUENCE_LENGTH));
    }
}