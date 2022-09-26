using System.Data.Common;
using MySqlConnector;
using NCDC.Enums;
using NCDC.Exceptions;
using NCDC.ProxyServer.Abstractions;

namespace NCDC.ProxyServer.DbProviderFactories;

public sealed class ProxyDbProviderFactory:IDbProviderFactory
{
    private readonly IAppConfiguration _appConfiguration;

    public ProxyDbProviderFactory(IAppConfiguration appConfiguration)
    {
        _appConfiguration = appConfiguration;
    }
    public DbProviderFactory Create()
    {
        var databaseType = _appConfiguration.GetDatabaseType();
        switch (databaseType)
        {
            case DatabaseTypeEnum.MySql: return MySqlConnectorFactory.Instance;
        }

        throw new ShardingNotSupportedException($"{databaseType}");
    }
}