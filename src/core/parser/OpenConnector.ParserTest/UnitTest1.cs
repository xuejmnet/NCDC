using OpenConnector.Api.Database;
using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParser.Command.DML;
using OpenConnector.CommandParser.Constant;
using OpenConnector.CommandParser.SqlParseEngines;
using OpenConnector.CommandParserBinder;
using OpenConnector.CommandParserBinder.Command.DML;
using OpenConnector.CommandParserBinder.MetaData;
using OpenConnector.CommandParserBinder.MetaData.Column;
using OpenConnector.CommandParserBinder.MetaData.Index;
using OpenConnector.CommandParserBinder.MetaData.Schema;
using OpenConnector.CommandParserBinder.MetaData.Table;
using OpenConnector.MySqlParser;
using OpenConnector.ShardingAdoNet;

namespace OpenConnector.ParserTest;

public class Tests
{
    private ISqlCommandParser _sqlCommandParser;
    private SchemaMetaData _schemaMetaData;
    private ITableMetadataManager _tableMetadataManager;
    [SetUp]
    public void Setup()
    {
        var mySqlParserConfiguration = new MySqlParserConfiguration();
        _sqlCommandParser = new SqlCommandParser(mySqlParserConfiguration);
        var tableMetaDatas = new Dictionary<string, TableMetaData>();
        tableMetaDatas.Add("test",new TableMetaData(new List<ColumnMetaData>()
        {
            new ColumnMetaData("id",0,"varchar",true,false,true),
            new ColumnMetaData("name",1,"varchar",false,false,true),
            new ColumnMetaData("age",2,"int",false,false,true),
        },new List<IndexMetaData>()));
        _schemaMetaData=new SchemaMetaData(tableMetaDatas);
        _tableMetadataManager = new TableMetadataManager();
        var tableMetadata = new TableMetadata("test",new Dictionary<string, ColumnMetadata>()
        {
            {"id",new ColumnMetadata("id",0,"varchar",true,false,true)},
            {"name",new ColumnMetadata("name",1,"varchar",false,false,true)},
            {"age",new ColumnMetadata("age",2,"int",false,false,true)},
        });
        _tableMetadataManager.AddTableMetadata(tableMetadata);
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
    [Test]
    public void Test2()
    {
        var sqlCommand = _sqlCommandParser.Parse("select id,name,age from test",false);
        Assert.True(sqlCommand is SelectCommand);
        var sqlCommandContext = SqlCommandContextFactory.Create(_tableMetadataManager, "select id,name,age from test", new ParameterContext(), sqlCommand);
        Assert.True(sqlCommandContext is SelectCommandContext);
    }
}