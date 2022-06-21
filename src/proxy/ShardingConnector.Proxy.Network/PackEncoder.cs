using System.Text;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;

namespace ShardingConnector.Proxy.Network;

public class PackEncoder:MessageToByteEncoder<DatabasePacket>
{
    public override bool IsSharable => true;
    protected override void Encode(IChannelHandlerContext context, DatabasePacket message, IByteBuffer output)
    {
        // try (MySQLPacketPayload payload = new MySQLPacketPayload(context.alloc().buffer())) {
        //     message.write(payload);
        //         out.writeMediumLE(payload.getByteBuf().readableBytes());
        //         out.writeByte(message.getSequenceId());
        //         out.writeBytes(payload.getByteBuf());
        // }
        var mysqlPacket = message as MysqlPacket;
        using (var mySqlPacketPayload = new MySqlPacketPayload(context.Allocator.Buffer(),Encoding.Default))
        {
            mysqlPacket.Write(mySqlPacketPayload);
            var byteBuffer = mySqlPacketPayload.GetByteBuffer();
            output.WriteMediumLE(byteBuffer.ReadableBytes);
            output.WriteByte(mysqlPacket.GetSequenceId());
            output.WriteBytes(byteBuffer);
        }
    }
}