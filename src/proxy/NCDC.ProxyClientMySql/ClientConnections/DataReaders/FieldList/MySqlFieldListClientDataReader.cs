using NCDC.CommandParser.Common.Command;
using NCDC.Protocol.MySql.Constant;
using NCDC.Protocol.MySql.Constant.CharacterSets;
using NCDC.Protocol.MySql.Packet.Command;
using NCDC.Protocol.MySql.Packet.Generic;
using NCDC.Protocol.MySql.Payload;
using NCDC.Protocol.Packets;
using NCDC.ProxyClient.Abstractions;
using NCDC.ProxyClientMySql.Common;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Connection;
using NCDC.ProxyServer.Connection.Abstractions;
using NCDC.ProxyServer.Extensions;
using NCDC.ShardingAdoNet;
using NCDC.ShardingParser.Abstractions;

namespace NCDC.ProxyClientMySql.ClientConnections.DataReaders.FieldList;

public sealed class MySqlFieldListClientDataReader : IClientDataReader<MySqlPacketPayload>
{
    private const string SQL = "SHOW COLUMNS FROM {0} FROM {1}";
    private readonly string _table;
    private readonly string _filedWildcard;
    private readonly IConnectionSession _connectionSession;
    private readonly string _database;
    private readonly IServerDataReader _serverDbDataReader;
    private readonly MySqlCharacterSetEnum _mySqlCharacterSet;

    public MySqlFieldListClientDataReader(string table, string filedWildcard, IConnectionSession connectionSession,
        IServerDataReaderFactory serverDataReaderFactory,ISqlCommandContextFactory sqlCommandContextFactory)
    {
        _table = table;
        _filedWildcard = filedWildcard;
        _connectionSession = connectionSession;
        _mySqlCharacterSet = connectionSession.Channel.GetMySqlCharacterSet().Value;
        var sql = string.Format(SQL, _table, _connectionSession.DatabaseName);
        var sqlCommand = ParseSqlCommand(sql);
        var sqlCommandContext = sqlCommandContextFactory.Create(ParameterContext.Empty, sqlCommand);
        var  queryContext = new QueryContext(_connectionSession,sqlCommandContext, sql, ParameterContext.Empty);
        _serverDbDataReader = serverDataReaderFactory.Create(queryContext);
    }

    private ISqlCommand ParseSqlCommand(string sql)
    {
        return _connectionSession.GetSqlCommandParser().Parse(sql,false);
    }

    public async IAsyncEnumerable<IPacket<MySqlPacketPayload>> SendCommand()
    {
        await _serverDbDataReader.ExecuteDbDataReaderAsync();

        int currentSequenceId = 0;
        while (_serverDbDataReader.Read())
        {
            var columnName = _serverDbDataReader.GetRowData().Cells[0].ToString();
            yield return new MySqlColumnDefinition41Packet(++currentSequenceId, (int)_mySqlCharacterSet, _database, _table, _table,
                columnName ?? string.Empty, columnName ?? string.Empty, 100,
                (int)MySqlColumnTypeEnum.VarChar,
                0, true);
        }

        yield return new MySqlOkPacket(++currentSequenceId,
            ServerStatusFlagCalculator.CalculateFor(_connectionSession));
    }

    // private async IAsyncEnumerable<IPacket<MySqlPacketPayload>> GetColumnDefinition41Packet()
    // {
    //     var result = new LinkedList<IPacket<MySqlPacketPayload>>();
    //     int currentSequenceId = 0;
    //     while (_serverDbDataReader.Read())
    //     {
    //         var columnName = _serverDbDataReader.GetRowData().Cells[0].ToString();
    //         result.AddLast(new MySqlColumnDefinition41Packet(++currentSequenceId,_dbEncoding,_database,_table,_table,columnName??string.Empty,columnName??string.Empty,100,(int)MySqlColumnTypeEnum.MYSQL_TYPE_VARCHAR,0,true));
    //     }
    //
    //     result.AddLast(new MySqlOkPacket(++currentSequenceId,
    //         ServerStatusFlagCalculator.CalculateFor(_connectionSession)));
    //     return result;
    // }

    public void Dispose()
    {
        _serverDbDataReader?.Dispose();
    }
}