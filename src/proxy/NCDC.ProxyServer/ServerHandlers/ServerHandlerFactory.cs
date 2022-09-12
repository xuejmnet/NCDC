using Microsoft.Extensions.Logging;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Command;
using NCDC.CommandParser.Command.DAL.Dialect;
using NCDC.CommandParser.Command.DAL.Dialect.MySql;
using NCDC.CommandParser.Command.DCL;
using NCDC.CommandParser.Command.TCL;
using NCDC.CommandParser.Util;
using NCDC.Common;
using OpenConnector.Configuration.Session;
using OpenConnector.Enums;
using OpenConnector.Logger;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Session;

namespace NCDC.ProxyServer.ServerHandlers;

public sealed class ServerHandlerFactory:IServerHandlerFactory
{
    private static readonly ILogger<ServerHandlerFactory> _logger = InternalLoggerFactory.CreateLogger<ServerHandlerFactory>();
    private readonly IServerDataReaderFactory _serverDataReaderFactory;

    public ServerHandlerFactory(IServerDataReaderFactory serverDataReaderFactory)
    {
        _serverDataReaderFactory = serverDataReaderFactory;
    }
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

        return new QueryServerHandler(sql,connectionSession,_serverDataReaderFactory);
    }
    private IServerHandler CreateDALCommandServerHandler(DALCommand dalCommand, string sql,
        ConnectionSession connectionSession)
    {
        if (dalCommand is UseCommand useCommand)
        {
            return new UseDatabaseServerHandler(useCommand, connectionSession);
        }

        if (dalCommand is ShowDatabasesCommand)
        {
            return new ShowDatabasesServerHandler(connectionSession);
        }

        return new UnicastServerHandler(sql, connectionSession, _serverDataReaderFactory);
    }

    private IServerHandler CreateTCLCommandServerHandler(TCLCommand tclCommand, string sql,
        ConnectionSession connectionSession)
    {
        if (tclCommand is BeginTransactionCommand beginTransactionCommand)
        {
            throw new NotSupportedException("BeginTransactionCommand");
            // return new TransactionServerHandler(TransactionOperationTypeEnum.BEGIN, connectionSession);
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

        return new UnicastServerHandler(sql,connectionSession,_serverDataReaderFactory);
        // throw new NotSupportedException(tclCommand.GetType().FullName);
    }

    private void CheckNotSupportCommand(ISqlCommand sqlCommand)
    {
        if (sqlCommand is DCLCommand || sqlCommand is FlushCommand || sqlCommand is MySqlShowCreateUserCommand)
        {
            throw new NotSupportedException("unsupported operation");
        }
    }
}