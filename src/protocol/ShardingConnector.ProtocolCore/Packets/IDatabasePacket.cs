using ShardingConnector.ProtocolCore.Payloads;

namespace ShardingConnector.ProtocolCore.Packets;

// public interface IDatabasePacket
// {
//     void Write(IPacketPayload payload);
// }
public interface IDatabasePacket<T> where T:IPacketPayload
{
    void Write(T payload);
}