using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;
using ShardingConnector.Logger;

namespace ShardingConnector.ProtocolCore.Codecs;

public sealed class MessagePacketDecoder:ByteToMessageDecoder
{
    private static readonly ILogger<MessagePacketDecoder> _logger = InternalLoggerFactory.CreateLogger<MessagePacketDecoder>();
    private readonly bool _isDebugEnabled;
    private readonly IDatabasePacketCodecEngine _databasePacketCodecEngine;

    public MessagePacketDecoder(IDatabasePacketCodecEngine databasePacketCodecEngine)
    {
        _isDebugEnabled=_logger.IsEnabled(LogLevel.Debug);
        _databasePacketCodecEngine = databasePacketCodecEngine;
    }
    protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
    {
        var readableBytes = input.ReadableBytes;
        var isValidHeader = _databasePacketCodecEngine.IsValidHeader(readableBytes);
        if (!isValidHeader)
        {
            return;
        }

        if (_isDebugEnabled)
        {
            _logger.LogDebug($"read from client {context.Channel.Id.AsShortText()} : \n{ByteBufferUtil.PrettyHexDump(input)}");
        }
        _databasePacketCodecEngine.Decode(context,input,output);
        // var payLoadLength = input.MarkReaderIndex().ReadMediumLE();
        // var realPacketLength = payLoadLength+MySqlPacketConstant.PAYLOAD_LENGTH+MySqlPacketConstant.SEQUENCE_LENGTH;
        // if (readableBytes < realPacketLength)
        // {
        //     input.ResetReaderIndex();
        //     return;
        // }
        // output.Add(input.ReadRetainedSlice(payLoadLength+MySqlPacketConstant.SEQUENCE_LENGTH));
    }
}