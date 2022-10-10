using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;
using NCDC.Logger;
using NCDC.Protocol.Packets;

namespace NCDC.ProxyClient.Codecs;

/// <summary>
/// 消息包编码器
/// </summary>
public sealed class MessagePacketEncoder:MessageToByteEncoder<IPacket>
{
    private readonly ILogger<MessagePacketEncoder> _logger= NCDCLoggerFactory.CreateLogger<MessagePacketEncoder>();
    private readonly bool _logEncodePacket;
    private readonly IPacketCodec _packetCodec;

    public MessagePacketEncoder(IPacketCodec packetCodec)
    {
        _logEncodePacket = packetCodec.LogEncodePacket();
        _packetCodec = packetCodec;
    }
    public override bool IsSharable => true;
    protected override void Encode(IChannelHandlerContext context, IPacket message, IByteBuffer output)
    {
        _packetCodec.Encode(context,message,output);
        if (_logEncodePacket)
        {
            _logger.LogDebug($"write to client {context.Channel.Id.AsShortText()} : \n{ByteBufferUtil.PrettyHexDump(output)}");
        }
    }
}