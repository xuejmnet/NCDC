using MySqlConnector;
using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParser.Command.DML;
using OpenConnector.Configuration.Session;
using OpenConnector.Protocol.MySql.Constant;
using OpenConnector.Protocol.MySql.Packet.Generic;
using OpenConnector.Protocol.MySql.Payload;
using OpenConnector.Protocol.Packets;
using OpenConnector.ProxyClient.Abstractions;
using OpenConnector.ProxyClientMySql.Common;
using OpenConnector.ProxyServer;
using OpenConnector.ProxyServer.Session;

namespace OpenConnector.ProxyClientMySql.ClientConnections.DataReaders.StmtPrepare;

public sealed class MySqlStmtPrepareClientDataReader:IClientDataReader<MySqlPacketPayload>
{
    private readonly string _sql;
    private readonly ConnectionSession _connectionSession;

    public MySqlStmtPrepareClientDataReader(string sql,ConnectionSession connectionSession)
    {
        _sql = sql;
        _connectionSession = connectionSession;
    }
    public IEnumerable<IPacket<MySqlPacketPayload>> SendCommand()
    {
        throw new NotImplementedException();
        // var result = new LinkedList<IPacket<MySqlPacketPayload>>();
        // int currentSequenceId = 0;
        // var sqlCommand = ParseSql(_sql);
        // if (!MySqlComCommandPrepareChecker.IsCommandAllowed(sqlCommand))
        // {
        //     result.AddLast(new MySqlErrPacket(++currentSequenceId,MySqlServerErrorCode.ER_NO_SUCH_TABLE_ARG1,_sql));
        //     return result;
        // }
        //
        // var parameterCount = sqlCommand.GetParameterCount();
        // var columnsCount = GetColumnsCount(sqlCommand);
        // result.AddLast(new mysqlcomcomm)
    }

    private int GetColumnsCount(ISqlCommand sqlCommand)
    {
        return sqlCommand is SelectCommand selectCommand ? selectCommand.Projections.GetProjections().Count : 0;
    }
    private ISqlCommand ParseSql(string sql)
    {
        if (string.IsNullOrEmpty(sql))
        {
            throw new NotSupportedException(sql);
        }

        return ProxyContext.ShardingRuntimeContext.GetSqlParserEngine().Parse(sql, false);
    }
}