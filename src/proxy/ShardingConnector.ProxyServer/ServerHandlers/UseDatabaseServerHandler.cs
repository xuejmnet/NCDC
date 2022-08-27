using ShardingConnector.CommandParser.Command.DAL.Dialect.MySql;
using ShardingConnector.CommandParser.Util;
using ShardingConnector.Exceptions;
using ShardingConnector.ProxyServer.Abstractions;
using ShardingConnector.ProxyServer.ServerHandlers.Results;
using ShardingConnector.ProxyServer.Session;

namespace ShardingConnector.ProxyServer.ServerHandlers;

public sealed class UseDatabaseServerHandler:IServerHandler
{
    private readonly UseCommand _useCommand;
    private readonly ConnectionSession _connectionSession;

    public UseDatabaseServerHandler( UseCommand useCommand,ConnectionSession connectionSession)
    {
        _useCommand = useCommand;
        _connectionSession = connectionSession;
    }
    public IServerResult Execute()
    {
        var database = SqlUtil.GetExactlyValue(_useCommand.GetSchema());
        if (IsAuthorized(database))
        {
            return RecordsAffectedServerResult.Empty;
        }

        throw new ShardingException($"unknown database {database}");
    }

    private bool IsAuthorized(string database)
    {
        if (ProxyRuntimeContext.Instance.DatabaseExists(database))
        {
            var logicDatabase = ProxyRuntimeContext.Instance.GetDatabase(database);
            return logicDatabase!.UserNameAuthorize(_connectionSession.GetGrantee().Username);
        }

        return false;
    }
}