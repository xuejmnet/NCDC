using MySqlConnector;
using ShardingConnector.Api.Database.DatabaseType;
using ShardingConnector.Exceptions;
using ShardingConnector.MySqlParser;
using ShardingConnector.NewConnector.DataSource;
using ShardingConnector.ShardingApi.Api.Config.Sharding;
using ShardingConnector.ShardingApi.Api.Config.Sharding.Strategy;
using ShardingConnector.ShardingApi.Api.Sharding.Standard;
using ShardingConnector.ShardingCommon.Core.Rule;
using ShardingConnector.Spi.DataBase.DataBaseType;
using ShardingConnector.ProxyServer.Commons;
using ShardingConnector.ProxyServer.Options;
using ShardingConnector.ProxyServer.Options.Context;

namespace ShardingConnector.ProxyServer;

public class ProxyContext
{
    public static ShardingRuntimeContext ShardingRuntimeContext { get; set; }
    public static void Init()
    {
        var dataSourceMap = new Dictionary<string, IDataSource>()
        {
            {
                "ds0",
                new GenericDataSource("ds0",MySqlConnectorFactory.Instance,"server=127.0.0.1;port=3306;database=test;userid=root;password=root;",true)
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
    
       var defaultDataSource = dataSourceMap.Values.FirstOrDefault(o => o.IsDefault()) ??
                             throw new InvalidOperationException("not found default data source for init sharding");
        var databaseType = CreateDatabaseType(dataSourceMap);
        var mySqlParserConfiguration = new MySqlParserConfiguration();
        ShardingRuntimeContext=new ShardingRuntimeContext(dataSourceMap, new ShardingRule(shardingRuleConfig, dataSourceMap.Keys),mySqlParserConfiguration, new Dictionary<string, object>(), databaseType);
    }

    private static IDatabaseType CreateDatabaseType(Dictionary<string, IDataSource> _dataSourceMap)
    {
        IDatabaseType result = null;
        foreach (var dataSource in _dataSourceMap)
        {
            IDatabaseType databaseType = CreateDatabaseType(dataSource.Value);
            var flag = result != null ;
            //保证所有的数据源都是相同数据库
            if (flag)
            {
                throw new ShardingException($"Database type inconsistent with '{result}' and '{databaseType}'");
            }

            result = databaseType;
        }

        return result;
    }

    private static IDatabaseType CreateDatabaseType(IDataSource dataSource)
    {
        using (var connection = dataSource.GetDbProviderFactory().CreateConnection())
        {
            connection.ConnectionString = dataSource.GetConnectionString();
            return DatabaseTypes.GetDataBaseTypeByDbConnection(connection);
        }
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
            // result.KeyGeneratorConfig = new KeyGeneratorConfiguration("id", "id");
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