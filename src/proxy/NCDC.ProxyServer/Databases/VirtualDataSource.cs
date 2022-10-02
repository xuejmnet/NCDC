using System.Collections.Concurrent;
using System.Collections.Immutable;
using NCDC.Exceptions;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Options;

namespace NCDC.ProxyServer.Databases;

public sealed class VirtualDataSource : IVirtualDataSource
{
    private readonly ShardingConfigOption _shardingConfigOption;
    private readonly IDbProviderFactory _dbProviderFactory;
    public string GetDatabaseName()
    {
        return _shardingConfigOption.DatabaseName;
    }

    public string DefaultDataSourceName { get; }
    public string DefaultConnectionString { get; }
    private readonly ConcurrentDictionary<string, IDataSource> _dataSources = new();

    public VirtualDataSource(ShardingConfigOption shardingConfigOption, IDbProviderFactory dbProviderFactory)
    {
        _shardingConfigOption = shardingConfigOption;
        _dbProviderFactory = dbProviderFactory;
        DefaultDataSourceName = shardingConfigOption.DefaultDataSourceName ??
                                throw new ArgumentNullException(
                                    $"{nameof(ShardingConfigOption)}.{nameof(shardingConfigOption.DefaultDataSourceName)}");
        DefaultConnectionString = shardingConfigOption.DefaultConnectionString ??
                                  throw new ArgumentNullException(
                                      $"{nameof(ShardingConfigOption)}.{nameof(shardingConfigOption.DefaultConnectionString)}");
        foreach (var (dataSourceName, connectionString) in shardingConfigOption.DataSources)
        {
            AddDataSource(dataSourceName, connectionString);
        }
    }

    public string GetConnectionString(string dataSourceName)
    {
        if (!_dataSources.TryGetValue(dataSourceName, out var dataSource))
        {
            throw new ShardingInvalidOperationException(
                $"{nameof(GetConnectionString)} cant get dataSourceName:[{dataSourceName}]");
        }

        return dataSource.ConnectionString;
    }

    public IReadOnlyCollection<string> GetAllDataSourceNames()
    {
        return _dataSources.Keys.ToImmutableList();
    }

    public bool IsDefault(string dataSourceName)
    {
        if (!_dataSources.TryGetValue(dataSourceName, out var dataSource))
        {
            throw new ShardingInvalidOperationException(
                $"{nameof(IsDefault)} cant get dataSourceName:[{dataSourceName}]");
        }

        return dataSource.IsDefault;
    }

    public bool AddDataSource(string dataSourceName, string connectionString)
    {
        return _dataSources.TryAdd(dataSourceName,
            new DataSource(dataSourceName, connectionString, dataSourceName == DefaultDataSourceName, _dbProviderFactory.Create()));
    }

    public bool Exists(string dataSourceName)
    {
        return _dataSources.ContainsKey(dataSourceName);
    }
}