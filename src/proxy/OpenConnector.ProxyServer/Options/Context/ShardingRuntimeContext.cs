using NCDC.CommandParser.Abstractions;
using NCDC.CommandParserBinder.MetaData.Schema;
using NCDC.Common.MetaData.Decorator;
using OpenConnector.DataSource;
using OpenConnector.ShardingCommon.Core.MetaData;
using OpenConnector.ShardingCommon.Core.Rule;
using OpenConnector.Spi.DataBase.DataBaseType;

namespace OpenConnector.ProxyServer.Options.Context
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
        public ShardingRuntimeContext(IDictionary<string, IDataSource> dataSourceMap, ShardingRule rule,ISqlParserConfiguration sqlParserConfiguration, IDictionary<string, object> props, IDatabaseType databaseType) : base(dataSourceMap, rule,sqlParserConfiguration, props, databaseType)
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
            int maxConnectionsLimitSize = 10;
            bool isCheckingMetaData = true;
            var rule = GetRule();
            SchemaMetaData result = new ShardingMetaDataLoader(dataSourceMap, rule, maxConnectionsLimitSize, isCheckingMetaData).Load(GetDatabaseType());
            result = SchemaMetaDataDecorator.Decorate(result, rule, new ShardingTableMetaDataDecorator());
            return result;
        }
    }
}