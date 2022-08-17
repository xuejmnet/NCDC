using ShardingConnector.CommandParser.Command;
using ShardingConnector.CommandParser.Command.DAL.Dialect;
using ShardingConnector.CommandParser.Command.DAL.Dialect.MySql;
using ShardingConnector.CommandParser.Command.DCL;
using ShardingConnector.CommandParser.Command.DML;
using ShardingConnector.CommandParser.Util;
using ShardingConnector.Common;
using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.Session;
using ShardingConnector.ProxyServer.TextCommandHandlers.Skip;
using ShardingConnector.ProxyServer.TextProtocolHandlers;

namespace ShardingConnector.ProxyServer.TextCommandHandlers;

public class TextCommandHandlerFactory:ITextCommandHandlerFactory
{
    public ITextCommandHandler Create(DatabaseTypeEnum databaseType, string sql, ISqlCommand sqlCommand,
        ConnectionSession connectionSession)
    {
        //取消sql的注释信息
        var trimCommentSql = SqlUtil.TrimComment(sql);
        if (string.IsNullOrEmpty(trimCommentSql))
        {
            return new SkipCommandHandler(new EmptyCommand());
        }

        CheckNotSupportCommand(sqlCommand);
        if (sqlCommand is DALCommand dalCommand)
        {
            return CreateDALCommandServerHandler(dalCommand, sql,connectionSession);
        }

        throw new NotSupportedException();
    }

    private ITextCommandHandler CreateDALCommandServerHandler(DALCommand dalCommand, string sql,
        ConnectionSession connectionSession)
    {
        if (dalCommand is SetCommand)
        {
            return new BroadcastHandler(sql, connectionSession);
        }
        throw new NotSupportedException();
    }

    private void CheckNotSupportCommand(ISqlCommand sqlCommand)
    {
        if (sqlCommand is DCLCommand || sqlCommand is FlushCommand || sqlCommand is MySqlShowCreateUserCommand)
        {
            throw new NotSupportedException("unsupported operation");
        }
    }
}