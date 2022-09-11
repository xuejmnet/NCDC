using System.Data.Common;
using Microsoft.Extensions.Logging;
using NCDC.Basic.Configurations;
using NCDC.Basic.Connection.User;
using OpenConnector.Base;
using OpenConnector.Extensions;

namespace NCDC.Basic.Metadatas;

public sealed partial class LogicDbServer:ILogicDbServer
{
    
    
    public void Init(RuntimeOption runtimeOption)
    {
        if (runtimeOption.Databases.IsEmpty())
        {
            _logger.LogWarning("not found any databases");
            return;
        }

    
        foreach (var databaseOption in runtimeOption.Databases)
        {
            InitDatabase(runtimeOption.DbProviderFactory,databaseOption);
        }
        if (runtimeOption.Users.IsNotEmpty())
        {
            foreach (var userOption in runtimeOption.Users)
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
                if (_logicDatabases.TryGetValue(database,out var logicDatabase))
                {
                    logicDatabase.AddConnectorUser(userOption.Username);
                }
            }
        }
        _connectorUsers.TryAdd(userOption.Username,OpenConnectorUser);
    }

    private void InitDatabase(DbProviderFactory dbProviderFactory,DatabaseOption databaseOption)
    {
        var logicDatabase = new LogicDatabase(databaseOption.Name);
        if (!_logicDatabases.TryAdd(logicDatabase.Name, logicDatabase))
        {
            throw new ArgumentException("database repeat");
        }

        var defaultCount = databaseOption.DataSources.Where(o=>o.IsDefault).Take(2).Count();
        ShardingAssert.If(defaultCount == 0,$"database:{logicDatabase.Name},not found default data source");
        ShardingAssert.If(defaultCount == 2,$"database:{logicDatabase.Name},default data source repeat");
        if (databaseOption.DataSources.IsNotEmpty())
        {
            foreach (var dataSource in databaseOption.DataSources)
            {
                logicDatabase.AddDataSource(dataSource.DataSourceName, dataSource.ConnectionString,dbProviderFactory,
                    dataSource.IsDefault);
            }
        }
    }
}