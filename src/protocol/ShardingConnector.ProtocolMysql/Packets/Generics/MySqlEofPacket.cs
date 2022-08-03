using ShardingConnector.ProtocolCore;
using ShardingConnector.ProtocolMysql.Constants;
using ShardingConnector.ProtocolMysql.Payloads;

namespace ShardingConnector.ProtocolMysql.Packets.Generics;

public class MySqlEofPacket:IMysqlPacket
{
    /// <summary>
    /// Header of EOF packet.
    /// </summary>
    public const int HEADER = 0xfe;

    public MySqlEofPacket(int sequenceId,MySQLStatusFlagEnum statusFlag):this(sequenceId,0,statusFlag)
    {
        
    }
    public MySqlEofPacket(int sequenceId,int warnings,MySQLStatusFlagEnum statusFlag)
    {
        SequenceId = sequenceId;
        Warnings = warnings;
        StatusFlag = statusFlag;
    }

    public MySqlEofPacket(MySqlPacketPayload payload)
    {
        SequenceId = payload.ReadInt1();
        Check.ArgumentIfFail(HEADER==payload.ReadInt1(),"header of MySql Eof packet must be `0xfe`.");
        Warnings = payload.ReadInt2();
        StatusFlag=(MySQLStatusFlagEnum)payload.ReadInt2();
    }
    public void Write(MySqlPacketPayload payload)
    {
       payload.WriteInt1(HEADER);
       payload.WriteInt2(Warnings);
       payload.WriteInt2((int)StatusFlag);
    }

    public int SequenceId { get; }
    public int Warnings { get; }
    public MySQLStatusFlagEnum StatusFlag { get; }
}