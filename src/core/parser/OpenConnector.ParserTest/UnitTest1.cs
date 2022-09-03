using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParser.Command.DML;
using OpenConnector.CommandParser.Constant;
using OpenConnector.CommandParser.SqlParseEngines;
using OpenConnector.MySqlParser;

namespace OpenConnector.ParserTest;

public class Tests
{
    private ISqlCommandParser _sqlCommandParser;
    [SetUp]
    public void Setup()
    {
        var mySqlParserConfiguration = new MySqlParserConfiguration();
        _sqlCommandParser = new SqlCommandParser(mySqlParserConfiguration);
    }

    [Test]
    public void Test1()
    {
        var sqlCommand = _sqlCommandParser.Parse("select * from test",false);
        Assert.True(sqlCommand is SelectCommand);
        var sqlCommand1 = _sqlCommandParser.Parse("select id,name,age from test",false);
        Assert.True(sqlCommand1 is SelectCommand);
        var selectCommand1 = (SelectCommand)sqlCommand1;
        Assert.True(3==selectCommand1.Projections.GetProjections().Count);
    }
}