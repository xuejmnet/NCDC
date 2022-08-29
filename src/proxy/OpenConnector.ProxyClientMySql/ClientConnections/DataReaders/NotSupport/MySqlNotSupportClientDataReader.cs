using OpenConnector.Protocol.Errors;
using OpenConnector.Protocol.MySql.Packet.Command;
using OpenConnector.Protocol.MySql.Packet.Generic;
using OpenConnector.Protocol.MySql.Payload;
using OpenConnector.Protocol.Packets;
using OpenConnector.ProxyClient.Abstractions;
using OpenConnector.ProxyServer.Commons;

namespace OpenConnector.ProxyClientMySql.ClientConnections.DataReaders.NotSupport;

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