using System.Data.Common;
using MySqlConnector;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Command.DML;
using NCDC.Enums;
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

        return ConnectionSession.RuntimeContext!.GetSqlCommandParser().Parse(sql, false);
    }

    private bool IsMultiCommands(IConnectionSession connectionSession, ISqlCommand sqlCommand, string sql)
    {
        return connectionSession.Channel.HasAttribute(MySqlConstants.MYSQL_OPTION_MULTI_STATEMENTS)
               && MySqlSetOptionClientCommand.MYSQL_OPTION_MULTI_STATEMENTS_ON.Equals(connectionSession.Channel
                   .GetAttribute(MySqlConstants.MYSQL_OPTION_MULTI_STATEMENTS).Get())
               && (sqlCommand is UpdateCommand || sqlCommand is DeleteCommand) && sql.Contains(";");
    }

    public IEnumerable<IPacket<MySqlPacketPayload>> SendCommand()
    {
        var serverResult = ServerHandler.Execute();
        ResultType = serverResult.ResultType;
        if (serverResult is QueryServerResult queryServerResult)
        {
            return ProcessQuery(queryServerResult);
        }

        if (serverResult is RecordsAffectedServerResult effectResult)
        {
            return new List<IPacket<MySqlPacketPayload>>()
            {
                CreateUpdatePacket(effectResult)
            };
        }

        throw new NotImplementedException();
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
            result.Add(new MySqlColumnDefinition41Packet(++sequenceId, characterSet, GetColumnFieldDetailFlag(dbColumn),
                dbColumn.BaseSchemaName, dbColumn.BaseTableName, dbColumn.BaseTableName,
                dbColumn.ColumnName, dbColumn.BaseColumnName, dbColumn.ColumnSize.Value,
                (int)((MySqlDbColumn)dbColumn).ProviderType, dbColumn.NumericScale ?? 0, false));
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