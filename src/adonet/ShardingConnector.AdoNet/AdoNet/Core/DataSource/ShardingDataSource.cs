// using System;
// using System.Collections.Generic;
// using System.Data.Common;
// using ShardingConnector.AdoNet.AdoNet.Adapter;
// using ShardingConnector.AdoNet.AdoNet.Core.Command;
// using ShardingConnector.AdoNet.AdoNet.Core.Connection;
// using ShardingConnector.AdoNet.AdoNet.Core.Context;
// using ShardingConnector.Merge.Engine;
// using ShardingConnector.NewConnector.DataSource;
// using ShardingConnector.RewriteEngine.Context;
// using ShardingConnector.Route;
// using ShardingConnector.ShardingCommon.Core.Rule;
// using ShardingConnector.Transaction;
// using ShardingConnector.Transaction.Core;
//
// namespace ShardingConnector.AdoNet.AdoNet.Core.DataSource
// {
// /*
// * @Author: xjm
// * @Description:
// * @Date: Sunday, 25 April 2021 21:51:47
// * @Email: 326308290@qq.com
// */
//     public sealed class ShardingDataSource:AbstractDataSourceAdapter
//     {
//
//         private readonly ShardingRuntimeContext _runtimeContext;
//
//         static ShardingDataSource()
//         {
//             NewInstanceServiceLoader.Register<IRouteDecorator>();
//             NewInstanceServiceLoader.Register<ISqlRewriteContextDecorator>();
//             NewInstanceServiceLoader.Register<IResultProcessEngine>();
//         }
//
//
//         public override DbConnection CreateConnection()
//         {
//             return new ShardingConnection(DataSourceMap, _runtimeContext,
//                 TransactionTypeHolder.Get() ?? TransactionTypeEnum.LOCAL,DefaultDataSource.CreateConnection());
//         }
//
//         public override DbCommand CreateCommand()
//         {
//             return new ShardingCommand(DefaultDataSource.CreateCommand());
//         }
//
//         public override bool IsDefault()
//         {
//             throw new NotImplementedException();
//         }
//
//         public ShardingDataSource(IDictionary<string, IDataSource> dataSourceMap,ShardingRule shardingRule, IDictionary<string,object> props) : base(dataSourceMap)
//         {
//             _runtimeContext = new ShardingRuntimeContext(dataSourceMap, shardingRule, props, DatabaseType);
//         }
//
//     }
// }