using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParser.Command.DML;
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
        Assert.Pass();
    }
}