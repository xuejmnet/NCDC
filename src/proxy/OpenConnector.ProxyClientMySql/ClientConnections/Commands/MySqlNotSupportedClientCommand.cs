using OpenConnector.Protocol.MySql.Packet.Command;
using OpenConnector.Protocol.MySql.Payload;
using OpenConnector.ProxyClient.Abstractions;
using OpenConnector.ProxyClient.Exceptions;
using OpenConnector.ProxyClientMySql.ClientConnections.DataReaders.NotSupport;

namespace OpenConnector.ProxyClientMySql.ClientConnections.Commands;

public sealed class MySqlNotSupportedClientCommand:IClientCommand<MySqlPacketPayload>
{
    private readonly MySqlCommandTypeEnum _commandType;

    public MySqlNotSupportedClientCommand(MySqlCommandTypeEnum commandType)
    {
        _commandType = commandType;
    }
    public IClientDataReader<MySqlPacketPayload> ExecuteReader()
    {
        return new MySqlNotSupportClientDataReader(_commandType);
    }
}