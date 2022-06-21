namespace ShardingConnector.Proxy.Network;

public interface DatabasePacket
{
    void Write(IPacketPayload payload);
}
public interface DatabasePacket<T>:DatabasePacket where T:IPacketPayload
{
    void Write(T payload);
}