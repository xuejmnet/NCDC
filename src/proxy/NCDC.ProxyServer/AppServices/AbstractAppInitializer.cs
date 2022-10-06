using Microsoft.Extensions.Logging;
using NCDC.Logger;
using NCDC.ProxyServer.AppServices.Builder;
using NCDC.ProxyServer.Configurations.Apps;
using NCDC.ProxyServer.Connection.User;

namespace NCDC.ProxyServer.AppServices;

public abstract class AbstractAppInitializer:IAppInitializer
{
    private readonly ILogger<AbstractAppInitializer> _logger =
        InternalNCDCLoggerFactory.CreateLogger<AbstractAppInitializer>();
    private readonly IAppRuntimeLoader _appRuntimeLoader;
    private readonly IAppUserLoader _appUserLoader;
    private readonly IUserDatabaseMappingLoader _userDatabaseMappingLoader;
    private readonly IAppRuntimeBuilder _appRuntimeBuilder;

    public AbstractAppInitializer(IAppRuntimeLoader appRuntimeLoader,IAppUserLoader appUserLoader,IUserDatabaseMappingLoader userDatabaseMappingLoader,IAppRuntimeBuilder appRuntimeBuilder)
    {
        _appRuntimeLoader = appRuntimeLoader;
        _appUserLoader = appUserLoader;
        _userDatabaseMappingLoader = userDatabaseMappingLoader;
        _appRuntimeBuilder = appRuntimeBuilder;
    }
    public async Task InitializeAsync()
    {
        var authUsers =await GetAuthUsersAsync();
        foreach (var authUser in authUsers)
        {
            _appUserLoader.AddAppUser(authUser);
        }

        var userDatabases = await GetUserDatabasesAsync();
        foreach (var userDatabaseEntry in userDatabases)
        {
            _userDatabaseMappingLoader.AddUserDatabaseMapping(userDatabaseEntry);
        }
        var runtimes = await GetRuntimesAsync();
        foreach (var database in runtimes)
        {
            var runtimeContext = await _appRuntimeBuilder.BuildAsync(database);
            if (!_appRuntimeLoader.LoadRuntimeContext(runtimeContext))
            {
                _logger.LogWarning($"repeat load runtime:{runtimeContext.DatabaseName}");
            }
        }
    }

    protected abstract Task<IReadOnlyCollection<string>> GetRuntimesAsync();
    protected abstract Task<IReadOnlyCollection<AuthUser>> GetAuthUsersAsync();
    protected abstract Task<IReadOnlyCollection<UserDatabaseEntry>> GetUserDatabasesAsync();
}