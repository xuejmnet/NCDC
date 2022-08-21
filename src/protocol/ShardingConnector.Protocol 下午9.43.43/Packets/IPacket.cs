namespace ShardingConnector.Protocol.Packets;

public interface IPacket
{
    void Write(IPacketPayload payload);
}
public interface IPacket<in T> :IPacket where T:IPacketPayload
{
    void Write(T payload);

    void IPacket.Write(IPacketPayload payload)
    {
        Write((T)payload);
    }
}