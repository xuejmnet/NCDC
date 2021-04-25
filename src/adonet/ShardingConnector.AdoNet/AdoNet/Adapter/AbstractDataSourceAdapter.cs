using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using ShardingConnector.AdoNet.AdoNet.Core.Context;
using ShardingConnector.Api.Database.DatabaseType;
using ShardingConnector.Common.Rule;
using ShardingConnector.Exceptions;
using ShardingConnector.NewConnector.DataSource;
using ShardingConnector.Spi.DataBase.DataBaseType;

namespace ShardingConnector.AdoNet.AdoNet.Adapter
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/25 16:56:19
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public abstract class AbstractDataSourceAdapter:IDataSource
    {
        public IDictionary<string, IDataSource> DataSourceMap { get; }

        public  IDatabaseType DatabaseType{ get; }


        public AbstractDataSourceAdapter(IDictionary<string, IDataSource> dataSourceMap)
        {
            this.DataSourceMap = dataSourceMap;
            DatabaseType = CreateDatabaseType();
        }

        public AbstractDataSourceAdapter(IDataSource dataSource) : this(new Dictionary<string, IDataSource>() { { "unique", dataSource } })
        {
        }

        private IDatabaseType CreateDatabaseType()
        {
            IDatabaseType result = null;
            foreach (var dataSource in DataSourceMap)
            {
                IDatabaseType databaseType = CreateDatabaseType(dataSource.Value);
                var flag = result == null || result == DatabaseType;
                if (!flag)
                {
                    throw new ShardingException($"Database type inconsistent with '{result}' and '{databaseType}'");
                }

                result = databaseType;
            }

            return result;
        }

        private IDatabaseType CreateDatabaseType(IDataSource dataSource)
        {
            if (dataSource is AbstractDataSourceAdapter abstractDataSourceAdapter)
            {
                return abstractDataSourceAdapter.DatabaseType;
            }

            using (var connection = dataSource.GetDbConnection())
            {
                return DatabaseTypes.GetDataBaseTypeByDbConnection(connection);
            }
        }
        public abstract DbConnection GetDbConnection();
    }
}
