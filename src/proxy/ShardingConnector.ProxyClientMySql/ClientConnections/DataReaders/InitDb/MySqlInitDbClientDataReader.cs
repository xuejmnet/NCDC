using ShardingConnector.CommandParser.Util;
using ShardingConnector.Protocol.MySql.Constant;
using ShardingConnector.Protocol.MySql.Packet.Generic;
using ShardingConnector.Protocol.MySql.Payload;
using ShardingConnector.Protocol.Packets;
using ShardingConnector.ProxyClient.Abstractions;
using ShardingConnector.ProxyClientMySql.Common;
using ShardingConnector.ProxyServer;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyClientMySql.ClientConnections.DataReaders.InitDb;

public sealed class MySqlInitDbClientDataReader : IClientDataReader<MySqlPacketPayload>
{
    private readonly string _database;
    private readonly ConnectionSession _connectionSession;

    public MySqlInitDbClientDataReader(string database, ConnectionSession connectionSession)
    {
        _database = database;
        _connectionSession = connectionSession;
    }

    public IEnumerable<IPacket<MySqlPacketPayload>> SendCommand()
    {
        var exactlySchema = SqlUtil.GetExactlyValue(_database);
        if (ProxyRuntimeContext.Instance.DatabaseExists(exactlySchema)&&IsAuthorizeDatabase(exactlySchema))
        {
            _connectionSession.SetCurrentDatabaseName(exactlySchema);
            return new List<IPacket<MySqlPacketPayload>>()
            {
                new MySqlOkPacket(1, ServerStatusFlagCalculator.CalculateFor(_connectionSession))
            };
        }

        return new List<IPacket<MySqlPacketPayload>>()
        {
            new MySqlErrPacket(1, MySqlServerErrorCode.ER_BAD_DB_ERROR_ARG1,_database)
        };
    }

    /// <summary>
    /// 判断初始化的数据库是存在当前用户配置
    /// </summary>
    /// <param name="database"></param>
    /// <returns></returns>
    private bool IsAuthorizeDatabase(string database)
    {
        var logicDatabase = ProxyRuntimeContext.Instance.GetDatabase(database)!;
        return logicDatabase.UserNameAuthorize(_connectionSession.GetGrantee().Username);
    }
}