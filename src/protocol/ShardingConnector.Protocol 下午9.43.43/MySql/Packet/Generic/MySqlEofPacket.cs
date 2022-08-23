using ShardingConnector.Protocol.MySql.Constant;
using ShardingConnector.Protocol.MySql.Payload;
using ShardingConnector.Protocol.Packets;

namespace ShardingConnector.Protocol.MySql.Packet.Generic;

public class MySqlEofPacket:IMysqlPacket
{
    /// <summary>
    /// Header of EOF packet.
    /// </summary>
    public const int HEADER = 0xfe;

    public MySqlEofPacket(int sequenceId,MySqlStatusFlagEnum statusFlag):this(sequenceId,0,statusFlag)
    {
        
    }
    public MySqlEofPacket(int sequenceId,int warnings,MySqlStatusFlagEnum statusFlag)
    {
        SequenceId = sequenceId;
        Warnings = warnings;
        StatusFlag = statusFlag;
    }

    public MySqlEofPacket(MySqlPacketPayload payload)
    {
        SequenceId = payload.ReadInt1();
        if (HEADER == payload.ReadInt1())
        {
            throw new ArgumentException("header of MySql Eof packet must be `0xfe`.");
        }
        Warnings = payload.ReadInt2();
        StatusFlag=(MySqlStatusFlagEnum)payload.ReadInt2();
    }
    public void WriteTo(MySqlPacketPayload payload)
    {
       payload.WriteInt1(HEADER);
       payload.WriteInt2(Warnings);
       payload.WriteInt2((int)StatusFlag);
    }

    public int SequenceId { get; }
    public int Warnings { get; }
    public MySqlStatusFlagEnum StatusFlag { get; }
}