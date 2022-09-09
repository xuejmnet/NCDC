using OpenConnector;
using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParser.SqlParseEngines;
using OpenConnector.CommandParserBinder;
using OpenConnector.CommandParserBinder.MetaData;
using OpenConnector.MySqlParser;
using OpenConnector.Sharding.Routes;
using OpenConnector.Sharding.Routes.Abstractions;
using OpenConnector.Sharding.Routes.DataSourceRoutes;
using OpenConnector.Sharding.Routes.TableRoutes;
using OpenConnector.ShardingAdoNet;
using OpenConnector.Shardings;

namespace NCDC.ShardingTest;

public class Tests
{
    private ISqlCommandParser _sqlCommandParser;
    private ITableMetadataManager _tableMetadataManager;
    private ITableRoute _testModTableRoute;

    [SetUp]
    public void Setup()
    {
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
    }

    [Test]
    public void Test1()
    {
        var sql = "select * from test where (id='12' or id='13') and (name='1' or name='2' or name='3')";
        var sqlCommand = _sqlCommandParser.Parse(sql,false);
        var sqlCommandContext = SqlCommandContextFactory.Create(_tableMetadataManager, sql, ParameterContext.Empty, sqlCommand);
        var sqlParserResult = new SqlParserResult(sql,sqlCommandContext,ParameterContext.Empty);
        var dataSourceRouteResult = new DataSourceRouteResult("ds0");

        var tableRouteUnits = _testModTableRoute.Route(dataSourceRouteResult,sqlParserResult);
        // tableRouteUnits.GroupBy(o=>o.DataSourceName).se
        // var routeResult = new RouteResult();
        // new RouteContext(sqlCommandContext,ParameterContext.Empty,)
        
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