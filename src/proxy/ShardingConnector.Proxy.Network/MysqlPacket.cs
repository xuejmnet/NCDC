namespace ShardingConnector.Proxy.Network;

public interface MysqlPacket:DatabasePacket<MySqlPacketPayload>
{
    int GetSequenceId();
}