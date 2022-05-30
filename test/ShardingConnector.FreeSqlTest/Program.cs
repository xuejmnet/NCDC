
using System.Data.Common;
using FreeSql;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using ShardingConnector.AdoNet.AdoNet.Core;
using ShardingConnector.AdoNet.AdoNet.Core.DataSource;
using ShardingConnector.Logger;
using ShardingConnector.NewConnector.DataSource;
using ShardingConnector.ShardingApi.Api.Config.Sharding;
using ShardingConnector.ShardingApi.Api.Config.Sharding.Strategy;
using ShardingConnector.ShardingApi.Api.Sharding.Standard;

namespace ShardingConnector.FreeSqlTest
{
    /// <summary>
    /// 还未实现
    /// </summary>
    internal class Program
    {
        // private static readonly string conn = "server=127.0.0.1;port=3306;database=test;userid=root;password=L6yBtV6qNENrwBy7;";
        private static readonly string conn = "server=127.0.0.1;port=3306;database=test;userid=root;password=root;";
        private static IFreeSql fsql;
        static void Main(string[] args)
        {
            InternalLoggerFactory.DefaultFactory = LoggerFactory.Create(builder =>
            {
                builder.AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddSimpleConsole(c => c.TimestampFormat = "[yyyy-MM-dd HH:mm:ss]");
            });
            //var dbProviderFactory = ShardingCreateDbProviderFactory.CreateDataSource(dataSourceMap, new ShardingRuleConfiguration(),
            //    new Dictionary<string, object>());
            var dataSourceMap = new Dictionary<string, IDataSource>()
            {
                {
                    "ds0",
                    new GenericDataSource(MySqlConnectorFactory.Instance,conn,true)
                }
            };
            //2、分库分表配置
            ShardingRuleConfiguration shardingRuleConfig = new ShardingRuleConfiguration();
            //2.2、配置各个表的分库分表策略，这里只配了一张表的就是t_order
            shardingRuleConfig.TableRuleConfigs.Add(CreateSysUserModTableRule());
            //2.5、配置默认分表规则
            shardingRuleConfig.DefaultTableShardingStrategyConfig = new NoneShardingStrategyConfiguration();
            //2.6、配置默认分库规则(不配置分库规则,则只采用分表规则)
            shardingRuleConfig.DefaultDatabaseShardingStrategyConfig = new NoneShardingStrategyConfiguration();
            //2.7、配置默认数据源
            shardingRuleConfig.DefaultDataSourceName = "ds0";
            var shardingDbProviderFactory = new ShardingDbProviderFactory(dataSourceMap, shardingRuleConfig, new Dictionary<string, object>());
            fsql  = new FreeSql.FreeSqlBuilder()
                 .UseConnectionFactory(DataType.MySql, () =>
                 {
                     var dbConnection = shardingDbProviderFactory.CreateConnection();
                     dbConnection.ConnectionString = conn;
                     return dbConnection;
                 })
                .Build();
            Test1();
        }

        static void Test1()
        {
            var ids = new[]{"21","22"};
            var list = fsql.Select<User>().Where(o=>ids.Contains(o.Id)).ToList();
        }

        static TableRuleConfiguration CreateSysUserModTableRule()
        {
            TableRuleConfiguration result = new TableRuleConfiguration("SysUserMod", new List<string>()
            {
                "ds0.SysUserMod_00",
                "ds0.SysUserMod_01",
                "ds0.SysUserMod_02"
            });
            //1、指定逻辑索引(oracle/PostgreSQL需要配置)
            //        result.setLogicIndex("order_id");
            result.DatabaseShardingStrategyConfig = new NoneShardingStrategyConfiguration();
            //4、配置分库策略,缺省表示使用默认分库策略
            //result.setDatabaseShardingStrategyConfig(new InlineShardingStrategyConfiguration("user_id", "ds${user_id % 2}"));
            //result.setDatabaseShardingStrategyConfig(new HintShardingStrategyConfiguration(new OrderDataBaseHintShardingAlgorithm()));
            //5、配置分表策略,缺省表示使用默认分表策略
            result.TableShardingStrategyConfig = new StandardShardingStrategyConfiguration("Id", new SysUserModId());
            //result.setTableShardingStrategyConfig(new InlineShardingStrategyConfiguration("order_id", "t_order_${order_id % 2}"));
            //result.setTableShardingStrategyConfig(new StandardShardingStrategyConfiguration("order_id",new orderPreciseShardingAlgorithm(),new orderRangeShardingAlgorithm()));
            //result.setTableShardingStrategyConfig(new ComplexShardingStrategyConfiguration("order_id,user_id",new orderComplexKeysShardingAlgorithm()));
            //result.setTableShardingStrategyConfig(new HintShardingStrategyConfiguration(new OrderTableHintShardingAlgorithm()));
            //6、指定自增字段以及key的生成方式
            //result.setKeyGeneratorColumnName("order_id");
            //result.setKeyGenerator(new DefaultKeyGenerator());
            return result;
        }
    }

    public class SysUserModId : IPreciseShardingAlgorithm<string>
    {
        public string DoSharding(ICollection<string> availableTargetNames, PreciseShardingValue shardingValue)
        {
            foreach (var name in availableTargetNames)
            {
                if (name.EndsWith($"{(int.Parse((string)shardingValue.Value) % availableTargetNames.Count)}"))
                    return name;
            }

            return null;
        }
    }

    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}