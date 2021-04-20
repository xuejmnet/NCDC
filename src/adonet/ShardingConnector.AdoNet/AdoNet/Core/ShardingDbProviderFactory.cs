using System;
using System.Collections.Generic;
using System.Data.Common;
using ShardingConnector.AdoNet.AdoNet.Core.Connection;
using ShardingConnector.AdoNet.AdoNet.Core.Context;
using ShardingConnector.Common.Rule;
using ShardingConnector.Core.Rule;
using ShardingConnector.Kernels.Route;
using ShardingConnector.Merge.Engine;
using ShardingConnector.NewConnector.DataSource;
using ShardingConnector.Rewrite.Context;
using ShardingConnector.ShardingAdoNet;
using ShardingConnector.ShardingAdoNet.AdoNet.Adapter;
using ShardingConnector.Transaction;
using ShardingConnector.Transaction.Core;
using AbstractDbProviderFactory = ShardingConnector.AdoNet.AdoNet.Abstraction.AbstractDbProviderFactory;

namespace ShardingConnector.AdoNet.AdoNet.Core
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

        public ShardingDbProviderFactory(IDictionary<string, IDataSource> dataSourceMap,
            ShardingRule shardingRule, IDictionary<string, object> props) : base(dataSourceMap)
        {
            CheckDataSourceType(dataSourceMap);
            _runtimeContext = new ShardingRuntimeContext(dataSourceMap, shardingRule, props, GetDatabaseType());
        }

        public ShardingDbProviderFactory(IDataSource dataSource, ShardingRule shardingRule,
            IDictionary<string, object> props) : base(dataSource)
        {
            var dataSourceMap = new Dictionary<string, IDataSource>() {{"unique", dataSource}};
            CheckDataSourceType(dataSourceMap);
            _runtimeContext = new ShardingRuntimeContext(dataSourceMap, shardingRule, props, GetDatabaseType());
        }

        private void CheckDataSourceType(IDictionary<String, IDataSource> dataSourceMap)
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