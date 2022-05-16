using ShardingConnector.Common.Config;
using ShardingConnector.Common.MetaData;
using ShardingConnector.Common.MetaData.DataSource;
using ShardingConnector.Common.Rule;
using ShardingConnector.NewConnector.DataSource;
using ShardingConnector.Spi.DataBase.DataBaseType;
using System;
using System.Collections.Generic;
using ShardingConnector.ParserBinder.MetaData.Schema;

namespace ShardingConnector.AdoNet.AdoNet.Core.Context
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/16 10:02:09
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public abstract class MultipleDataSourcesRuntimeContext<T> : AbstractRuntimeContext<T> where T : IBaseRule
    {
        private readonly ShardingConnectorMetaData _metaData;

        protected MultipleDataSourcesRuntimeContext(IDictionary<string, IDataSource> dataSourceMap, T rule,
            IDictionary<string, object> props, IDatabaseType databaseType) : base(rule, props, databaseType)
        {
            _metaData = CreateMetaData(dataSourceMap, databaseType);
        }

        private ShardingConnectorMetaData CreateMetaData(IDictionary<string, IDataSource> dataSourceMap,
            IDatabaseType databaseType)
        {
            long start = UtcTime.CurrentTimeMillis();
            DataSourceMetas dataSourceMetas =
                new DataSourceMetas(databaseType, GetDatabaseAccessConfigurationMap(dataSourceMap));
            SchemaMetaData schemaMetaData = LoadSchemaMetaData(dataSourceMap);
            ShardingConnectorMetaData result = new ShardingConnectorMetaData(dataSourceMetas, schemaMetaData);
            // log.info("Meta data load finished, cost {} milliseconds.", System.currentTimeMillis() - start);
            return result;
        }

        private IDictionary<string, DatabaseAccessConfiguration> GetDatabaseAccessConfigurationMap(
            IDictionary<String, IDataSource> dataSourceMap)
        {
            IDictionary<String, DatabaseAccessConfiguration> result =
                new Dictionary<string, DatabaseAccessConfiguration>(dataSourceMap.Count);
            foreach (var dataSource in dataSourceMap)
            {
                var dbProviderFactory = dataSource.Value;
                using (var connection = dbProviderFactory.CreateConnection())
                {
                    connection.Open();
                    result.Add(dataSource.Key,
                        new DatabaseAccessConfiguration(connection.ConnectionString, null, null));
                }
            }

            return result;
        }

        protected abstract SchemaMetaData LoadSchemaMetaData(IDictionary<string, IDataSource> dataSourceMap);

        public override ShardingConnectorMetaData GetMetaData()
        {
            return _metaData;
        }
    }
}