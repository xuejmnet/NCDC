using ShardingConnector.ProtocolMysql.Packet.ServerCommand;
using ShardingConnector.ProtocolMysql.Payload;

namespace ShardingConnector.ProtocolMysql.Packet.Command.Admin;

public class MySqlServerNotSupportCommandPacket:AbstractMySqlServerCommandPacket
{
    public MySqlServerNotSupportCommandPacket(MySqlCommandTypeEnum commandType) : base(commandType)
    {
    }

    protected override void DoWrite(MySqlPacketPayload payload)
    {
        
    }
}