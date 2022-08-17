using ShardingConnector.ProtocolCore.Payloads;

namespace ShardingConnector.ProtocolCore.Packets;

/// <summary>
/// 数据库命令包
/// </summary>
public interface IDatabasePacket
{
    void Write(IPacketPayload payload);
}
public interface IDatabasePacket<T> :IDatabasePacket where T:IPacketPayload
{
    void Write(T payload);
}