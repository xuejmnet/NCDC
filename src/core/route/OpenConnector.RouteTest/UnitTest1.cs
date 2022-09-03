using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParser.SqlParseEngines;
using OpenConnector.CommandParserBinder;
using OpenConnector.CommandParserBinder.Command;
using OpenConnector.MySqlParser;
using OpenConnector.Route.Context;
using OpenConnector.ShardingAdoNet;

namespace OpenConnector.RouteTest;

public class Tests
{
    private ISqlCommandParser _sqlCommandParser;
    [SetUp]
    public void Setup()
    {
        _sqlCommandParser = new SqlCommandParser(new MySqlParserConfiguration());
    }

    [Test]
    public void Test1()
    {
        var parameterContext = new ParameterContext();
        var sql = "select * from test";
        var sqlCommand = _sqlCommandParser.Parse(sql,false);
        // ISqlCommandContext<ISqlCommand> sqlCommandContext = SqlCommandContextFactory.Create(_metaData.Schema, sql, parameterContext, sqlCommand);
        ISqlCommandContext<ISqlCommand> sqlCommandContext = SqlCommandContextFactory.Create(null, sql, parameterContext, sqlCommand);
        var routeContext = new RouteContext(sqlCommandContext, parameterContext, new RouteResult());
        Assert.NotNull(routeContext);
    }
}