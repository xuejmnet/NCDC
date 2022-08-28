namespace ShardingConnector.Protocol.Packets;

public interface IPacket
{
    void WriteTo(IPacketPayload payload);
}
public interface IPacket<in T> :IPacket where T:IPacketPayload
{
    void WriteTo(T payload);

    void IPacket.WriteTo(IPacketPayload payload)
    {
        WriteTo((T)payload);
    }
}