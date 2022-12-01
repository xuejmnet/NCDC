using System.Data;
using DotNetty.Transport.Channels;
using NCDC.Basic.User;
using NCDC.CommandParser.Abstractions;
using NCDC.Enums;
using NCDC.ProxyServer.AppServices.Abstractions;
using NCDC.ProxyServer.Connection.Abstractions;

namespace NCDC.ProxyServer.Connection;

public class DefaultConnectionSessionFactory:IConnectionSessionFactory
{
    private readonly IAppRuntimeManager _appRuntimeManager;
    private readonly ISqlCommandParser _sqlCommandParser;

    public DefaultConnectionSessionFactory(IAppRuntimeManager appRuntimeManager,ISqlCommandParser sqlCommandParser)
    {
        _appRuntimeManager = appRuntimeManager;
        _sqlCommandParser = sqlCommandParser;
    }

    public IConnectionSession Create(int connectionId,string database, Grantee grantee, IChannel channel)
    {
        return new DefaultConnectionSession(connectionId,grantee,TransactionTypeEnum.LOCAL,IsolationLevel.RepeatableRead, channel,_sqlCommandParser,_appRuntimeManager.GetRuntimeContext(database));
    }
}