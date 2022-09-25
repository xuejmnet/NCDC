// using Microsoft.Extensions.DependencyInjection;
// using MySqlConnector;
// using NCDC.Basic.Metadatas;
// using NCDC.Basic.TableMetadataManagers;
// using NCDC.Enums;
// using NCDC.ShardingRoute.Abstractions;
// using NCDC.ShardingRoute.TableRoutes.Abstractions;
//
// namespace NCDC.ShardingTest;
//
// public class Tests
// {
//     private IRuntimeContext _runtimeContext;
//
//     [SetUp]
//     public void Setup()
//     {
//         var logicDatabase = new LogicDatabase("a");
//         logicDatabase.AddDataSource("ds0", "123", MySqlConnectorFactory.Instance, true);
//         var shardingRuntimeContext = new ShardingRuntimeContext("a");
//         shardingRuntimeContext.Services.AddSingleton<ILogicDatabase>(logicDatabase);
//         shardingRuntimeContext.Services.AddSingleton<ITableMetadataManager,TableMetadataManager>();
//         shardingRuntimeContext.Services.AddSingleton<IShardingExecutionContextFactory,TestShardingExecutionContextFactory>();
//         shardingRuntimeContext.Services.AddShardingParser();
//         shardingRuntimeContext.Services.AddMySqlParser();
//         shardingRuntimeContext.Services.AddShardingRoute();
//         shardingRuntimeContext.Services.AddShardingRewrite();
//         _runtimeContext = shardingRuntimeContext;
//         _runtimeContext.Build();
//         
//         
//         var tableMetadata = new TableMetadata("test", new Dictionary<string, ColumnMetadata>()
//         {
//             { "id", new ColumnMetadata("id", 0, "varchar", true, false, true) },
//             { "name", new ColumnMetadata("name", 1, "varchar", false, false, true) },
//             { "age", new ColumnMetadata("age", 2, "int", false, false, true) },
//         });
//         tableMetadata.SetShardingTableColumn("id");
//         tableMetadata.AddActualTableWithDataSource("ds0","test_00");
//         tableMetadata.AddActualTableWithDataSource("ds0","test_01");
//         tableMetadata.AddActualTableWithDataSource("ds0","test_02");
//         var tableMetadataManager = _runtimeContext.GetTableMetadataManager();
//         tableMetadataManager.AddTableMetadata(tableMetadata);
//         var testModTableRoute = _runtimeContext.CreateInstance<TestModTableRoute>();
//         var tableRouteManager = _runtimeContext.GetRequiredService<ITableRouteManager>();
//         tableRouteManager.AddRoute(testModTableRoute);
//     }
//
//     [Test]
//     public void Test1()
//     {
//         var sql = "select * from test where (id='12' or id='13') and (name='1' or name='2' or name='3')";
//         var shardingExecutionContextFactory = _runtimeContext.GetShardingExecutionContextFactory();
//         var shardingExecutionContext = shardingExecutionContextFactory.Create(sql);
//     }
//
//     public class TestModTableRoute : ShardingTableRoute
//     {
//         public TestModTableRoute(ITableMetadataManager tableMetadataManager) : base(tableMetadataManager)
//         {
//         }
//
//         public override string TableName => "test";
//         public override Func<string, bool> GetRouteToFilter(IComparable shardingValue, ShardingOperatorEnum shardingOperator)
//         {
//             var tail = FormatTableName(shardingValue);
//             var table = $"{TableName}{GetTableMetadata().TableSeparator}{tail}";
//             
//             switch (shardingOperator)
//             {
//                 case ShardingOperatorEnum.EQUAL: return t => t.EndsWith(table);
//                 default:
//                 {
//                     return t => true;
//                 }
//             }
//             
//         }
//
//         public string FormatTableName(IComparable shardingValue)
//         {
//             var shardingKey = $"{shardingValue}";
//             var stringHashCode = GetStringHashCode(shardingKey)%3;
//             return stringHashCode.ToString().PadLeft(2, '0');
//         }
//         public static int GetStringHashCode(string value)
//         {
//             Check.NotNull(value, nameof(value));
//             int h = 0; // 默认值是0
//             if (value.Length > 0)
//             {
//                 for (int i = 0; i < value.Length; i++)
//                 {
//                     h = 31 * h + value[i]; // val[0]*31^(n-1) + val[1]*31^(n-2) + ... + val[n-1]
//                 }
//             }
//             return h;
//         }
//     }
// }

