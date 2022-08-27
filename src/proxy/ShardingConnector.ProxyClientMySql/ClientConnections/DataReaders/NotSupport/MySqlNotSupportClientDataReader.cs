using ShardingConnector.Protocol.Errors;
using ShardingConnector.Protocol.MySql.Packet.Command;
using ShardingConnector.Protocol.MySql.Packet.Generic;
using ShardingConnector.Protocol.MySql.Payload;
using ShardingConnector.Protocol.Packets;
using ShardingConnector.ProxyClient.Abstractions;
using ShardingConnector.ProxyServer.Commons;

namespace ShardingConnector.ProxyClientMySql.ClientConnections.DataReaders.NotSupport;

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