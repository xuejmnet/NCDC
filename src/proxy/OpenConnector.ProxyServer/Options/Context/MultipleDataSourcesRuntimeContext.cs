using Microsoft.Extensions.Logging;
using OpenConnector.CommandParserBinder.MetaData.Schema;
using OpenConnector.Common.Config;
using OpenConnector.Common.MetaData;
using OpenConnector.Common.MetaData.DataSource;
using OpenConnector.Common.Rule;
using OpenConnector.Logger;
using OpenConnector.NewConnector.DataSource;
using OpenConnector.Parsers;
using OpenConnector.Spi.DataBase.DataBaseType;

namespace OpenConnector.ProxyServer.Options.Context
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
    public abstract class MultipleDataSourcesRuntimeContext<T> : ProxyServer.Options.Context.AbstractRuntimeContext<T> where T : IBaseRule
    {
        private static readonly ILogger<MultipleDataSourcesRuntimeContext<T>> _logger =
            InternalLoggerFactory.CreateLogger<MultipleDataSourcesRuntimeContext<T>>();
        private readonly OpenConnectorMetaData _metaData;

        protected MultipleDataSourcesRuntimeContext(IDictionary<string, IDataSource> dataSourceMap, T rule,ISqlParserConfiguration sqlParserConfiguration,
            IDictionary<string, object> props, IDatabaseType databaseType) : base(rule,sqlParserConfiguration, props, databaseType)
        {
            _metaData = CreateMetaData(dataSourceMap, databaseType);
        }
        /// <summary>
        /// 创建数据库元数据信息
        /// </summary>
        /// <param name="dataSourceMap"></param>
        /// <param name="databaseType"></param>
        /// <returns></returns>
        private OpenConnectorMetaData CreateMetaData(IDictionary<string, IDataSource> dataSourceMap,
            IDatabaseType databaseType)
        {
            long start = UtcTime.CurrentTimeMillis();
            DataSourceMetas dataSourceMetas =
                new DataSourceMetas(databaseType, GetDatabaseAccessConfigurationMap(dataSourceMap));
            SchemaMetaData schemaMetaData = LoadSchemaMetaData(dataSourceMap);
            OpenConnectorMetaData result = new OpenConnectorMetaData(dataSourceMetas, schemaMetaData);
            var costMillis = UtcTime.CurrentTimeMillis() - start;
            _logger.LogInformation($"Meta data load finished, cost {costMillis} milliseconds.");
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
                    result.Add(dataSource.Key,
                        new DatabaseAccessConfiguration(connection.ConnectionString, null, null));
                }
            }

            return result;
        }

        protected abstract SchemaMetaData LoadSchemaMetaData(IDictionary<string, IDataSource> dataSourceMap);

        public override OpenConnectorMetaData GetMetaData()
        {
            return _metaData;
        }
    }
}