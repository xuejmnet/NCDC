using NCDC.Protocol.Errors;
using NCDC.Protocol.MySql.Packet.Command;
using NCDC.Protocol.MySql.Packet.Generic;
using NCDC.Protocol.MySql.Payload;
using NCDC.Protocol.Packets;
using NCDC.ProxyClient.Abstractions;

namespace NCDC.ProxyClientMySql.ClientConnections.DataReaders.NotSupport;

public sealed class MySqlNotSupportClientDataReader:IClientDataReader<MySqlPacketPayload>
{
    private readonly MySqlCommandTypeEnum _commandType;

    public MySqlNotSupportClientDataReader(MySqlCommandTypeEnum commandType)
    {
        _commandType = commandType;
    }
    public IEnumerable<IPacket<MySqlPacketPayload>> SendCommand()
    {
        return new IPacket<MySqlPacketPayload>[]
        {
            new MySqlErrPacket(1, CommonSqlErrorCode.UNSUPPORTED_COMMAND_ARGS1, _commandType)
        };
    }
}