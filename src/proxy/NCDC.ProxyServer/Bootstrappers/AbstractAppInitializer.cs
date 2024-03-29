using Microsoft.Extensions.Logging;
using NCDC.Basic.User;
using NCDC.Logger;
using NCDC.ProxyServer.AppServices;
using NCDC.ProxyServer.AppServices.Abstractions;
using NCDC.ProxyServer.Runtimes.Builder;

namespace NCDC.ProxyServer.Bootstrappers;

public abstract class AbstractAppInitializer:IAppInitializer
{
    private readonly ILogger<AbstractAppInitializer> _logger =
        NCDCLoggerFactory.CreateLogger<AbstractAppInitializer>();
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
        await PreInitializeAsync();
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
            if (runtimeContext is null)
                continue;
            
            if (!_appRuntimeLoader.LoadRuntimeContext(runtimeContext))
            {
                _logger.LogWarning($"repeat load runtime:{runtimeContext.DatabaseName}");
            }
        }

        await PostInitializeAsync();
    }

    protected virtual Task PreInitializeAsync()
    {
        return Task.CompletedTask;
    }

    protected virtual Task PostInitializeAsync()
    {
        return Task.CompletedTask;
    }

    protected abstract Task<IReadOnlyList<string>> GetRuntimesAsync();
    protected abstract Task<IReadOnlyList<AuthUser>> GetAuthUsersAsync();
    protected abstract Task<IReadOnlyList<UserDatabaseEntry>> GetUserDatabasesAsync();
}