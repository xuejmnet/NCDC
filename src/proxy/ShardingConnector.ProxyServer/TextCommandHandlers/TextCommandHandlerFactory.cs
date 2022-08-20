using Microsoft.Extensions.Logging;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.CommandParser.Command.DAL.Dialect;
using ShardingConnector.CommandParser.Command.DAL.Dialect.MySql;
using ShardingConnector.CommandParser.Command.DCL;
using ShardingConnector.CommandParser.Command.DML;
using ShardingConnector.CommandParser.Util;
using ShardingConnector.Common;
using ShardingConnector.Logger;
using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.Session;
using ShardingConnector.ProxyServer.TextCommandHandlers.Skip;

namespace ShardingConnector.ProxyServer.TextCommandHandlers;

public class TextCommandHandlerFactory:ITextCommandHandlerFactory
{
    // private readonly IServerConnector _serverConnector;

    private static readonly ILogger<TextCommandHandlerFactory> _logger =
        InternalLoggerFactory.CreateLogger<TextCommandHandlerFactory>();

    public TextCommandHandlerFactory(
        // IServerConnector serverConnector
        )
    {
        // _serverConnector = serverConnector;
    }
    public ITextCommandHandler Create(DatabaseTypeEnum databaseType, string sql, ISqlCommand sqlCommand,
        ConnectionSession connectionSession)
    {
        _logger.LogDebug($"database type:{databaseType},sql:{sql},sql command:{sqlCommand}");
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

        return new QueryTextCommandHandler(null,sql,connectionSession);
    }

    private ITextCommandHandler CreateDALCommandServerHandler(DALCommand dalCommand, string sql,
        ConnectionSession connectionSession)
    {
        if (dalCommand is SetCommand&&null==connectionSession.GetDatabaseName())
        {
            return new NoDatabaseTextCommandHandler();
        }

        return new GenericDatabaseTextCommandHandler();
    }

    private void CheckNotSupportCommand(ISqlCommand sqlCommand)
    {
        if (sqlCommand is DCLCommand || sqlCommand is FlushCommand || sqlCommand is MySqlShowCreateUserCommand)
        {
            throw new NotSupportedException("unsupported operation");
        }
    }
}