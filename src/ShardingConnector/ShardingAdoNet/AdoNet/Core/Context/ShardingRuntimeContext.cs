using System.Collections.Generic;
using System.Data.Common;
using ShardingConnector.Core.Rule;
using ShardingConnector.Kernels.MetaData.Schema;
using ShardingConnector.Spi.DataBase.DataBaseType;

namespace ShardingConnector.ShardingAdoNet.AdoNet.Core.Context
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
    public class ShardingRuntimeContext:MultipleDataSourcesRuntimeContext<ShardingRule>
    {
        // private readonly CachedDatabaseMetaData cachedDatabaseMetaData;
        //
        // private readonly ShardingTransactionManagerEngine shardingTransactionManagerEngine;
        public ShardingRuntimeContext(IDictionary<string, DbProviderFactory> dataSourceMap, ShardingRule rule, IDictionary<string, object> props, IDatabaseType databaseType) : base(dataSourceMap, rule, props, databaseType)
        {
        }

        protected override SchemaMetaData LoadSchemaMetaData(IDictionary<string, DbProviderFactory> dataSourceMap)
        {
            throw new System.NotImplementedException();
        }
    }
}