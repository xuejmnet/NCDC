using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParser.SqlParseEngines;
using OpenConnector.CommandParserBinder;
using OpenConnector.CommandParserBinder.Command;
using OpenConnector.CommandParserBinder.MetaData;
using OpenConnector.CommandParserBinder.MetaData.Column;
using OpenConnector.CommandParserBinder.MetaData.Index;
using OpenConnector.CommandParserBinder.MetaData.Schema;
using OpenConnector.CommandParserBinder.MetaData.Table;
using OpenConnector.MySqlParser;
using OpenConnector.Route.Context;
using OpenConnector.ShardingAdoNet;

namespace OpenConnector.RouteTest;

public class Tests
{
    private ISqlCommandParser _sqlCommandParser;
    private SchemaMetaData _schemaMetaData;
    private ITableMetadataManager _tableMetadataManager;
    [SetUp]
    public void Setup()
    {
        _sqlCommandParser = new SqlCommandParser(new MySqlParserConfiguration());
        
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
        var parameterContext = new ParameterContext();
        var sql = "select * from test";
        var sqlCommand = _sqlCommandParser.Parse(sql,false);
        // ISqlCommandContext<ISqlCommand> sqlCommandContext = SqlCommandContextFactory.Create(_metaData.Schema, sql, parameterContext, sqlCommand);
        ISqlCommandContext<ISqlCommand> sqlCommandContext = SqlCommandContextFactory.Create(_tableMetadataManager, sql, parameterContext, sqlCommand);
        var routeContext = new RouteContext(sqlCommandContext, parameterContext, new RouteResult());
        Assert.NotNull(routeContext);
    }
}