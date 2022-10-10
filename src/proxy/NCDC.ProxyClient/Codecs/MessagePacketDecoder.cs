using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;
using NCDC.Logger;

namespace NCDC.ProxyClient.Codecs;

/// <summary>
/// 消息包解码器
/// </summary>
public sealed class MessagePacketDecoder:ByteToMessageDecoder
{
    private static readonly ILogger<MessagePacketDecoder> _logger = NCDCLoggerFactory.CreateLogger<MessagePacketDecoder>();
    private readonly bool _logDecodePacket;
    private readonly IPacketCodec _packetCodec;

    public MessagePacketDecoder(IPacketCodec packetCodec)
    {
        _logDecodePacket=packetCodec.LogDecodePacket();
        _packetCodec = packetCodec;
    }
    protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
    {
        var readableBytes = input.ReadableBytes;
        var isValidHeader = _packetCodec.IsValidHeader(readableBytes);
        if (!isValidHeader)
        {
            _logger.LogWarning($"decode invalid header");
            return;
        }

        if (_logDecodePacket)
        {
            _logger.LogDebug($"read from client {context.Channel.Id.AsShortText()} : \n{ByteBufferUtil.PrettyHexDump(input)}");
        }
        _packetCodec.Decode(context,input,output);
    }
}