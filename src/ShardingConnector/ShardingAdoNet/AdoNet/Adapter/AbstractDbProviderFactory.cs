using System;
using System.Collections.Generic;
using System.Data.Common;
using ShardingConnector.Api.Database.DatabaseType;
using ShardingConnector.Common.Rule;
using ShardingConnector.Exceptions;
using ShardingConnector.ShardingAdoNet.AdoNet.Adapter;
using ShardingConnector.ShardingAdoNet.AdoNet.Core.Context;
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
    public abstract class AbstractDbProviderFactory : DbProviderFactory
    {
        public  IDictionary<string, DbProviderFactory> DataSourceMap  { get; }

        private readonly IDatabaseType _databaseType;


        public AbstractDbProviderFactory(IDictionary<String, DbProviderFactory> dataSourceMap)
        {
            this.DataSourceMap = dataSourceMap;
            _databaseType = CreateDatabaseType();
        }

        public AbstractDbProviderFactory(DbProviderFactory dataSource)
        {
            DataSourceMap = new Dictionary<string, DbProviderFactory>();
            DataSourceMap.Add("unique", dataSource);
            _databaseType = CreateDatabaseType();
        }

        private IDatabaseType CreateDatabaseType()
        {
            IDatabaseType result = null;
            foreach (var dataSource in DataSourceMap)
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

        private IDatabaseType CreateDatabaseType(DbProviderFactory dataSource)
        {
            if (dataSource is AbstractDbProviderFactory abstractDataSourceAdapter)
            {
                return abstractDataSourceAdapter._databaseType;
            }

            using (var connection = dataSource.CreateConnection())
            {
                return DatabaseTypes.GetDatabaseTypeByUrl(connection.ConnectionString);
            }
        }


        protected abstract IRuntimeContext<IBaseRule> GetRuntimeContext();
    }
}