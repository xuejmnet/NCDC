using System.Text;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using NCDC.Protocol.Errors;
using NCDC.Protocol.MySql.Packet;
using NCDC.Protocol.MySql.Packet.Generic;
using NCDC.Protocol.MySql.Payload;
using NCDC.Protocol.Packets;
using NCDC.ProxyClient.Codecs;
using NCDC.ProxyServer;
using NCDC.ProxyServer.Extensions;

namespace NCDC.ProxyClientMySql.Codec;

public sealed class MySqlPacketCodecEngine : IPacketCodec
{
    /// <summary>
    /// 16MB
    /// </summary>
    public const int MAX_PACKET_LENGTH = 0xFFFFFF;
    /// <summary>
    /// 
    /// </summary>
    public const int PAYLOAD_LENGTH = 3;
    /// <summary>
    /// 序号长度
    /// </summary>
    public const int SEQUENCE_LENGTH = 1;
    /// <summary>
    /// 最小二进制长度
    /// </summary>
    public const int READABLE_BYTES_MIN_LENGTH = PAYLOAD_LENGTH+SEQUENCE_LENGTH;

    private readonly List<IByteBuffer> _pendingmessages = new List<IByteBuffer>();

    public bool IsValidHeader(int readableBytes)
    {
        return readableBytes >= READABLE_BYTES_MIN_LENGTH;
    }

    public void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
    {
        var payloadLength = input.MarkReaderIndex().ReadUnsignedMediumLE();
        var remainPayloadLength = SEQUENCE_LENGTH+payloadLength;
        if (input.ReadableBytes < remainPayloadLength)
        {
            input.ResetReaderIndex();
            return;
        }

        var message = input.ReadRetainedSlice(SEQUENCE_LENGTH+payloadLength);
        if (MAX_PACKET_LENGTH == payloadLength)
        {
            _pendingmessages.Add(message);
        } else if (_pendingmessages.Count == 0)
        {
            output.Add(message);
        }
        else
        {
            AggregateMessages(context, message, output);
        }
    }

    private void AggregateMessages(IChannelHandlerContext context, IByteBuffer lastMessage, List<object> output)
    {
        var result = context.Allocator.CompositeBuffer(_pendingmessages.Count+1);
        int i = 0;
        foreach (var byteBuffer in _pendingmessages)
        {
            result.AddComponent(true, i > 0? byteBuffer.SkipBytes(1):byteBuffer);
            i++;
        }

        if (lastMessage.ReadableBytes > 1)
        {
            result.AddComponent(true, lastMessage.SkipBytes(1));
        }
        output.Add(result);
        _pendingmessages.Clear();
    }

    public void Encode(IChannelHandlerContext context, IPacket message, IByteBuffer output)
    {
        var markWriterIndex = PrepareMessageHeader(output).MarkWriterIndex();
        var encoding = context.Channel.GetEncoding();
        var payload = new MySqlPacketPayload(markWriterIndex,encoding);
        try
        {
            message.WriteTo(payload);
        }
        catch (Exception ex)
        {
            output.ResetWriterIndex();
            new MySqlErrPacket(1, CommonSqlErrorCode.UNKNOWN_EXCEPTION_ARGS1, ex.Message).WriteTo(payload);
        }
        finally
        {
            UpdateMessageHeader(output, ((IMysqlPacket)message).SequenceId);
        }
    }

    private void UpdateMessageHeader(IByteBuffer byteBuffer, int sequenceId)
    {
        byteBuffer.SetMediumLE(0, byteBuffer.ReadableBytes - PAYLOAD_LENGTH - SEQUENCE_LENGTH);
        byteBuffer.SetByte(3, sequenceId);
    }

    private IByteBuffer PrepareMessageHeader(IByteBuffer output)
    {
        //先占4位 前3位长度 第4位sequenceId
        return output.WriteInt(0);
    }

    public IPacketPayload CreatePacketPayload(IByteBuffer message, Encoding encoding)
    {
        return new MySqlPacketPayload(message, encoding);
    }
}