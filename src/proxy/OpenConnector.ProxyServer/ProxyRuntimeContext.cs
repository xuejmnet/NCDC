using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using OpenConnector.Base;
using OpenConnector.Configuration.Metadatas;
using OpenConnector.Configuration.User;
using OpenConnector.Extensions;
using OpenConnector.Logger;
using OpenConnector.ProxyServer.Options;

namespace OpenConnector.ProxyServer;

public sealed class ProxyRuntimeContext
{
    private static readonly ILogger<ProxyRuntimeContext> _logger =
        InternalLoggerFactory.CreateLogger<ProxyRuntimeContext>();
    public static ProxyRuntimeContext Instance { get; } = new ProxyRuntimeContext();
    private readonly ConcurrentDictionary<string/*logic database name*/,LogicDatabase> _logicDatabase=new ();
    private readonly ConcurrentDictionary<string/*user name*/,OpenConnectorUser> _connectorUsers=new ();

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

        var OpenConnectorUser = new OpenConnectorUser(userOption.Username, userOption.Password, null);
        if (userOption.Databases.IsNotEmpty())
        {
            foreach (var database in userOption.Databases)
            {
                OpenConnectorUser.AuthorizeDatabases.TryAdd(database, null);
                //如果数据源存在那么就添加到数据源中
                if (_logicDatabase.TryGetValue(database,out var logicDatabase))
                {
                    logicDatabase.AddConnectorUser(userOption.Username);
                }
            }
        }
        _connectorUsers.TryAdd(userOption.Username,OpenConnectorUser);
    }

    private void InitDatabase(DatabaseOption databaseOption)
    {
        // var logicDatabase = new LogicDatabase(databaseOption.Name);
        // if (!_logicDatabase.TryAdd(logicDatabase.Database, logicDatabase))
        // {
        //     throw new ArgumentException("database repeat");
        // }
        //
        // var defaultCount = databaseOption.DataSources.Where(o=>o.IsDefault).Take(2).Count();
        // ShardingAssert.If(defaultCount == 0,$"database:{logicDatabase.Database},not found default data source");
        // ShardingAssert.If(defaultCount == 2,$"database:{logicDatabase.Database},default data source repeat");
        // if (databaseOption.DataSources.IsNotEmpty())
        // {
        //     foreach (var dataSource in databaseOption.DataSources)
        //     {
        //         logicDatabase.AddDataSource(dataSource.DataSourceName, dataSource.ConnectionString,
        //             dataSource.IsDefault);
        //     }
        // }
    }
    
    public bool DatabaseExists(string? database)
    {
        if (database == null)
        {
            return false;
        }
        return _logicDatabase.ContainsKey(database);
    }

    public LogicDatabase? GetDatabase(string? database)
    {
        if (database == null)
        {
            return null;
        }
        _logicDatabase.TryGetValue(database, out var logicDatabase);
        return logicDatabase;
    }

    public ICollection<string> GetAllDatabaseNames()
    {
        return _logicDatabase.Keys;
    }

    public OpenConnectorUser? GetUser(string username)
    {
        _connectorUsers.TryGetValue(username, out var OpenConnectorUser);
        return OpenConnectorUser;
    }
}