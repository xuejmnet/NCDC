using ShardingConnector.ProtocolCore;
using ShardingConnector.ProtocolCore.Errors;
using ShardingConnector.ProtocolMysql.Payloads;

namespace ShardingConnector.ProtocolMysql.Packets.Generics;

public class MySqlErrPacket: IMysqlPacket
{
    /// <summary>
    /// Header of ERR packet.
    /// </summary>
    public const int HEADER = 0xff;
    public const string  SQL_STATE_MARKER = "#";
    public int SequenceId { get; }
    public int ErrorCode { get; }
    public string SqlState { get; }
    public string ErrorMessage { get; }


    public MySqlErrPacket(int sequenceId,ISqlErrorCode sqlErrorCode):this(sequenceId,sqlErrorCode.GetErrorCode(),sqlErrorCode.GetSqlState(),sqlErrorCode.GetErrorMessage())
    {
        
    }
    public MySqlErrPacket(int sequenceId,int errorCode,string sqlState,string errorMessage)
    {
        SequenceId = sequenceId;
        ErrorCode = errorCode;
        SqlState = sqlState;
        ErrorMessage = errorMessage;
    }

    public MySqlErrPacket(MySqlPacketPayload payload)
    {
        SequenceId = payload.ReadInt1();
        Check.ArgumentIfFail(HEADER!=payload.ReadInt1(),"header of MySql Error packet must be `0xff`.");
        ErrorCode = payload.ReadInt2();
        payload.ReadStringFix(1);
        SqlState = payload.ReadStringFix(5);
        ErrorMessage = payload.ReadStringEOF();
        
    }
    public void Write(MySqlPacketPayload payload)
    {
        payload.WriteInt1(HEADER);
        payload.WriteInt2(ErrorCode);
        if (SequenceId != 0)
        {
            payload.WriteStringFix(SQL_STATE_MARKER);
            payload.WriteStringFix(SqlState);
        }
        payload.WriteStringEOF(ErrorMessage);
    }

    public int GetSequenceId()
    {
        throw new NotImplementedException();
    }
}