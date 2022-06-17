namespace ShardingConnector.Proxy.Network;

public interface DatabasePacket<T> where T:IPacketPayload
{
    void Write(T payload);
}