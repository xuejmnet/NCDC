using ShardingConnector.CommandParser.Command;
using ShardingConnector.CommandParser.Command.DML;
using ShardingConnector.Common;
using ShardingConnector.ParserEngine;
using ShardingConnector.ProtocolCore.Packets;
using ShardingConnector.ProtocolCore.Packets.Executor;
using ShardingConnector.ProtocolMysql.Constant;
using ShardingConnector.ProtocolMysql.Packet;
using ShardingConnector.ProtocolMysql.Packet.Command.Admin;
using ShardingConnector.ProtocolMysql.Packet.Generic;
using ShardingConnector.ProtocolMysql.Packet.ServerCommand.Query;
using ShardingConnector.ProxyClientMySql.Command;
using ShardingConnector.ProxyClientMySql.Command.Query.Text.Query;
using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.Response.Header;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyClientMySql.ServerCommand.Query;

public sealed class MySqlQueryServerCommandExecutor:IQueryCommandExecutor
{
    public ConnectionSession ConnectionSession { get; }
    public ITextCommandHandler TextCommandHandler { get; }
    public int MySqlEncoding { get; }
    private int _currntSequenceId;

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
    public List<IDatabasePacket> Execute()
    {
        var responseHeader = TextCommandHandler.Execute();
        if (responseHeader is UpdateResponseHeader updateResponseHeader)
        {
            return new List<IDatabasePacket>()
            {
                CreateUpdatePacket(updateResponseHeader)
            };
        }

        throw new NotImplementedException();
    }

    public ResponseTypeEnum GetResponseType()
    {
        throw new NotImplementedException();
    }

    public IDatabasePacket GetQueryRowPacket()
    {
        throw new NotImplementedException();
    }

    private IMysqlPacket CreateUpdatePacket(UpdateResponseHeader responseHeader)
    {
        return new MySqlOkPacket(1, responseHeader.GetUpdateCount(), responseHeader.LastInsertId,(MySqlStatusFlagEnum)ServerStatusFlagCalculator.CalculateFor(ConnectionSession));
    }

}