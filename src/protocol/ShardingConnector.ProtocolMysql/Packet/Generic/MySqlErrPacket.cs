using ShardingConnector.ProtocolCore;
using ShardingConnector.ProtocolCore.Errors;
using ShardingConnector.ProtocolCore.Payloads;
using ShardingConnector.ProtocolMysql.Payload;

namespace ShardingConnector.ProtocolMysql.Packet.Generic;

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


    public MySqlErrPacket(int sequenceId,ISqlErrorCode sqlErrorCode,params object[] errorMessageArguments):this(sequenceId,sqlErrorCode.GetErrorCode(),sqlErrorCode.GetSqlState(),string.Format(sqlErrorCode.GetErrorMessage(),errorMessageArguments))
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

    public void Write(IPacketPayload payload)
    {
        Write((MySqlPacketPayload)payload);
    }
}