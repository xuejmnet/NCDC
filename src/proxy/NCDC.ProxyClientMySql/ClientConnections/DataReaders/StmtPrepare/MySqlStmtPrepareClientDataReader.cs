using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Command.DML;
using NCDC.Protocol.MySql.Payload;
using NCDC.Protocol.Packets;
using NCDC.ProxyClient.Abstractions;
using NCDC.ProxyServer.Connection.Abstractions;

namespace NCDC.ProxyClientMySql.ClientConnections.DataReaders.StmtPrepare;

public sealed class MySqlStmtPrepareClientDataReader:IClientDataReader<MySqlPacketPayload>
{
    private readonly string _sql;
    private readonly IConnectionSession _connectionSession;

    public MySqlStmtPrepareClientDataReader(string sql,IConnectionSession connectionSession)
    {
        _sql = sql;
        _connectionSession = connectionSession;
    }
    public IAsyncEnumerable<IPacket<MySqlPacketPayload>> SendCommand()
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

    // private int GetColumnsCount(ISqlCommand sqlCommand)
    // {
    //     return sqlCommand is SelectCommand selectCommand ? selectCommand.Projections.GetProjections().Count : 0;
    // }
    // private ISqlCommand ParseSql(string sql)
    // {
    //     if (string.IsNullOrEmpty(sql))
    //     {
    //         throw new NotSupportedException(sql);
    //     }
    //
    //     return ProxyContext.ShardingRuntimeContext.GetSqlParserEngine().Parse(sql, false);
    // }
}