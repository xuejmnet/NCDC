using NCDC.CommandParser.Util;
using NCDC.Protocol.MySql.Constant;
using NCDC.Protocol.MySql.Packet.Generic;
using NCDC.Protocol.MySql.Payload;
using NCDC.Protocol.Packets;
using NCDC.ProxyClient.Abstractions;
using NCDC.ProxyClientMySql.Common;
using NCDC.ProxyServer.Connection.Abstractions;

namespace NCDC.ProxyClientMySql.ClientConnections.DataReaders.InitDb;

public sealed class MySqlInitDbClientDataReader : IClientDataReader<MySqlPacketPayload>
{
    private readonly string _database;
    private readonly IConnectionSession _connectionSession;

    public MySqlInitDbClientDataReader(string database, IConnectionSession connectionSession)
    {
        _database = database;
        _connectionSession = connectionSession;
    }

    public IEnumerable<IPacket<MySqlPacketPayload>> SendCommand()
    {
        var exactlyDatabase = SqlUtil.GetExactlyValue(_database);
        
        if (_connectionSession.DatabaseExists(exactlyDatabase))
        {
            if (_connectionSession.GetAuthorizeDatabases().Contains(exactlyDatabase))
            {
                _connectionSession.SetCurrentDatabaseName(exactlyDatabase);
                
                return new List<IPacket<MySqlPacketPayload>>()
                {
                    new MySqlOkPacket(1, ServerStatusFlagCalculator.CalculateFor(_connectionSession))
                };
            }
           
        }

        return new List<IPacket<MySqlPacketPayload>>()
        {
            new MySqlErrPacket(1, MySqlServerErrorCode.ER_BAD_DB_ERROR_ARG1,_database)
        };
    }

}