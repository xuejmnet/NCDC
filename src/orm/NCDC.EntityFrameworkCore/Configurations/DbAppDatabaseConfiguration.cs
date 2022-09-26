using NCDC.ProxyServer.Abstractions;

namespace NCDC.EntityFrameworkCore.Configurations;

public sealed class DbAppDatabaseConfiguration:IAppDatabaseConfiguration
{
    private readonly IServiceProvider _serviceProvider;

    public DbAppDatabaseConfiguration(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public IReadOnlyCollection<string> GetDatabases()
    {
        //todo 数据库获取所有的数据源
        throw new NotImplementedException();
    }
}