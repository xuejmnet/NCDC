using NCDC.CommandParser.Command.DAL.Dialect.MySql;
using NCDC.CommandParser.Util;
using OpenConnector.Configuration.Session;
using OpenConnector.Exceptions;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.ServerHandlers.Results;
using NCDC.ProxyServer.Session;

namespace NCDC.ProxyServer.ServerHandlers;

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