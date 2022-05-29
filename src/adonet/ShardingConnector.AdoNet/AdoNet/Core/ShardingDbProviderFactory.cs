using ShardingConnector.AdoNet.AdoNet.Core.Connection;
using ShardingConnector.AdoNet.AdoNet.Core.Context;
using ShardingConnector.Common.Rule;
using ShardingConnector.Merge.Engine;
using ShardingConnector.NewConnector.DataSource;
using ShardingConnector.RewriteEngine.Context;
using ShardingConnector.Route;
using ShardingConnector.ShardingCommon.Core.Rule;
using ShardingConnector.Transaction;
using ShardingConnector.Transaction.Core;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using ShardingConnector.AdoNet.AdoNet.Core.Command;
using ShardingConnector.Api.Database.DatabaseType;
using ShardingConnector.Exceptions;
using ShardingConnector.ShardingApi.Api.Config.Sharding;
using ShardingConnector.Spi.DataBase.DataBaseType;

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
    public class ShardingDbProviderFactory : DbProviderFactory
    {

        static ShardingDbProviderFactory()
        {
            NewInstanceServiceLoader.Register<IRouteDecorator>();
            NewInstanceServiceLoader.Register<ISqlRewriteContextDecorator>();
            NewInstanceServiceLoader.Register<IResultProcessEngine>();
        }

        private readonly IDictionary<string, IDataSource> _dataSourceMap;
        private readonly IDataSource _defaultDataSource;
        private readonly ShardingRuntimeContext _shardingRuntimeContext;
        public  IDatabaseType DatabaseType{ get; }
        // public static readonly ShardingDbProviderFactory Instance = new ShardingDbProviderFactory();
        // private  IDataSource _dataSource;
        //
        // public static void Init(IDictionary<string, DbProviderFactory> dataSourceMap, ShardingRuleConfiguration shardingRuleConfig, IDictionary<string, object> props)
        // {
        //     Instance._dataSource= ShardingDataSourceFactory.CreateDataSource(dataSourceMap, shardingRuleConfig, new Dictionary<string, object>());
        // }
        public ShardingDbProviderFactory(IDictionary<string, IDataSource> dataSourceMap, ShardingRuleConfiguration shardingRuleConfig, IDictionary<string, object> props)
        {
            _dataSourceMap = dataSourceMap;
            _defaultDataSource = dataSourceMap.Values.FirstOrDefault(o => o.IsDefault()) ??
                                 throw new InvalidOperationException("not found default data source for init sharding");
            var databaseType = CreateDatabaseType();
            _shardingRuntimeContext=new ShardingRuntimeContext(dataSourceMap, new ShardingRule(shardingRuleConfig, dataSourceMap.Keys), props, databaseType);
        }

        private IDatabaseType CreateDatabaseType()
        {
            IDatabaseType result = null;
            foreach (var dataSource in _dataSourceMap)
            {
                IDatabaseType databaseType = CreateDatabaseType(dataSource.Value);
                var flag = result != null && result != DatabaseType;
                //保证所有的数据源都是相同数据库
                if (flag)
                {
                    throw new ShardingException($"Database type inconsistent with '{result}' and '{databaseType}'");
                }

                result = databaseType;
            }

            return result;
        }

        private IDatabaseType CreateDatabaseType(IDataSource dataSource)
        {
            using (var connection = dataSource.GetDbProviderFactory().CreateConnection())
            {
                connection.ConnectionString = dataSource.GetConnectionString();
                return DatabaseTypes.GetDataBaseTypeByDbConnection(connection);
            }
        }

        public override DbConnection CreateConnection()
        {
            return new ShardingConnection(_dataSourceMap,_shardingRuntimeContext,TransactionTypeEnum.LOCAL,_defaultDataSource.GetDbProviderFactory().CreateConnection());
        }

        public override DbCommand CreateCommand()
        {
            return new ShardingCommand();
        }

        public override DbParameter CreateParameter()
        {
            return new ShardingParameter();
        }
    }
}