using System;
using System.Collections.Generic;
using System.Data.Common;
using ShardingConnector.Common.Rule;
using ShardingConnector.Core.Rule;
using ShardingConnector.Exceptions;
using ShardingConnector.Kernels.Route;
using ShardingConnector.Merge.Engine;
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
        private readonly ShardingRuntimeContext _runtimeContext;

        static ShardingDbProviderFactory()
        {
            NewInstanceServiceLoader.Register(typeof(IRouteDecorator<>));
            NewInstanceServiceLoader.Register(typeof(ISqlRewriteContextDecorator<>));
            NewInstanceServiceLoader.Register(typeof(IResultProcessEngine<>));
        }


        public override DbConnection CreateConnection()
        {
            return new ShardingConnection(DataSourceMap, _runtimeContext,
                TransactionTypeHolder.Get() ?? TransactionTypeEnum.LOCAL);
        }

        public ShardingDbProviderFactory(IDictionary<string, DbProviderFactory> dataSourceMap,
            ShardingRule shardingRule, IDictionary<string, object> props) : base(dataSourceMap)
        {
            CheckDataSourceType(dataSourceMap);
            _runtimeContext = new ShardingRuntimeContext(dataSourceMap, shardingRule, props, GetDatabaseType());
        }

        public ShardingDbProviderFactory(DbProviderFactory dataSource, ShardingRule shardingRule,
            IDictionary<string, object> props) : base(dataSource)
        {
            var dataSourceMap = new Dictionary<string, DbProviderFactory>() {{"unique", dataSource}};
            CheckDataSourceType(dataSourceMap);
            _runtimeContext = new ShardingRuntimeContext(dataSourceMap, shardingRule, props, GetDatabaseType());
        }

        private void CheckDataSourceType(IDictionary<String, DbProviderFactory> dataSourceMap)
        {
            foreach (var dataSource in dataSourceMap)
            {
                // if (dataSource is MasterSlaveDataSource)
                // {
                //     throw new ShardingException("Initialized data sources can not be master-slave data sources.");
                // }
            }
        }

        protected override IRuntimeContext<IBaseRule> GetRuntimeContext()
        {
            return _runtimeContext;
        }
    }
}