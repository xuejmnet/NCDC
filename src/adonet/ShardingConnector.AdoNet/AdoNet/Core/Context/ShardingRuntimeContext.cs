using ShardingConnector.NewConnector.DataSource;
using ShardingConnector.ParserBinder.MetaData.Schema;
using ShardingConnector.ShardingCommon.Core.Rule;
using ShardingConnector.Spi.DataBase.DataBaseType;
using System.Collections.Generic;
using ShardingConnector.ShardingCommon.Core.MetaData;

namespace ShardingConnector.AdoNet.AdoNet.Core.Context
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/16 10:24:27
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public class ShardingRuntimeContext: MultipleDataSourcesRuntimeContext<ShardingRule>
    {
        // private readonly CachedDatabaseMetaData cachedDatabaseMetaData;
        //
        // private readonly ShardingTransactionManagerEngine shardingTransactionManagerEngine;
        public ShardingRuntimeContext(IDictionary<string, IDataSource> dataSourceMap, ShardingRule rule, IDictionary<string, object> props, IDatabaseType databaseType) : base(dataSourceMap, rule, props, databaseType)
        {
        }

        protected override SchemaMetaData LoadSchemaMetaData(IDictionary<string, IDataSource> dataSourceMap)
        {
            //int maxConnectionsSizePerQuery = getProperties().< Integer > getValue(ConfigurationPropertyKey.MAX_CONNECTIONS_SIZE_PER_QUERY);
            //boolean isCheckingMetaData = getProperties().< bool > getValue(ConfigurationPropertyKey.CHECK_TABLE_METADATA_ENABLED);
            //SchemaMetaData result = new ShardingMetaDataLoader(dataSourceMap, getRule(), maxConnectionsSizePerQuery, isCheckingMetaData).load(getDatabaseType());
            //result = SchemaMetaDataDecorator.decorate(result, getRule(), new ShardingTableMetaDataDecorator());
            //if (!getRule().getEncryptRule().getEncryptTableNames().isEmpty())
            //{
            //    result = SchemaMetaDataDecorator.decorate(result, getRule().getEncryptRule(), new EncryptTableMetaDataDecorator());
            //}
            int maxConnectionsSizePerQuery = 10;
            bool isCheckingMetaData = true;
            SchemaMetaData result = new ShardingMetaDataLoader(dataSourceMap, getRule(), maxConnectionsSizePerQuery, isCheckingMetaData).load(getDatabaseType());
            //result = SchemaMetaDataDecorator.decorate(result, getRule(), new ShardingTableMetaDataDecorator());
            //if (!getRule().getEncryptRule().getEncryptTableNames().isEmpty())
            //{
            //    result = SchemaMetaDataDecorator.decorate(result, getRule().getEncryptRule(), new EncryptTableMetaDataDecorator());
            //}
            //return new SchemaMetaData();
            return null;
        }
    }
}