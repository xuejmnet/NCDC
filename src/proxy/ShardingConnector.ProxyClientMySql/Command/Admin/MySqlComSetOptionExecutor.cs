using ShardingConnector.ProtocolCore.Packets;
using ShardingConnector.ProtocolCore.Packets.Executor;
using ShardingConnector.ProtocolMysql.Constant;
using ShardingConnector.ProtocolMysql.Packet.Command.Admin;
using ShardingConnector.ProtocolMysql.Packet.Generic;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyClientMySql.Command.Admin;

public class MySqlComSetOptionExecutor:ICommandExecutor
{
    private readonly MySqlServerComSetOptionPacket _packet;
    private readonly ConnectionSession _connectionSession;

    public MySqlComSetOptionExecutor(MySqlServerComSetOptionPacket packet,ConnectionSession connectionSession)
    {
        _packet = packet;
        _connectionSession = connectionSession;
    }
    public List<IDatabasePacket> Execute()
    {
        _connectionSession.AttributeMap.GetAttribute(MySqlConstants.MYSQL_OPTION_MULTI_STATEMENTS).Set(_packet.Value);
        return new List<IDatabasePacket>()
        {
            new MySqlOkPacket(1, (MySqlStatusFlagEnum)ServerStatusFlagCalculator.CalculateFor(_connectionSession))
        };
    }
    public void Dispose()
    {
        
    }
}