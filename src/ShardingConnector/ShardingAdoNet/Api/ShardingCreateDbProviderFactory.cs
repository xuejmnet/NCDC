using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using ShardingConnector.Api.Config.Sharding;
using ShardingConnector.Core.Rule;
using ShardingConnector.ShardingAdoNet.AdoNet.Core;

namespace ShardingConnector.ShardingAdoNet.Api
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/14 9:16:11
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ShardingCreateDbProviderFactory
    {
        private ShardingCreateDbProviderFactory() { }

        public static DbProviderFactory CreateDataSource(IDictionary<string, DbProviderFactory> dataSourceMap, ShardingRuleConfiguration shardingRuleConfig, IDictionary<string,object> props)
        {
            return new ShardingDbProviderFactory(dataSourceMap,new ShardingRule(),props);
            // return new ShardingDataSource(dataSourceMap, new ShardingRule(shardingRuleConfig, dataSourceMap.keySet()), props);
    }
}
}
