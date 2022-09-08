// using OpenConnector.Api.Database.DatabaseType.Dialect;
// using OpenConnector.CommandParser.Abstractions;
// using OpenConnector.CommandParser.SqlParseEngines;
// using OpenConnector.CommandParserBinder;
// using OpenConnector.CommandParserBinder.Command;
// using OpenConnector.CommandParserBinder.MetaData.Column;
// using OpenConnector.CommandParserBinder.MetaData.Index;
// using OpenConnector.CommandParserBinder.MetaData.Schema;
// using OpenConnector.CommandParserBinder.MetaData.Table;
// using OpenConnector.Common.Config;
// using OpenConnector.Common.Config.Properties;
// using OpenConnector.Common.MetaData;
// using OpenConnector.Common.MetaData.DataSource;
// using OpenConnector.MySqlParser;
// using OpenConnector.Route.Context;
// using OpenConnector.ShardingAdoNet;
// using OpenConnector.ShardingApi.Api.Config.Sharding;
// using OpenConnector.ShardingApi.Api.Config.Sharding.Strategy;
// using OpenConnector.ShardingApi.Api.Sharding.Standard;
// using OpenConnector.ShardingCommon.Core.Rule;
// using OpenConnector.ShardingRoute.Engine;
//
// namespace OpenConnector.ShardingRouteTest;
//
// public class Tests
// {
//     private ISqlCommandParser _sqlCommandParser;
//     private SchemaMetaData _schemaMetaData;
//     private ShardingRouteDecorator _shardingRouteDecorator;
//     private OpenConnectorMetaData _openConnectorMetaData;
//     private ShardingRule _shardingRule;
//     [SetUp]
//     public void Setup()
//     {
//         _sqlCommandParser = new SqlCommandParser(new MySqlParserConfiguration());
//         
//         var tableMetaDatas = new Dictionary<string, TableMetaData>();
//         tableMetaDatas.Add("SysUserMod",new TableMetaData(new List<ColumnMetaData>()
//         {
//             new ColumnMetaData("id",0,"varchar",true,false,true),
//             new ColumnMetaData("name",1,"varchar",false,false,true),
//             new ColumnMetaData("age",2,"int",false,false,true),
//         },new List<IndexMetaData>()));
//         _schemaMetaData=new SchemaMetaData(tableMetaDatas);
//         _shardingRouteDecorator=new ShardingRouteDecorator();
//         var dataSourceMetas = new DataSourceMetas(new MySqlDatabaseType(), new Dictionary<string, DatabaseAccessConfiguration>());
//         _openConnectorMetaData=new OpenConnectorMetaData(dataSourceMetas,_schemaMetaData);
//         
//         //2、分库分表配置
//         ShardingRuleConfiguration shardingRuleConfig = new ShardingRuleConfiguration();
//         //2.2、配置各个表的分库分表策略，这里只配了一张表的就是t_order
//         shardingRuleConfig.TableRuleConfigs.Add(CreateSysUserModTableRule());
//         //2.5、配置默认分表规则
//         shardingRuleConfig.DefaultTableShardingStrategyConfig = new NoneShardingStrategyConfiguration();
//         //2.6、配置默认分库规则(不配置分库规则,则只采用分表规则)
//         shardingRuleConfig.DefaultDatabaseShardingStrategyConfig = new NoneShardingStrategyConfiguration();
//         //2.7、配置默认数据源
//         shardingRuleConfig.DefaultDataSourceName = "ds0";
//         _shardingRule= new ShardingRule(shardingRuleConfig, new List<string>() { "ds0" });
//     }
//     static TableRuleConfiguration CreateSysUserModTableRule()
//         {
//             TableRuleConfiguration result = new TableRuleConfiguration("SysUserMod", new List<string>()
//             {
//                 "ds0.SysUserMod_00",
//                 "ds0.SysUserMod_01",
//                 "ds0.SysUserMod_02"
//             });
//             //1、指定逻辑索引(oracle/PostgreSQL需要配置)
//             //        result.setLogicIndex("order_id");
//             result.DatabaseShardingStrategyConfig = new NoneShardingStrategyConfiguration();
//             //4、配置分库策略,缺省表示使用默认分库策略
//             //result.setDatabaseShardingStrategyConfig(new InlineShardingStrategyConfiguration("user_id", "ds${user_id % 2}"));
//             //result.setDatabaseShardingStrategyConfig(new HintShardingStrategyConfiguration(new OrderDataBaseHintShardingAlgorithm()));
//             //5、配置分表策略,缺省表示使用默认分表策略
//             result.TableShardingStrategyConfig = new StandardShardingStrategyConfiguration("Id", new SysUserModId());
//             //result.setTableShardingStrategyConfig(new InlineShardingStrategyConfiguration("order_id", "t_order_${order_id % 2}"));
//             //result.setTableShardingStrategyConfig(new StandardShardingStrategyConfiguration("order_id",new orderPreciseShardingAlgorithm(),new orderRangeShardingAlgorithm()));
//             //result.setTableShardingStrategyConfig(new ComplexShardingStrategyConfiguration("order_id,user_id",new orderComplexKeysShardingAlgorithm()));
//             //result.setTableShardingStrategyConfig(new HintShardingStrategyConfiguration(new OrderTableHintShardingAlgorithm()));
//             //6、指定自增字段以及key的生成方式
//             //result.setKeyGeneratorColumnName("order_id");
//             //result.setKeyGenerator(new DefaultKeyGenerator());
//             // result.KeyGeneratorConfig = new KeyGeneratorConfiguration("id", "id");
//             return result;
//         }
//
//
//     [Test]
//     public void Test1()
//     {
//         var parameterContext = new ParameterContext();
//         var sql = "select * from SysUserMod WHERE ID='123'";
//         var sqlCommand = _sqlCommandParser.Parse(sql,false);
//         // ISqlCommandContext<ISqlCommand> sqlCommandContext = SqlCommandContextFactory.Create(_metaData.Schema, sql, parameterContext, sqlCommand);
//         ISqlCommandContext<ISqlCommand> sqlCommandContext = SqlCommandContextFactory.Create(_schemaMetaData, sql, parameterContext, sqlCommand);
//         var routeContext = new RouteContext(sqlCommandContext, parameterContext, new RouteResult());
//         Assert.NotNull(routeContext);
//         var decorateRouteContext = _shardingRouteDecorator.Decorate(routeContext, _openConnectorMetaData, _shardingRule,
//             new ConfigurationProperties());
//     }
// }
// public class SysUserModId : IPreciseShardingAlgorithm<string>
// {
//     public string DoSharding(ICollection<string> availableTargetNames, PreciseShardingValue shardingValue)
//     {
//         foreach (var name in availableTargetNames)
//         {
//             if (name.EndsWith($"{(int.Parse((string)shardingValue.Value) % availableTargetNames.Count)}"))
//                 return name;
//         }
//
//         return null;
//     }
// }