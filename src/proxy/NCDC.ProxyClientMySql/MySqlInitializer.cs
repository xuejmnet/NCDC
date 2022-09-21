using NCDC.Exceptions;
using NCDC.Extensions;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Connection.User;
using NCDC.ProxyServer.Contexts;
using NCDC.ProxyServer.Options;

namespace NCDC.ProxyClientMySql;

public sealed class MySqlInitializer:IInitializer
{
    private readonly IContextManager _contextManager;
    private readonly NCDCConfigOption _ncdcConfigOption;
    private readonly IUserManager _userManager;

    public MySqlInitializer(IContextManager contextManager,NCDCConfigOption ncdcConfigOption,IUserManager userManager)
    {
        _contextManager = contextManager;
        _ncdcConfigOption = ncdcConfigOption;
        _userManager = userManager;
    }
    public async Task InitializeAsync()
    {
        _ncdcConfigOption.CheckOptionCompleteness();
        var allDatabases = _ncdcConfigOption.Databases.Select(o=>o.DatabaseName).ToHashSet();
        foreach (var userOption in _ncdcConfigOption.Users)
        {
            foreach (var userDatabaseNameOption in userOption.DatabaseNames)
            {
                if (!allDatabases.Contains(userDatabaseNameOption))
                {
                    throw new ShardingException(
                        $"user's {userOption.UserName} config unknown database:{userDatabaseNameOption}");
                }
            }
            _userManager.AddUser(new AuthUser())
        }
    }
}