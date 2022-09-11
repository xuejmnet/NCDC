using MySqlConnector;
using OpenConnector;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.SqlParseEngines;
using NCDC.CommandParserBinder;
using NCDC.CommandParserBinder.MetaData;
using NCDC.Configuration;
using NCDC.Configuration.Metadatas;
using OpenConnector.Extensions;
using OpenConnector.MySqlParser;
using NCDC.Sharding.Rewrites;
using NCDC.Sharding.Rewrites.Abstractions;
using NCDC.Sharding.Rewrites.ParameterRewriters;
using NCDC.Sharding.Routes;
using NCDC.Sharding.Routes.Abstractions;
using NCDC.Sharding.Routes.DataSourceRoutes;
using NCDC.Sharding.Routes.TableRoutes;
using NCDC.ShardingAdoNet;
using ITableRouteManager = NCDC.Sharding.Routes.Abstractions.ITableRouteManager;

namespace NCDC.ShardingTest;

public class Tests
{
    private ISqlCommandParser _sqlCommandParser;
    private ITableMetadataManager _tableMetadataManager;
    private ITableRoute _testModTableRoute;
    private IParameterRewriterBuilder _parameterRewriterBuilder;
    private IShardingSqlRewriter _shardingSqlRewriter;
    private IShardingExecutionContextFactory _shardingExecutionContextFactory;
    private IRouteContextFactory _routeContextFactory;
    private IDataSourceRouteRuleEngine _dataSourceRouteRuleEngine;
    private ITableRouteRuleEngine _tableRouteRuleEngine;
    private IDataSourceRouteManager _dataSourceRouteManager;
    private ILogicDatabase _logicDatabase;
    private ITableRouteManager _tableRouteManager;

    [SetUp]
    public void Setup()
    {
        _logicDatabase = new LogicDatabase("a");
        _logicDatabase.AddDataSource("ds0", "123", MySqlConnectorFactory.Instance, true);
        _sqlCommandParser = new SqlCommandParser(new MySqlParserConfiguration());
        _tableMetadataManager = new TableMetadataManager();
        var tableMetadata = new TableMetadata("test", new Dictionary<string, ColumnMetadata>()
        {
            { "id", new ColumnMetadata("id", 0, "varchar", true, false, true) },
            { "name", new ColumnMetadata("name", 1, "varchar", false, false, true) },
            { "age", new ColumnMetadata("age", 2, "int", false, false, true) },
        });
        tableMetadata.SetShardingTableColumn("id");
        tableMetadata.AddActualTableWithDataSource("ds0","test_00");
        tableMetadata.AddActualTableWithDataSource("ds0","test_01");
        tableMetadata.AddActualTableWithDataSource("ds0","test_02");
        _tableMetadataManager.AddTableMetadata(tableMetadata);
        _testModTableRoute=new TestModTableRoute(_tableMetadataManager);
        _parameterRewriterBuilder = new ShardingParameterRewriterBuilder();
        _shardingSqlRewriter = new ShardingSqlRewriter(_tableMetadataManager, _parameterRewriterBuilder);
        _shardingExecutionContextFactory = new ShardingExecutionContextFactory();
        _dataSourceRouteManager = new DataSourceRouteManager(_tableMetadataManager, _logicDatabase);
        _dataSourceRouteRuleEngine =
            new DataSourceRouteRuleEngine(_tableMetadataManager, _logicDatabase, _dataSourceRouteManager);
        _tableRouteManager = new TableRouteManager(_tableMetadataManager, _logicDatabase);
        _tableRouteManager.AddRoute(_testModTableRoute);
        _tableRouteRuleEngine = new TableRouteRuleEngine(_tableMetadataManager, _logicDatabase, _tableRouteManager);
        _routeContextFactory = new RouteContextFactory(_dataSourceRouteRuleEngine, _tableRouteRuleEngine);
    }

    [Test]
    public void Test1()
    {
        var sql = "select * from test where (id='12' or id='13') and (name='1' or name='2' or name='3')";
        var sqlCommand = _sqlCommandParser.Parse(sql,false);
        var sqlCommandContext = SqlCommandContextFactory.Create(_tableMetadataManager, sql, ParameterContext.Empty, sqlCommand);
        var sqlParserResult = new SqlParserResult(sql,sqlCommandContext,ParameterContext.Empty);
        var routeContext = _routeContextFactory.Create(sqlParserResult);
        var sqlRewriteContext = _shardingSqlRewriter.Rewrite(sqlParserResult,routeContext);
        var shardingExecutionContext = _shardingExecutionContextFactory.Create(routeContext,sqlRewriteContext);
    }

    public class TestModTableRoute : AbstractOperatorTableRoute
    {
        public TestModTableRoute(ITableMetadataManager tableMetadataManager) : base(tableMetadataManager)
        {
        }

        public override string TableName => "test";
        public override Func<string, bool> GetRouteToFilter(IComparable shardingValue, ShardingOperatorEnum shardingOperator)
        {
            var tail = FormatTableName(shardingValue);
            var table = $"{TableName}{GetTableMetadata().TableSeparator}{tail}";
            
            switch (shardingOperator)
            {
                case ShardingOperatorEnum.EQUAL: return t => t.EndsWith(table);
                default:
                {
                    return t => true;
                }
            }
            
        }

        public string FormatTableName(IComparable shardingValue)
        {
            var shardingKey = $"{shardingValue}";
            var stringHashCode = GetStringHashCode(shardingKey)%3;
            return stringHashCode.ToString().PadLeft(2, '0');
        }
        public static int GetStringHashCode(string value)
        {
            Check.NotNull(value, nameof(value));
            int h = 0; // 默认值是0
            if (value.Length > 0)
            {
                for (int i = 0; i < value.Length; i++)
                {
                    h = 31 * h + value[i]; // val[0]*31^(n-1) + val[1]*31^(n-2) + ... + val[n-1]
                }
            }
            return h;
        }
    }
}