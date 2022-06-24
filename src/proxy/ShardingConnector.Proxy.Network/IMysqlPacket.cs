namespace ShardingConnector.Proxy.Network;

public interface IMysqlPacket:DatabasePacket<MySqlPacketPayload>
{
    int GetSequenceId();
}