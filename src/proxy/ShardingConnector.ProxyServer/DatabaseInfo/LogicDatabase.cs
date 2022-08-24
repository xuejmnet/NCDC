using System.Collections.Concurrent;
using ShardingConnector.ShardingCommon.User;

namespace ShardingConnector.ProxyServer.DatabaseInfo;

public sealed class LogicDatabase
{
    public string Database { get; }
    private readonly ConcurrentDictionary<string /*data source name*/, ProxyDatabase> _proxyDatabases = new();

    private readonly ConcurrentDictionary<string/*username*/, object?> _connectorUsers = new();
    public string DefaultDataSourceName { get; private set; }
    public string DefaultConnectionString { get; private set; }

    public LogicDatabase( string database)
    {
        Database = database;
    }

    public bool AddDataSource(string dataSourceName, string connectionString, bool isDefault)
    {
        if (isDefault)
        {
            if (_proxyDatabases.Values.Any(o => o.IsDefault))
                throw new ArgumentException($"{Database}:multi default data source");
            DefaultDataSourceName = dataSourceName;
            DefaultConnectionString = connectionString;
        }

        if (_proxyDatabases.TryGetValue(dataSourceName, out var proxyDatabase))
        {
            throw new ArgumentException($"{Database}:datasource name repeat");
        }

        return _proxyDatabases.TryAdd(dataSourceName, new ProxyDatabase(dataSourceName, connectionString, isDefault));
    }

    public bool AddConnectorUser(string username)
    {
        return _connectorUsers.TryAdd(username, null);
    }

    public bool UserNameAuthorize(string username)
    {
        return _connectorUsers.ContainsKey(username);
    }
    
}