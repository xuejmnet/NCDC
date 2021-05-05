using Microsoft.Data.SqlClient;
using ShardingConnector.AdoNet.AdoNet.Core.DataSource;
using ShardingConnector.AdoNet.Api;
using ShardingConnector.NewConnector.DataSource;
using ShardingConnector.ShardingApi.Api.Config.Sharding;
using ShardingConnector.ShardingApi.Api.Config.Sharding.Strategy;
using ShardingConnector.ShardingApi.Api.Sharding.Standard;
using System;
using System.Collections.Generic;
using ShardingConnector.Executor.SqlLog;

namespace ShardingConnector.AppConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlLogger.AddLog((msg)=>Console.WriteLine(msg));
            //var dbProviderFactory = ShardingCreateDbProviderFactory.CreateDataSource(dataSourceMap, new ShardingRuleConfiguration(),
            //    new Dictionary<string, object>());
            var dataSourceMap = new Dictionary<string, IDataSource>()
            {
                {
                    "ds0",
                    new GenericDataSource(SqlClientFactory.Instance,
                        "Data Source=localhost;Initial Catalog=ShardingCoreDB;Integrated Security=True;MultipleActiveResultSets=True")
                }
            };
            //2、分库分表配置
            ShardingRuleConfiguration shardingRuleConfig = new ShardingRuleConfiguration();
            //2.2、配置各个表的分库分表策略，这里只配了一张表的就是t_order
            shardingRuleConfig.TableRuleConfigs.Add(CreateSysUserModTableRule());
            //2.5、配置默认分表规则
            shardingRuleConfig.DefaultTableShardingStrategyConfig=new NoneShardingStrategyConfiguration();
            //2.6、配置默认分库规则(不配置分库规则,则只采用分表规则)
            shardingRuleConfig.DefaultDatabaseShardingStrategyConfig = new NoneShardingStrategyConfiguration();
            //2.7、配置默认数据源
            shardingRuleConfig.DefaultDataSourceName= "ds0";

            var dataSource = ShardingDataSourceFactory.CreateDataSource(dataSourceMap, shardingRuleConfig, new Dictionary<string, object>());
            var dbConnection = dataSource.GetDbConnection();


            var dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = @"select [d].[Id],[d].[Name],[d].[Age] from [dbo].[SysUserMod] as [d] where [d].[Id]='21' ";
            //dbCommand.CommandText = @"select [d].[Id],[d].[Name],[d].[Age] from [dbo].[SysUserMod] as [d] ";
            var dbDataReader = dbCommand.ExecuteReader();
            while (dbDataReader.Read())
            {
                Console.WriteLine($"{dbDataReader[0]}-{dbDataReader[1]}-{dbDataReader[2]}");
            }
            Console.WriteLine("Hello World!");
        }

        static TableRuleConfiguration CreateSysUserModTableRule()
        {
            TableRuleConfiguration result = new TableRuleConfiguration("SysUserMod",new List<string>()
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
}