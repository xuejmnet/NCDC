using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NCDC.EntityFrameworkCore.Entities;
using NCDC.ProxyServer.Configurations;
using NCDC.ProxyServer.Connection.User;

namespace NCDC.EntityFrameworkCore.Configurations;

public class DbAppAuthUserBuilder:IAppAuthUserBuilder
{
    private readonly IServiceProvider _serviceProvider;

    public DbAppAuthUserBuilder(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public async Task<IReadOnlyList<AuthUser>> BuildAsync()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<NCDCDbContext>();
            var authUserEntities = await dbContext.Set<AppAuthUserEntity>().Where(o=>o.IsEnable).ToListAsync();
            return authUserEntities.Select(o => new AuthUser(o.UserName, o.Password, o.HostName)).ToImmutableList();
        }
    }
}