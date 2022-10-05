using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NCDC.EntityFrameworkCore.Entities;
using NCDC.Exceptions;
using NCDC.ProxyServer.AppServices;
using NCDC.ProxyServer.AppServices.Builder;
using NCDC.ProxyServer.Runtimes;

namespace NCDC.EntityFrameworkCore.Impls;

public class EntityFrameworkCoreAppRuntimeInitializer:AbstractAppRuntimeInitializer
{
    private readonly IServiceProvider _appServiceProvider;

    public EntityFrameworkCoreAppRuntimeInitializer(IServiceProvider appServiceProvider,IAppRuntimeLoader appRuntimeLoader, IAppRuntimeBuilder appRuntimeBuilder) : base(appRuntimeLoader, appRuntimeBuilder)
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
}