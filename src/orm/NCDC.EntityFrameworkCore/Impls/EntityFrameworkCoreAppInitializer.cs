using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NCDC.EntityFrameworkCore.Entities;
using NCDC.ProxyServer.AppServices;
using NCDC.ProxyServer.AppServices.Builder;
using NCDC.ProxyServer.Connection.User;

namespace NCDC.EntityFrameworkCore.Impls;

public class EntityFrameworkCoreAppInitializer:AbstractAppInitializer
{
    private readonly IServiceProvider _appServiceProvider;

    public EntityFrameworkCoreAppInitializer(IServiceProvider appServiceProvider,IAppRuntimeLoader appRuntimeLoader, IAppUserLoader appUserLoader, IUserDatabaseMappingLoader userDatabaseMappingLoader, IAppRuntimeBuilder appRuntimeBuilder) : base(appRuntimeLoader, appUserLoader, userDatabaseMappingLoader, appRuntimeBuilder)
    {
        _appServiceProvider = appServiceProvider;
    }

    protected override async Task<IReadOnlyCollection<string>> GetRuntimesAsync()
    {
        using (var scope = _appServiceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<NCDCDbContext>();
            var logicDatabases = await dbContext.Set<LogicDatabaseEntity>().Select(o=>o.Name).ToListAsync();
            return logicDatabases.AsReadOnly();
        }
    }

    protected override async Task<IReadOnlyCollection<AuthUser>> GetAuthUsersAsync()
    {
        using (var scope = _appServiceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<NCDCDbContext>();
            var appAuthUsers = await dbContext.Set<AppAuthUserEntity>().Where(o=>o.IsEnable).Select(o=>new AuthUser(o.UserName,o.Password,o.HostName)).ToListAsync();
            return appAuthUsers.AsReadOnly();
        }
    }

    protected override async Task<IReadOnlyCollection<UserDatabaseEntry>> GetUserDatabasesAsync()
    {
        using (var scope = _appServiceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<NCDCDbContext>();
            var appAuthUsers = await dbContext.Set<LogicDatabaseUserEntity>().Select(o=>new UserDatabaseEntry(o.UserName,o.Database)).ToListAsync();
            return appAuthUsers.AsReadOnly();
        }
    }
}