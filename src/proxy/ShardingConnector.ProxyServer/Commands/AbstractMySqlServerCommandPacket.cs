using ShardingConnector.Protocol.MySql.Packet;
using ShardingConnector.Protocol.MySql.Packet.Command;
using ShardingConnector.Protocol.MySql.Payload;
using ShardingConnector.Protocol.Packets;

namespace ShardingConnector.ProxyServer.Commands;

/// <summary>
/// mysql 服务端发送的命令包抽象
/// </summary>
public abstract class AbstractMySqlServerCommandPacket:IMysqlPacket,IServerCommandPacket
{
    private readonly MySqlCommandTypeEnum _commandType;

    public AbstractMySqlServerCommandPacket(MySqlCommandTypeEnum commandType)
    {
        SequenceId = 0;
        _commandType = commandType;
    }
    public void WriteTo(MySqlPacketPayload payload)
    {
        payload.WriteInt1((int)_commandType);
        DoWriteTo(payload);
    }

    protected virtual void DoWriteTo(MySqlPacketPayload payload)
    {
        
    }

    public void WriteTo(IPacketPayload payload)
    {
        WriteTo((MySqlPacketPayload)payload);
    }

    public int SequenceId { get; }
}