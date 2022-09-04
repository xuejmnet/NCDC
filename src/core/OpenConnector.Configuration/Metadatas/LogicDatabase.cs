using System.Collections.Concurrent;
using System.Data.Common;
using Microsoft.Extensions.Logging;
using OpenConnector.DataSource;
using OpenConnector.Exceptions;
using OpenConnector.Logger;
using OpenConnector.ProxyServer.Session.Connection.Abstractions;
using OpenConnector.Transaction;

namespace OpenConnector.Configuration.Metadatas;

public class LogicDatabase:ILogicDatabase
{
    private static ILogger<LogicDatabase> _logger = InternalLoggerFactory.CreateLogger<LogicDatabase>();
    public string Name { get; }
    private readonly ConcurrentDictionary<string /*data source name*/, IProxyDatabase> _proxyDatabases = new();

    private readonly ConcurrentDictionary<string/*username*/, object?> _connectorUsers = new();

    public LogicDatabase(string name)
    {
        Name = name;
    }

    public string DefaultDataSourceName { get; private set; }
    public string DefaultConnectionString { get; private set; }


    public bool AddDataSource(string dataSourceName, string connectionString,DbProviderFactory dbProviderFactory, bool isDefault)
    {
        if (isDefault)
        {
            if (_proxyDatabases.Values.Any(o => o.IsDefault))
                throw new ArgumentException($"{Name}:multi default data source");
            DefaultDataSourceName = dataSourceName;
            DefaultConnectionString = connectionString;
        }

        if (_proxyDatabases.TryGetValue(dataSourceName, out var proxyDatabase))
        {
            throw new ArgumentException($"{Name}:datasource name repeat");
        }

        var genericDataSource = new GenericDataSource(dataSourceName,dbProviderFactory,connectionString, isDefault);
        return _proxyDatabases.TryAdd(dataSourceName, new ProxyDatabase(genericDataSource));
    }

    public bool AddConnectorUser(string username)
    {
        return _connectorUsers.TryAdd(username, null);
    }

    public bool UserNameAuthorize(string username)
    {
        return _connectorUsers.ContainsKey(username);
    }

    public List<IServerDbConnection> GetServerDbConnections(ConnectionModeEnum connectionMode, string dataSourceName,
        int connectionSize, TransactionTypeEnum transactionType)
    {
        if (!_proxyDatabases.TryGetValue(dataSourceName, out var proxyDatabase))
        {
            throw new ShardingException($"unknown data source name:{dataSourceName}");
        }

        if (1 == connectionSize)
        {
            return new List<IServerDbConnection>()
                { CreateServerDbConnection(transactionType, dataSourceName, proxyDatabase) };
        }

        // if (ConnectionModeEnum.CONNECTION_STRICTLY == connectionMode)
        // {
        //     return CreateServerDbConnections(transactionType, dataSourceName, proxyDatabase, connectionSize);
        // }
        //
        // lock (proxyDatabase)
        // {
            return CreateServerDbConnections(transactionType, dataSourceName, proxyDatabase, connectionSize);
        // }
    }

    private List<IServerDbConnection> CreateServerDbConnections(TransactionTypeEnum transactionType,string dataSourceName,IProxyDatabase proxyDatabase,int connectionSize)
    {
        var serverDbConnections = new List<IServerDbConnection>(connectionSize);
        for (int i = 0; i < connectionSize; i++)
        {
            try
            {
                serverDbConnections.Add(CreateServerDbConnection(transactionType,dataSourceName,proxyDatabase));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,$"{nameof(CreateServerDbConnections)} has error.");
                foreach (var serverDbConnection in serverDbConnections)
                {
                    serverDbConnection.Dispose();
                }

                throw new ShardingException($"couldn't get {connectionSize} server db connections one time, partition succeed connection({serverDbConnections.Count}) have released!",ex);
            }
        }

        return serverDbConnections;
    }

    private IServerDbConnection CreateServerDbConnection(TransactionTypeEnum transactionType, string dataSourceName,
        IProxyDatabase proxyDatabase)
    {
        return proxyDatabase.CreateServerDbConnection();
    }
    
}