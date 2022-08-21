using ShardingConnector.Protocol.MySql.Constant;
using ShardingConnector.Protocol.MySql.Payload;
using ShardingConnector.Protocol.Packets;

namespace ShardingConnector.Protocol.MySql.Packet.Generic;

public class MySqlOkPacket : IMysqlPacket
{
    public const int HEADER = 0X00;

    public int SequenceId { get; }
    public long AffectedRows { get; }
    public long LastInsertId { get; }
    public MySqlStatusFlagEnum StatusFlag { get; }
    public int Warnings { get; }
    public string Info { get; }

    public MySqlOkPacket(int sequenceId, MySqlStatusFlagEnum statusFlag) : this(sequenceId, 0L, 0L, statusFlag)
    {
    }

    public MySqlOkPacket(int sequenceId, long affectedRows, long lastInsertId, MySqlStatusFlagEnum statusFlag) : this(
        sequenceId, affectedRows, lastInsertId, statusFlag, 0, string.Empty)
    {
    }

    public MySqlOkPacket(int sequenceId, long affectedRows, long lastInsertId, MySqlStatusFlagEnum statusFlag,
        int warnings, string info)
    {
        SequenceId = sequenceId;
        AffectedRows = affectedRows;
        LastInsertId = lastInsertId;
        StatusFlag = statusFlag;
        Warnings = warnings;
        Info = info;
    }

    public MySqlOkPacket(MySqlPacketPayload payload)
    {
        SequenceId = payload.ReadInt1();
        if (HEADER != payload.ReadInt1())
        {
            throw new ArgumentException("header of MySql Ok packet must be `0x00`.");
        }

        AffectedRows = payload.ReadIntLenenc();
        LastInsertId = payload.ReadIntLenenc();
        StatusFlag = (MySqlStatusFlagEnum)payload.ReadInt2();
        Warnings = payload.ReadInt2();
        Info = payload.ReadStringEOF();
    }

    public void Write(MySqlPacketPayload payload)
    {
        payload.WriteInt1(HEADER);
        payload.WriteIntLenenc(AffectedRows);
        payload.WriteIntLenenc(LastInsertId);
        payload.WriteInt2((int)StatusFlag);
        payload.WriteInt2(Warnings);
        payload.WriteStringEOF(Info);
    }
    public void Write(IPacketPayload payload)
    {
        Write((MySqlPacketPayload)payload);
    }
}