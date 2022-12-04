using System.Collections.Concurrent;
using System.Collections.Immutable;
using Microsoft.Extensions.Logging;
using NCDC.Basic.Configurations;
using NCDC.Enums;
using NCDC.Exceptions;
using NCDC.Logger;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Connection;
using NCDC.ProxyServer.Connection.Abstractions;
using NCDC.ProxyServer.Options;

namespace NCDC.ProxyServer.Databases;

public sealed class VirtualDataSource : IVirtualDataSource
{
    private static ILogger<VirtualDataSource> _logger = NCDCLoggerFactory.CreateLogger<VirtualDataSource>();
    private readonly ShardingConfiguration _shardingConfiguration;
    private readonly IDbProviderFactory _dbProviderFactory;

    public string GetDatabaseName()
    {
        return _shardingConfiguration.DatabaseName;
    }

    public string DefaultDataSourceName { get; }
    public string DefaultConnectionString { get; }
    private readonly ConcurrentDictionary<string, IDataSource> _dataSources = new();

    public VirtualDataSource(ShardingConfiguration shardingConfiguration, IDbProviderFactory dbProviderFactory)
    {
        _shardingConfiguration = shardingConfiguration;
        _dbProviderFactory = dbProviderFactory;
        DefaultDataSourceName = shardingConfiguration.DefaultDataSourceName ??
                                throw new ArgumentNullException(
                                    $"{nameof(ShardingConfiguration)}.{nameof(shardingConfiguration.DefaultDataSourceName)}");
        DefaultConnectionString = shardingConfiguration.DefaultConnectionString ??
                                  throw new ArgumentNullException(
                                      $"{nameof(ShardingConfiguration)}.{nameof(shardingConfiguration.DefaultConnectionString)}");
        foreach (var (dataSourceName, connectionString) in shardingConfiguration.DataSources)
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

    public ICollection<string> GetAllDataSourceNames()
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
            new DataSource(dataSourceName, connectionString, dataSourceName == DefaultDataSourceName,
                _dbProviderFactory.Create()));
    }

    public bool Exists(string dataSourceName)
    {
        return _dataSources.ContainsKey(dataSourceName);
    }

    public IDataSource GetDataSource(string dataSourceName)
    {
        Check.NotNull(dataSourceName, nameof(dataSourceName));
        if (!_dataSources.TryGetValue(dataSourceName, out var dataSource))
        {
            throw new ShardingException($"data source not found:[{dataSourceName}]");
        }

        return dataSource;
    }

    public async ValueTask<List<IServerDbConnection>> GetServerDbConnectionsAsync(ConnectionModeEnum connectionMode,
        string dataSourceName, int connectionSize,
        TransactionTypeEnum transactionType)
    {
        if (!_dataSources.TryGetValue(dataSourceName, out var dataSource))
        {
            throw new ShardingException($"unknown data source name:{dataSourceName}");
        }

        if (1 == connectionSize)
        {
            var serverDbConnection = await CreateServerDbConnection(transactionType, dataSource);
            return new List<IServerDbConnection>()
                { serverDbConnection };
        }

        // if (ConnectionModeEnum.CONNECTION_STRICTLY == connectionMode)
        // {
        //     return CreateServerDbConnections(transactionType, dataSourceName, proxyDatabase, connectionSize);
        // }
        //
        // lock (proxyDatabase)
        // {
        return await CreateServerDbConnections(transactionType, dataSource, connectionSize);
        // }
    }

    // public SchemaMetaData SchemaMetaData { get; }

    private async ValueTask<List<IServerDbConnection>> CreateServerDbConnections(TransactionTypeEnum transactionType,
        IDataSource dataSource, int connectionSize)
    {
        var serverDbConnections = new List<IServerDbConnection>(connectionSize);
        for (int i = 0; i < connectionSize; i++)
        {
            try
            {
                serverDbConnections.Add(await CreateServerDbConnection(transactionType, dataSource));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(CreateServerDbConnections)} has error.");
                foreach (var serverDbConnection in serverDbConnections)
                {
                    serverDbConnection.Dispose();
                }

                throw new ShardingException(
                    $"couldn't get {connectionSize} server db connections one time, partition succeed connection({serverDbConnections.Count}) have released!",
                    ex);
            }
        }

        return serverDbConnections;
    }

    private ValueTask<IServerDbConnection> CreateServerDbConnection(TransactionTypeEnum transactionType,
        IDataSource dataSource)
    {
        return dataSource.CreateServerDbConnectionAsync();
    }
}