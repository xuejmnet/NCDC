using Microsoft.Extensions.Logging;
using NCDC.CommandParser.Common.Command;
using NCDC.CommandParser.Common.Command.DAL;
using NCDC.CommandParser.Common.Command.DCL;
using NCDC.CommandParser.Common.Command.TCL;
using NCDC.CommandParser.Common.Constant;
using NCDC.CommandParser.Common.Util;
using NCDC.CommandParser.Dialect.Command.MySql.DAL;
using NCDC.Enums;
using NCDC.Logger;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Connection;
using NCDC.ProxyServer.Connection.Abstractions;
using NCDC.ProxyServer.Helpers;
using NCDC.ShardingAdoNet;
using NCDC.ShardingParser.Abstractions;

namespace NCDC.ProxyServer.ServerHandlers;

public sealed class ServerHandlerFactory : IServerHandlerFactory
{
    private static readonly ILogger<ServerHandlerFactory> _logger =
        NCDCLoggerFactory.CreateLogger<ServerHandlerFactory>();

    private readonly IServerDataReaderFactory _serverDataReaderFactory;
    private readonly ISqlCommandContextFactory _sqlCommandContextFactory;

    public ServerHandlerFactory(IServerDataReaderFactory serverDataReaderFactory,ISqlCommandContextFactory sqlCommandContextFactory)
    {
        _serverDataReaderFactory = serverDataReaderFactory;
        _sqlCommandContextFactory = sqlCommandContextFactory;
    }

    public IServerHandler Create(DatabaseTypeEnum databaseType, string sql, ISqlCommand sqlCommand,
        IConnectionSession connectionSession)
    {
        _logger.LogDebug($"database type:{databaseType},sql:{sql},sql command:{sqlCommand}");
        //取消sql的注释信息
        var trimCommentSql = SqlUtil.TrimComment(sql);
        if (string.IsNullOrEmpty(trimCommentSql))
        {
            return SkipServerHandler.Default;
        }

        CheckNotSupportCommand(sqlCommand);
        var sqlCommandContext = _sqlCommandContextFactory.Create(ParameterContext.Empty, sqlCommand);
        connectionSession.QueryContext = new QueryContext(sqlCommandContext, sql, ParameterContext.Empty);
        if (sqlCommand is ITCLCommand tclCommand)
        {
            return CreateTCLCommandServerHandler(tclCommand,connectionSession);
        }

        if (sqlCommand is IDALCommand dalCommand)
        {
            return CreateDALCommandServerHandler(dalCommand, connectionSession);
        }


        return new QueryServerHandler(connectionSession, _serverDataReaderFactory);
    }

    private IServerHandler CreateDALCommandServerHandler(IDALCommand dalCommand,
        IConnectionSession connectionSession)
    {
        if (dalCommand is MySqlUseCommand useCommand)
        {
            return SkipServerHandler.Default;
        }

        if (dalCommand is SetCommand && null == connectionSession.DatabaseName)
        {
            return SkipServerHandler.Default;
        }

        return new UnicastServerHandler(connectionSession, _serverDataReaderFactory);
    }

    private IServerHandler CreateTCLCommandServerHandler(ITCLCommand tclCommand,IConnectionSession connectionSession)
    {
        if (tclCommand is BeginTransactionCommand beginTransactionCommand)
        {
            return new TransactionServerHandler(TransactionOperationTypeEnum.BEGIN, connectionSession);
        }

        if (tclCommand is SetAutoCommitCommand setAutoCommitCommand)
        {
            if (setAutoCommitCommand.AutoCommit)
            {
                return connectionSession.GetTransactionStatus().IsInTransaction()
                    ? new TransactionServerHandler(TransactionOperationTypeEnum.COMMIT, connectionSession)
                    : SkipServerHandler.Default;
            }

            throw new NotSupportedException("SetAutoCommitCommand");
        }

        if (tclCommand is CommitCommand commitCommand)
        {
            return new TransactionServerHandler(TransactionOperationTypeEnum.COMMIT, connectionSession);
        }

        if (tclCommand is RollbackCommand rollbackCommand)
        {
            return new TransactionServerHandler(TransactionOperationTypeEnum.ROLLBACK, connectionSession);
        }
        if (tclCommand is SetTransactionCommand setTransactionCommand &&
            OperationScopeEnum.GLOBAL != setTransactionCommand.Scope)
        {
            return new TransactionSetServerHandler(setTransactionCommand, connectionSession);
        }

        return new UnicastServerHandler(connectionSession, _serverDataReaderFactory);
    }

    private void CheckNotSupportCommand(ISqlCommand sqlCommand)
    {
        if (sqlCommand is IDCLCommand)
        {
            throw new NotSupportedException("unsupported operation");
        }

        if (sqlCommand is MySqlShowDatabasesCommand)
        {
            throw new NotSupportedException("show databases");
        }
    }
}