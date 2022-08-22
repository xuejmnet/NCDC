using System.Data.Common;
using MySqlConnector;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.CommandParser.Command.DML;
using ShardingConnector.Common;
using ShardingConnector.ParserEngine;
using ShardingConnector.Protocol.MySql.Constant;
using ShardingConnector.Protocol.MySql.Packet;
using ShardingConnector.Protocol.MySql.Packet.Command;
using ShardingConnector.Protocol.MySql.Packet.Generic;
using ShardingConnector.Protocol.Packets;
using ShardingConnector.ProxyClient;
using ShardingConnector.ProxyClientMySql.Command;
using ShardingConnector.ProxyClientMySql.Command.Query.Text.Query;
using ShardingConnector.ProxyServer;
using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.Commands;
using ShardingConnector.ProxyServer.Response;
using ShardingConnector.ProxyServer.Response.EffectRow;
using ShardingConnector.ProxyServer.Response.Query;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyClientMySql.ServerCommand.Query;

public sealed class MySqlQueryServerCommandExecutor:IQueryCommandExecutor
{
    public ConnectionSession ConnectionSession { get; }
    public ITextCommandHandler TextCommandHandler { get; }
    public int MySqlEncoding { get; }
    private int _currentSequenceId;
    private ResponseTypeEnum _responseType;

    public MySqlQueryServerCommandExecutor(MySqlQueryServerCommandPacket mySqlQueryServerCommandPacket,ConnectionSession connectionSession,ITextCommandHandlerFactory textCommandHandlerFactory)
    {
        ConnectionSession = connectionSession;
        var sql = mySqlQueryServerCommandPacket.Sql;
        var sqlCommand = ParseSql(sql);
        var isMultiCommands = IsMultiCommands(connectionSession,sqlCommand,sql);
        TextCommandHandler=isMultiCommands
            ? new MySqlMultiCommandHandler()
            : textCommandHandlerFactory.Create(DatabaseTypeEnum.MySQL, sql, sqlCommand, connectionSession);
        MySqlEncoding = connectionSession.AttributeMap.GetAttribute(MySqlConstants.MYSQL_CHARACTER_SET_ATTRIBUTE_KEY)
            .Get().DbEncoding;
    }

    private ISqlCommand ParseSql(string sql)
    {
        if (string.IsNullOrEmpty(sql))
        {
            throw new NotSupportedException(sql);
        }

        return SqlParserEngineFactory.GetSqlParserEngine("MySql").Parse(sql, false);
    }

    private bool IsMultiCommands(ConnectionSession connectionSession, ISqlCommand sqlCommand, string sql)
    {
        return connectionSession.AttributeMap.HasAttribute(MySqlConstants.MYSQL_OPTION_MULTI_STATEMENTS)
            && MySqlServerComSetOptionPacket.MYSQL_OPTION_MULTI_STATEMENTS_ON.Equals(connectionSession.AttributeMap
                .GetAttribute(MySqlConstants.MYSQL_OPTION_MULTI_STATEMENTS).Get())
            && (sqlCommand is UpdateCommand || sqlCommand is DeleteCommand) && sql.Contains(";");
    }
    public List<IPacket> Execute()
    {
        var responseHeader = TextCommandHandler.Execute();
        if (responseHeader is QueryResponse queryResponse)
        {
            return ProcessQuery(queryResponse);
        }

        _responseType = ResponseTypeEnum.UPDATE;
        
        if (responseHeader is EffectRowServerResponse updateResponseHeader)
        {
            return new List<IPacket>()
            {
                CreateUpdatePacket(updateResponseHeader)
            };
        }

        throw new NotImplementedException();
    }

    private List<IPacket> ProcessQuery(QueryResponse queryResponse)
    {
        _responseType = ResponseTypeEnum.QUERY;
        var result = BuildQueryResponsePackets(queryResponse,MySqlEncoding,(MySqlStatusFlagEnum)ServerStatusFlagCalculator.CalculateFor(ConnectionSession));
        _currentSequenceId = result.Count;
        return result;
    }
    
    public static  List<IPacket> BuildQueryResponsePackets( QueryResponse queryResponseHeader,  int characterSet,  MySqlStatusFlagEnum statusFlags) {
        List<IPacket> result = new (queryResponseHeader.DbColumns.Count+2);
        int sequenceId = 0;
        var dbColumns = queryResponseHeader.DbColumns;
        result.Add(new MySqlFieldCountPacket(++sequenceId, dbColumns.Count));
        foreach (var dbColumn in dbColumns)
        {
            result.Add(new MySqlColumnDefinition41Packet(++sequenceId,characterSet,GetColumnFieldDetailFlag(dbColumn),dbColumn.BaseSchemaName,dbColumn.BaseTableName,dbColumn.BaseTableName,
                dbColumn.ColumnName,dbColumn.BaseColumnName,dbColumn.ColumnSize.Value,(int)((MySqlDbColumn)dbColumn).ProviderType,dbColumn.NumericScale??0,false));
        }
        result.Add(new MySqlEofPacket(++sequenceId, statusFlags));
        return result;
    }

    private static int GetColumnFieldDetailFlag(DbColumn header) {
        int result = 0;
        if (header.IsKey is true) {
            result += (int)MySqlColumnFieldDetailFlagEnum.PRIMARY_KEY;
        }
        if (!(header.AllowDBNull is true)) {
            result += (int)MySqlColumnFieldDetailFlagEnum.NOT_NULL;
        }

        var mySqlDbColumn = (MySqlDbColumn)header;
        var mySqlDbType = mySqlDbColumn.ProviderType;
        if (mySqlDbType==MySqlDbType.UByte||mySqlDbType==MySqlDbType.UInt16||mySqlDbType==MySqlDbType.UInt24||mySqlDbType==MySqlDbType.UInt32||mySqlDbType==MySqlDbType.UInt64) {
            result += (int)MySqlColumnFieldDetailFlagEnum.UNSIGNED;
        }
        if (header.IsIdentity is true) {
            result += (int)MySqlColumnFieldDetailFlagEnum.AUTO_INCREMENT;
        }
        return result;
    }

    public ResponseTypeEnum GetResponseType()
    {
        return _responseType;
    }

    public IPacket GetQueryRowPacket()
    {
        return new MySqlTextResultSetRowPacket(++_currentSequenceId, TextCommandHandler.GetRowData().GetData());
    }

    public bool MoveNext()
    {
        return TextCommandHandler.MoveNext();
    }

    private IMysqlPacket CreateUpdatePacket(EffectRowServerResponse serverResponse)
    {
        return new MySqlOkPacket(1, serverResponse.GetUpdateCount(), serverResponse.LastInsertId,(MySqlStatusFlagEnum)ServerStatusFlagCalculator.CalculateFor(ConnectionSession));
    }

    public void Dispose()
    {
        TextCommandHandler.Dispose();
    }
}