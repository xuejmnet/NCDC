using Microsoft.Extensions.Logging;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.CommandParser.Command.DAL.Dialect;
using ShardingConnector.CommandParser.Command.DAL.Dialect.MySql;
using ShardingConnector.CommandParser.Command.DCL;
using ShardingConnector.CommandParser.Command.TCL;
using ShardingConnector.CommandParser.Util;
using ShardingConnector.Common;
using ShardingConnector.Logger;
using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyServer.ServerHandlers;

public sealed class ServerHandlerFactory:IServerHandlerFactory
{
    private static readonly ILogger<ServerHandlerFactory> _logger = InternalLoggerFactory.CreateLogger<ServerHandlerFactory>();
    
    public IServerHandler Create(DatabaseTypeEnum databaseType, string sql, ISqlCommand sqlCommand,
        ConnectionSession connectionSession)
    {
        _logger.LogDebug($"database type:{databaseType},sql:{sql},sql command:{sqlCommand}");
        //取消sql的注释信息
        var trimCommentSql = SqlUtil.TrimComment(sql);
        if (string.IsNullOrEmpty(trimCommentSql))
        {
            return new SkipServerHandler();
        }

        CheckNotSupportCommand(sqlCommand);
        if (sqlCommand is TCLCommand tclCommand)
        {
            return CreateTCLCommandServerHandler(tclCommand,sql, connectionSession);
        }
        if (sqlCommand is DALCommand dalCommand)
        {
            return CreateDALCommandServerHandler(dalCommand, sql,connectionSession);
        }

        return new QueryServerHandler(sql,connectionSession);
    }
    private IServerHandler CreateDALCommandServerHandler(DALCommand dalCommand, string sql,
        ConnectionSession connectionSession)
    {
        if (dalCommand is SetCommand)
        {
            return new NoDatabaseServerHandler();
        }

        return new GenericDatabaseServerHandler();
    }

    private IServerHandler CreateTCLCommandServerHandler(TCLCommand tclCommand, string sql,
        ConnectionSession connectionSession)
    {
        if (tclCommand is BeginTransactionCommand beginTransactionCommand)
        {
            return new TransactionServerHandler(TransactionOperationTypeEnum.BEGIN, connectionSession);
        }

        if (tclCommand is SetAutoCommitCommand setAutoCommitCommand)
        {
            throw new NotSupportedException("SetAutoCommitCommand");
        }

        if (tclCommand is CommitCommand commitCommand)
        {
            throw new NotSupportedException("CommitCommand");
        }

        if (tclCommand is RollbackCommand rollbackCommand)
        {
            throw new NotSupportedException("RollbackCommand");
        }
        //todo 判断设置隔离级别

        throw new NotSupportedException(tclCommand.GetType().FullName);
    }

    private void CheckNotSupportCommand(ISqlCommand sqlCommand)
    {
        if (sqlCommand is DCLCommand || sqlCommand is FlushCommand || sqlCommand is MySqlShowCreateUserCommand)
        {
            throw new NotSupportedException("unsupported operation");
        }
    }
}