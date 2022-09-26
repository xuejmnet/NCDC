using NCDC.ProxyServer.Configurations;
using NCDC.ProxyServer.Options;

namespace NCDC.EntityFrameworkCore.Configurations;

public sealed class DbShardingConfigOptionBuilder:IShardingConfigOptionBuilder
{
    private readonly IServiceProvider _serviceProvider;

    public DbShardingConfigOptionBuilder(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public ShardingConfigOption Build(string databaseName)
    {
        //todo 查询数据库获取databaseName的配置
        throw new NotImplementedException();
    }
}