using System.Data.Common;
using MySqlConnector;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Command.DML;
using NCDC.Enums;
using NCDC.Exceptions;
using NCDC.Protocol.MySql.Constant;
using NCDC.Protocol.MySql.Packet;
using NCDC.Protocol.MySql.Packet.Command;
using NCDC.Protocol.MySql.Packet.Generic;
using NCDC.Protocol.MySql.Payload;
using NCDC.Protocol.Packets;
using NCDC.ProxyClient.Abstractions;
using NCDC.ProxyClientMySql.ClientConnections.Commands;
using NCDC.ProxyClientMySql.ClientConnections.DataReaders.Query.ServerHandlers;
using NCDC.ProxyClientMySql.Common;
using NCDC.ProxyClientMySql.Extensions;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Commons;
using NCDC.ProxyServer.Connection.Abstractions;
using NCDC.ProxyServer.Extensions;
using NCDC.ProxyServer.ServerHandlers.Results;

namespace NCDC.ProxyClientMySql.ClientConnections.DataReaders.Query;

public sealed class MySqlQueryClientDataReader : IClientQueryDataReader<MySqlPacketPayload>
{
    public IConnectionSession ConnectionSession { get; }
    public IServerHandler ServerHandler { get; }
    public int MySqlEncoding { get; }
    private int _currentSequenceId;
    public ResultTypeEnum ResultType { get; private set; }

    public MySqlQueryClientDataReader(string sql, IConnectionSession connectionSession,
        IServerHandlerFactory serverHandlerFactory)
    {
        ConnectionSession = connectionSession;
        var sqlCommand = ParseSql(sql);
        var isMultiCommands = IsMultiCommands(connectionSession, sqlCommand, sql);
        ServerHandler = isMultiCommands
            ? new MySqlMultiServerHandler()
            : serverHandlerFactory.Create(DatabaseTypeEnum.MySql, sql, sqlCommand, connectionSession);
        MySqlEncoding = connectionSession.Channel.GetMySqlCharacterSet().DbEncoding;
    }

    private ISqlCommand ParseSql(string sql)
    {
        if (string.IsNullOrEmpty(sql))
        {
            throw new NotSupportedException(sql);
        }

        Console.WriteLine("MySqlQueryClientDataReader:"+sql);
        return ConnectionSession.GetSqlCommandParser().Parse(sql, false);
    }

    private bool IsMultiCommands(IConnectionSession connectionSession, ISqlCommand sqlCommand, string sql)
    {
        return connectionSession.Channel.HasAttribute(MySqlConstants.MYSQL_OPTION_MULTI_STATEMENTS)
               && MySqlSetOptionClientCommand.MYSQL_OPTION_MULTI_STATEMENTS_ON.Equals(connectionSession.Channel
                   .GetAttribute(MySqlConstants.MYSQL_OPTION_MULTI_STATEMENTS).Get())
               && (sqlCommand is UpdateCommand || sqlCommand is DeleteCommand) && sql.Contains(";");
    }

    public async IAsyncEnumerable<IPacket<MySqlPacketPayload>> SendCommand()
    {
        var serverResult =await ServerHandler.ExecuteAsync();
        ResultType = serverResult.ResultType;
        if (serverResult is QueryServerResult queryServerResult)
        {
            var processQuery = ProcessQuery(queryServerResult);
            foreach (var packet in processQuery)
            {
                yield return packet;
            }
        }else if (serverResult is RecordsAffectedServerResult effectResult)
        {
            yield return CreateUpdatePacket(effectResult);
        }
    }

    public IPacket<MySqlPacketPayload>? GetRowPacket()
    {
        return new MySqlTextResultSetRowPacket(++_currentSequenceId,
            ServerHandler.GetRowData().Cells.Select(o => o.Data).ToList());
    }

    private IEnumerable<IPacket<MySqlPacketPayload>> ProcessQuery(QueryServerResult queryServerResult)
    {
        var result = BuildQueryResponsePackets(queryServerResult, MySqlEncoding,
            ServerStatusFlagCalculator.CalculateFor(ConnectionSession));
        _currentSequenceId = result.Count;
        return result;
    }

    public bool MoveNext()
    {
        return ServerHandler.Read();
    }

    public static List<IPacket<MySqlPacketPayload>> BuildQueryResponsePackets(QueryServerResult queryServerResult,
        int characterSet, MySqlStatusFlagEnum statusFlags)
    {
        List<IPacket<MySqlPacketPayload>> result = new(queryServerResult.DbColumns.Count + 2);
        int sequenceId = 0;
        var dbColumns = queryServerResult.DbColumns;
        result.Add(new MySqlFieldCountPacket(++sequenceId, dbColumns.Count));
        foreach (var dbColumn in dbColumns)
        {
            var mySqlDbColumn = (MySqlDbColumn)dbColumn;
            var b = typeof(string)== mySqlDbColumn.DataType|| typeof(Guid)== mySqlDbColumn.DataType;
            var columnFieldDetailFlag = GetColumnFieldDetailFlag(dbColumn);
            result.Add(new MySqlColumnDefinition41Packet(++sequenceId, characterSet,columnFieldDetailFlag ,
                mySqlDbColumn.BaseSchemaName??string.Empty, mySqlDbColumn.BaseTableName??string.Empty, mySqlDbColumn.BaseTableName??string.Empty,
                mySqlDbColumn.ColumnName, mySqlDbColumn.BaseColumnName??string.Empty, mySqlDbColumn.ColumnSize.GetValueOrDefault()*(b?4:1),//utf8mb4是size*4
                (int)mySqlDbColumn.GetMySqlColumnType(null,mySqlDbColumn.ColumnSize.GetValueOrDefault(),columnFieldDetailFlag)
                , mySqlDbColumn.NumericScale ?? 0, false));
        }

        result.Add(new MySqlEofPacket(++sequenceId, statusFlags));
        return result;
    }

    private static int GetColumnFieldDetailFlag(DbColumn header)
    {
        int result = 0;
        if (header.IsKey is true)
        {
            result += (int)MySqlColumnFieldDetailFlagEnum.PRIMARY_KEY;
        }

        if (!(header.AllowDBNull is true))
        {
            result += (int)MySqlColumnFieldDetailFlagEnum.NOT_NULL;
        }

        var mySqlDbColumn = (MySqlDbColumn)header;
        var mySqlDbType = mySqlDbColumn.ProviderType;
        if (mySqlDbType == MySqlDbType.UByte || mySqlDbType == MySqlDbType.UInt16 ||
            mySqlDbType == MySqlDbType.UInt24 || mySqlDbType == MySqlDbType.UInt32 || mySqlDbType == MySqlDbType.UInt64)
        {
            result += (int)MySqlColumnFieldDetailFlagEnum.UNSIGNED;
        }

        if (header.IsIdentity is true)
        {
            result += (int)MySqlColumnFieldDetailFlagEnum.AUTO_INCREMENT;
        }

        return result;
    }


    private IMysqlPacket CreateUpdatePacket(RecordsAffectedServerResult affectResult)
    {
        return new MySqlOkPacket(1, affectResult.UpdateCount, affectResult.LastInsertId,
            (MySqlStatusFlagEnum)ServerStatusFlagCalculator.CalculateFor(ConnectionSession));
    }

    public void Dispose()
    {
        ServerHandler.Dispose();
    }
}