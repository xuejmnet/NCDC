// using System;
// using System.Collections.Generic;
// using System.Data.Common;
// using System.Text;
// using ShardingConnector.AdoNet.AdoNet.Core.DataSource;
// using ShardingConnector.NewConnector.DataSource;
// using ShardingConnector.ShardingApi.Api.Config.Sharding;
// using ShardingConnector.ShardingCommon.Core.Rule;
//
// namespace ShardingConnector.AdoNet.Api
// {
//     /*
//     * @Author: xjm
//     * @Description:
//     * @Date: 2021/4/26 9:22:08
//     * @Ver: 1.0
//     * @Email: 326308290@qq.com
//     */
//     public sealed class ShardingDataSourceFactory
//     {
//         private ShardingDataSourceFactory()
//         {
//
//         }
//         public static IDataSource CreateDataSource(IDictionary<string, IDataSource> dataSourceMap, ShardingRuleConfiguration shardingRuleConfig, IDictionary<string, object> props)
//         {
//             return new ShardingDataSource(dataSourceMap, new ShardingRule(shardingRuleConfig, dataSourceMap.Keys), props);
//         }
//
//         public static DbProviderFactory CreateDbProviderFactory(IDictionary<string, IDataSource> dataSourceMap,
//             ShardingRuleConfiguration shardingRuleConfig, IDictionary<string, object> props)
//         {
//             
//         }
//     }
// }
