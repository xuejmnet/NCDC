using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using ShardingConnector.Base;
using ShardingConnector.Extensions;
using ShardingConnector.Logger;
using ShardingConnector.ProxyServer.Commons;
using ShardingConnector.ProxyServer.DatabaseInfo;
using ShardingConnector.ProxyServer.Options;
using ShardingConnector.ShardingCommon.User;

namespace ShardingConnector.ProxyServer;

public sealed class ProxyRuntimeContext
{
    private static readonly ILogger<ProxyRuntimeContext> _logger =
        InternalLoggerFactory.CreateLogger<ProxyRuntimeContext>();
    public static ProxyRuntimeContext Instance { get; } = new ProxyRuntimeContext();
    private readonly ConcurrentDictionary<string/*logic database name*/,LogicDatabase> _logicDatabase=new ();
    private readonly ConcurrentDictionary<string/*user name*/,ShardingConnectorUser> _connectorUsers=new ();

    public void Init(ProxyRuntimeOption proxyRuntimeOption)
    {
        if (proxyRuntimeOption.Databases.IsEmpty())
        {
            _logger.LogWarning("not found any databases");
            return;
        }

    
        foreach (var databaseOption in proxyRuntimeOption.Databases)
        {
            InitDatabase(databaseOption);
        }
        if (proxyRuntimeOption.Users.IsNotEmpty())
        {
            foreach (var userOption in proxyRuntimeOption.Users)
            {
                InitConnectorUser(userOption);
            }
        }
    }

    private void InitConnectorUser(UserOption userOption)
    {
        if (_connectorUsers.ContainsKey(userOption.Username))
        {
            throw new ArgumentException("user name repeat");
        }

        var shardingConnectorUser = new ShardingConnectorUser(userOption.Username, userOption.Password, null);
        if (userOption.Databases.IsNotEmpty())
        {
            foreach (var database in userOption.Databases)
            {
                shardingConnectorUser.AuthorizeDatabases.TryAdd(database, null);
                //如果数据源存在那么就添加到数据源中
                if (_logicDatabase.TryGetValue(database,out var logicDatabase))
                {
                    logicDatabase.AddConnectorUser(userOption.Username);
                }
            }
        }
        _connectorUsers.TryAdd(userOption.Username,shardingConnectorUser);
    }

    private void InitDatabase(DatabaseOption databaseOption)
    {
        var logicDatabase = new LogicDatabase(databaseOption.Name);
        if (!_logicDatabase.TryAdd(logicDatabase.Database, logicDatabase))
        {
            throw new ArgumentException("database repeat");
        }

        var defaultCount = databaseOption.DataSources.Where(o=>o.IsDefault).Take(2).Count();
        ShardingAssert.If(defaultCount == 0,$"database:{logicDatabase.Database},not found default data source");
        ShardingAssert.If(defaultCount == 2,$"database:{logicDatabase.Database},default data source repeat");
        if (databaseOption.DataSources.IsNotEmpty())
        {
            foreach (var dataSource in databaseOption.DataSources)
            {
                logicDatabase.AddDataSource(dataSource.DataSourceName, dataSource.ConnectionString,
                    dataSource.IsDefault);
            }
        }
    }
    
    public bool DatabaseExists(string database)
    {
        return _logicDatabase.ContainsKey(database);
    }

    public LogicDatabase? GetDatabase(string database)
    {
        _logicDatabase.TryGetValue(database, out var logicDatabase);
        return logicDatabase;
    }

    public ShardingConnectorUser? GetUser(string username)
    {
        _connectorUsers.TryGetValue(username, out var shardingConnectorUser);
        return shardingConnectorUser;
    }
}