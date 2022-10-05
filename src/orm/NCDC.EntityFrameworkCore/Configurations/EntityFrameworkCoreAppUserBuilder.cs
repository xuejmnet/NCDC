using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NCDC.EntityFrameworkCore.Entities;
using NCDC.ProxyServer.Configurations.Apps;
using NCDC.ProxyServer.Connection.User;
using NCDC.ProxyServer.Contexts;

namespace NCDC.EntityFrameworkCore.Configurations;

public class EntityFrameworkCoreAppUserBuilder:IAppUserBuilder
{
    private readonly IServiceProvider _appServiceProvider;

    public EntityFrameworkCoreAppUserBuilder(IServiceProvider appServiceProvider)
    {
        _appServiceProvider = appServiceProvider;
    }
    public async Task<IReadOnlyCollection<AuthUser>> BuildAsync()
    {
        using (var scope = _appServiceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<NCDCDbContext>();
            var appAuthUsers = await dbContext.Set<AppAuthUserEntity>().Where(o=>o.IsEnable).ToListAsync();
            return appAuthUsers.Select(o => new AuthUser(o.UserName, o.Password, o.HostName)).ToImmutableList();
        }
    }
}