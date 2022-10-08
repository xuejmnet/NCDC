using System.Data.Common;
using MySqlConnector;
using NCDC.Basic.Configurations;
using NCDC.Enums;
using NCDC.Exceptions;
using NCDC.ProxyServer.Abstractions;

namespace NCDC.ProxyServer.DbProviderFactories;

public sealed class ProxyDbProviderFactory:IDbProviderFactory
{
    private readonly ShardingConfiguration _shardingConfiguration;

    public ProxyDbProviderFactory(ShardingConfiguration shardingConfiguration)
    {
        _shardingConfiguration = shardingConfiguration;
    }
    public DbProviderFactory Create()
    {
        var databaseType = _shardingConfiguration.DatabaseType;
        switch (databaseType)
        {
            case DatabaseTypeEnum.MySql: return MySqlConnectorFactory.Instance;
        }

        throw new ShardingNotSupportedException($"{databaseType}");
    }
}