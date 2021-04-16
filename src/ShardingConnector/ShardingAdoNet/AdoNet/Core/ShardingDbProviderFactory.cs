using System;
using System.Collections.Generic;
using System.Data.Common;
using ShardingConnector.Common.Rule;
using ShardingConnector.Kernels.Route;
using ShardingConnector.Rewrite.Context;
using ShardingConnector.ShardingAdoNet;
using ShardingConnector.ShardingAdoNet.AdoNet.Adapter;
using ShardingConnector.ShardingAdoNet.AdoNet.Core.Context;
using ShardingConnector.Transaction;
using ShardingConnector.Transaction.Core;

namespace ShardingConnector.ShardingAdoNet.AdoNet.Core
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/14 10:38:48
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public class ShardingDbProviderFactory : AbstractDbProviderFactory
    {
        private ShardingRuntimeContext runtimeContext;

        static ShardingDbProviderFactory()
        {
            NewInstanceServiceLoader.Register(typeof(IRouteDecorator<>));
            NewInstanceServiceLoader.Register(typeof(ISqlRewriteContextDecorator<>));
            // NewInstanceServiceLoader.Register<ResultProcessEngine>();
        }

        //     public ShardingDbProviderFactory(final Map<String, DataSource> dataSourceMap, final ShardingRule shardingRule, final Properties props) throws SQLException {
        //         super(dataSourceMap);
        //     ShardingDbProviderFactory(dataSourceMap);
        //     runtimeContext = new ShardingDbProviderFactory(dataSourceMap, shardingRule, props, getDatabaseType());
        // }

//     private void checkDataSourceType(final Map<String, DataSource> dataSourceMap) {
//     for (DataSource each : dataSourceMap.values()) {
//     Preconditions.checkArgument(!(each instanceof MasterSlaveDataSource), "Initialized data sources can not be master-slave data sources.");
//     }
// }

        public ShardingConnection GetConnection()
        {
            return new ShardingConnection(DataSourceMap, runtimeContext,
                TransactionTypeHolder.Get() ?? TransactionTypeEnum.LOCAL);
        }

        public ShardingDbProviderFactory(IDictionary<string, DbProviderFactory> dataSourceMap) : base(dataSourceMap)
        {
        }

        public ShardingDbProviderFactory(DbProviderFactory dataSource) : base(dataSource)
        {
        }

        protected override IRuntimeContext<IBaseRule> GetRuntimeContext()
        {
            throw new NotImplementedException();
        }
    }
}