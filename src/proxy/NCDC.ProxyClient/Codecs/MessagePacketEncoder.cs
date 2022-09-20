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
    private readonly ILogger<MessagePacketEncoder> _logger= InternalNCDCLoggerFactory.CreateLogger<MessagePacketEncoder>();
    private readonly bool _isDebugEnabled;
    private readonly IPacketCodec _packetCodec;

    public MessagePacketEncoder(IPacketCodec packetCodec)
    {
        _isDebugEnabled=_logger.IsEnabled(LogLevel.Debug);
        _packetCodec = packetCodec;
    }
    public override bool IsSharable => true;
    protected override void Encode(IChannelHandlerContext context, IPacket message, IByteBuffer output)
    {
        _packetCodec.Encode(context,message,output);
        if (_isDebugEnabled)
        {
            _logger.LogDebug($"write to client {context.Channel.Id.AsShortText()} : \n{ByteBufferUtil.PrettyHexDump(output)}");
        }
    }
}