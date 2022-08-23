using ShardingConnector.Protocol.MySql.Packet.Command;
using ShardingConnector.Protocol.Packets;
using ShardingConnector.ProxyClient.Abstractions;
using ShardingConnector.ProxyClient.Exceptions;

namespace ShardingConnector.ProxyClientMySql.ClientDataReader.NotSupported;

public sealed class MySqlNotSupportedClientDataReader:IClientDataReader
{
    private readonly MySqlCommandTypeEnum _commandType;

    public MySqlNotSupportedClientDataReader(MySqlCommandTypeEnum commandType)
    {
        _commandType = commandType;
    }
    public List<IPacket> SendCommand()
    {
        throw new NotSupportedCommandException($"{_commandType}");
    }
}