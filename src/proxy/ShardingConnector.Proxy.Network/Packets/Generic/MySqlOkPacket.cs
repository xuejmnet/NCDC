namespace ShardingConnector.Proxy.Network.Packets.Generic;

public class MySqlOkPacket:IMysqlPacket
{
    public static readonly int HEADER = 0X00;
    private static readonly int DEFAULT_STATUS_FLAG = (int)MySQLStatusFlagEnum.SERVER_STATUS_AUTOCOMMIT;
    
    private readonly int _sequenceId;
    private readonly long _affectedRows;
    private readonly long _lastInsertId;
    private readonly int _statusFlag;
    private readonly int _warnings;
    private readonly string _info;
    public MySqlOkPacket(int sequenceId) : this(sequenceId, 0L, 0L)
    {
        
    }

    public MySqlOkPacket(int sequenceId,long affectedRows,long lastInsertId):this(sequenceId,affectedRows,lastInsertId,DEFAULT_STATUS_FLAG,0,string.Empty)
    {
        
    }

    public MySqlOkPacket(int sequenceId,long affectedRows,long lastInsertId,int statusFlag,int warnings,string info)
    {
        _sequenceId = sequenceId;
        _affectedRows = affectedRows;
        _lastInsertId = lastInsertId;
        _statusFlag = statusFlag;
        _warnings = warnings;
        _info = info;
    }

    public MySqlOkPacket(MySqlPacketPayload payload)
    {
        
        _sequenceId = payload.ReadInt1();
        if (HEADER != payload.ReadInt1())
        {
            throw new ArgumentException("Header of MySQL OK packet must be `0x00`.");
        }
        _affectedRows = payload.ReadIntLenenc();
        _lastInsertId = payload.ReadIntLenenc();
        _statusFlag = payload.ReadInt2();
        _warnings = payload.ReadInt2();
        _info = payload.ReadStringEOF();
    }
    public void Write(MySqlPacketPayload payload)
    {
        payload.WriteInt1(HEADER);
        payload.WriteIntLenenc(_affectedRows);
        payload.WriteIntLenenc(_lastInsertId);
        payload.WriteInt2(_statusFlag);
        payload.WriteInt2(_warnings);
        payload.WriteStringEOF(_info);
    }

    public int GetSequenceId()
    {
        return _sequenceId;
    }

    public void Write(IPacketPayload payload)
    {
        Write((MySqlPacketPayload)payload);
    }
}