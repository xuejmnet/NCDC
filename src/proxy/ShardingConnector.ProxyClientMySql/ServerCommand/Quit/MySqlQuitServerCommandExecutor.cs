using ShardingConnector.ProtocolCore.Packets;
using ShardingConnector.ProtocolCore.Packets.Executor;
using ShardingConnector.ProtocolMysql.Constant;
using ShardingConnector.ProtocolMysql.Packet.Generic;

namespace ShardingConnector.ProxyClientMySql.ServerCommand.Quit;

public sealed class MySqlQuitServerCommandExecutor:ICommandExecutor
{
    public List<IDatabasePacket> Execute()
    {
        return new List<IDatabasePacket>(1)
        {
            new MySqlOkPacket(1, MySqlStatusFlagEnum.SERVER_STATUS_DEFAULT)
        };
    }
}