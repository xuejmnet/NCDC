using NCDC.Protocol.MySql.Packet.Command;
using NCDC.Protocol.MySql.Payload;
using NCDC.ProxyClient.Abstractions;
using NCDC.ProxyClientMySql.ClientConnections.DataReaders.NotSupport;

namespace NCDC.ProxyClientMySql.ClientConnections.Commands;

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