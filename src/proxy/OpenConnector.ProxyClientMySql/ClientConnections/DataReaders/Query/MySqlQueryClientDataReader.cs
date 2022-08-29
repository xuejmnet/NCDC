using System.Data.Common;
using MySqlConnector;
using OpenConnector.CommandParser.Command;
using OpenConnector.CommandParser.Command.DML;
using OpenConnector.Common;
using OpenConnector.ParserEngine;
using OpenConnector.Protocol.MySql.Constant;
using OpenConnector.Protocol.MySql.Packet;
using OpenConnector.Protocol.MySql.Packet.Command;
using OpenConnector.Protocol.MySql.Packet.Generic;
using OpenConnector.Protocol.MySql.Payload;
using OpenConnector.Protocol.Packets;
using OpenConnector.ProxyClient.Abstractions;
using OpenConnector.ProxyClientMySql.ClientConnections.Commands;
using OpenConnector.ProxyClientMySql.ClientConnections.DataReaders.Query.ServerHandlers;
using OpenConnector.ProxyClientMySql.Common;
using OpenConnector.ProxyServer;
using OpenConnector.ProxyServer.Abstractions;
using OpenConnector.ProxyServer.Commons;
using OpenConnector.ProxyServer.ServerHandlers.Results;
using OpenConnector.ProxyServer.Session;

namespace OpenConnector.ProxyClientMySql.ClientConnections.DataReaders.Query;

public sealed class MySqlQueryClientDataReader : IClientQueryDataReader<MySqlPacketPayload>
{
    public ConnectionSession ConnectionSession { get; }
    public IServerHandler ServerHandler { get; }
    public int MySqlEncoding { get; }
    private int _currentSequenceId;
    public ResultTypeEnum ResultType { get; private set; }

    public MySqlQueryClientDataReader(string sql, ConnectionSession connectionSession,
        IServerHandlerFactory serverHandlerFactory)
    {
        ConnectionSession = connectionSession;
        var sqlCommand = ParseSql(sql);
        var isMultiCommands = IsMultiCommands(connectionSession, sqlCommand, sql);
        ServerHandler = isMultiCommands
            ? new MySqlMultiServerHandler()
            : serverHandlerFactory.Create(DatabaseTypeEnum.MySQL, sql, sqlCommand, connectionSession);
        MySqlEncoding = connectionSession.AttributeMap.GetAttribute(MySqlConstants.MYSQL_CHARACTER_SET_ATTRIBUTE_KEY)
            .Get().DbEncoding;
    }

    private ISqlCommand ParseSql(string sql)
    {
        if (string.IsNullOrEmpty(sql))
        {
            throw new NotSupportedException(sql);
        }

       return ProxyContext.ShardingRuntimeContext.GetSqlParserEngine().Parse(sql, false);
    }

    private bool IsMultiCommands(ConnectionSession connectionSession, ISqlCommand sqlCommand, string sql)
    {
        return connectionSession.AttributeMap.HasAttribute(MySqlConstants.MYSQL_OPTION_MULTI_STATEMENTS)
               && MySqlSetOptionClientCommand.MYSQL_OPTION_MULTI_STATEMENTS_ON.Equals(connectionSession.AttributeMap
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