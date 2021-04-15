using System;
using System.Collections.Generic;
using System.Data.Common;
using ShardingConnector.Api.Database.DatabaseType;
using ShardingConnector.Exceptions;
using ShardingConnector.ShardingAdoNet.AdoNet.Adapter;
using ShardingConnector.Spi.DataBase.DataBaseType;

namespace ShardingConnector.ShardingAdoNet.AdoNet.Adapter
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/14 10:39:39
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public abstract class AbstractDataSourceAdapter:DbProviderFactory
    {
        
    private readonly IDictionary<String, DbProviderFactory> dataSourceMap;
    
    private readonly IDatabaseType _databaseType;
    
    
    public AbstractDataSourceAdapter(IDictionary<String, DbProviderFactory> dataSourceMap){
        this.dataSourceMap = dataSourceMap;
        _databaseType = CreateDatabaseType();
    }
    
    public AbstractDataSourceAdapter(DbProviderFactory dataSource) {
        dataSourceMap = new Dictionary<string, DbProviderFactory>();
        dataSourceMap.Add("unique", dataSource);
        _databaseType = CreateDatabaseType();
    }
    
    private IDatabaseType CreateDatabaseType() {
        IDatabaseType result = null;
        foreach (var dataSource in dataSourceMap)
        {
            IDatabaseType databaseType = CreateDatabaseType(dataSource.Value);
            var flag = result == null || result == _databaseType;
            if (!flag)
            {
                throw new ShardingException($"Database type inconsistent with '{result}' and '{databaseType}'");
            }

            result = databaseType;
        }
        return result;
    }
    
    private IDatabaseType CreateDatabaseType(DbProviderFactory dataSource) {
        
        if (dataSource is AbstractDataSourceAdapter abstractDataSourceAdapter) {
            return abstractDataSourceAdapter._databaseType;
        }

        using (var connection=dataSource.CreateConnection())
        {
            return DatabaseTypes.GetDatabaseTypeByUrl(connection.ConnectionString);
        }
    }
    
    
    protected abstract RuntimeContext getRuntimeContext();
    }
}