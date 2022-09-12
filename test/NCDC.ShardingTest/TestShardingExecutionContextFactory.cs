using NCDC.Basic.Executors;
using NCDC.CommandParser.Abstractions;
using NCDC.Extensions;
using NCDC.ShardingAdoNet;
using NCDC.ShardingParser;
using NCDC.ShardingParser.Abstractions;
using NCDC.ShardingRewrite;
using NCDC.ShardingRewrite.Abstractions;
using NCDC.ShardingRewrite.ParameterRewriters.ParameterBuilders;
using NCDC.ShardingRewrite.Sql.Impl;
using NCDC.ShardingRewrite.SqlLog;
using NCDC.ShardingRoute;
using NCDC.ShardingRoute.Abstractions;
using OpenConnector.Extensions;

namespace NCDC.ShardingTest;

public class TestShardingExecutionContextFactory:IShardingExecutionContextFactory
{
    private readonly ISqlCommandParser _sqlCommandParser;
    private readonly ISqlCommandContextFactory _sqlCommandContextFactory;
    private readonly IRouteContextFactory _routeContextFactory;
    private readonly ISqlRewriterContextFactory _sqlRewriterContextFactory;

    public TestShardingExecutionContextFactory(ISqlCommandParser sqlCommandParser,ISqlCommandContextFactory sqlCommandContextFactory,IRouteContextFactory routeContextFactory,ISqlRewriterContextFactory sqlRewriterContextFactory)
    {
        _sqlCommandParser = sqlCommandParser;
        _sqlCommandContextFactory = sqlCommandContextFactory;
        _routeContextFactory = routeContextFactory;
        _sqlRewriterContextFactory = sqlRewriterContextFactory;
    }
    public ShardingExecutionContext Create(string sql)
    {
        var sqlCommand = _sqlCommandParser.Parse(sql,false);
        var sqlCommandContext = _sqlCommandContextFactory.Create(sql,ParameterContext.Empty, sqlCommand);
        var sqlParserResult = new SqlParserResult(sql,sqlCommandContext,ParameterContext.Empty);
        var routeContext = _routeContextFactory.Create(sqlParserResult);
        var sqlRewriteContext = _sqlRewriterContextFactory.Rewrite(sqlParserResult,routeContext);
      
        ICollection<ExecutionUnit> executionUnits = new LinkedList<ExecutionUnit>();
        var sqlRewriteResults = GetRewriteResults(sqlRewriteContext, routeContext.GetRouteResult());
        foreach (var sqlRewriteResult in sqlRewriteResults)
        {
            executionUnits.Add(new ExecutionUnit(sqlRewriteResult.Key.DataSource, new SqlUnit(sqlRewriteResult.Value.Sql, sqlRewriteResult.Value.ParameterContext)));
        }
        ShardingExecutionContext result = new ShardingExecutionContext(routeContext.GetSqlCommandContext());
        result.GetExecutionUnits().AddAll(executionUnits);
        if (true)
        {
            SqlLogger.LogSql(routeContext.GetSql(),false,result.GetSqlCommandContext(),result.GetExecutionUnits());
        }
        return result;
        
    }
    public IDictionary<RouteUnit, SqlRewriteResult> GetRewriteResults(SqlRewriteContext sqlRewriteContext, RouteResult routeResult)
    {
        IDictionary<RouteUnit, SqlRewriteResult> result = new Dictionary<RouteUnit, SqlRewriteResult>();
        foreach (var routeUnit in routeResult.GetRouteUnits())
        {
            var sql = new RouteSqlBuilder(sqlRewriteContext,routeUnit).ToSql();
            var parameterContext = GetParameterContext(sqlRewriteContext.GetParameterBuilder(),routeResult,routeUnit);
            result.Add(routeUnit,new SqlRewriteResult(sql,parameterContext));
        }
        return result;
    }
    
    private ParameterContext GetParameterContext(IParameterBuilder parameterBuilder,RouteResult routeResult, RouteUnit routeUnit)
    {
        if (parameterBuilder is StandardParameterBuilder || routeResult.GetOriginalDataNodes().IsEmpty() || parameterBuilder.GetParameterContext().IsEmpty()) {
            return parameterBuilder.GetParameterContext();
        }
        var result = new ParameterContext();
        int count = 0;
        foreach (var originalDataNode in routeResult.GetOriginalDataNodes())
        {
            if (IsInSameDataNode(originalDataNode, routeUnit))
            {
                result.AddParameters(((GroupedParameterBuilder)parameterBuilder).GetParameterContext(count).GetDbParameters());
            }
            count++;
        }
        return result;
    }

    private bool IsInSameDataNode(ICollection<DataNode> dataNodes, RouteUnit routeUnit)
    {
        if (dataNodes.IsEmpty())
        {
            return true;
        }
        foreach (var dataNode in dataNodes)
        {
            if (routeUnit.FindTableMapper(dataNode.GetDataSourceName(), dataNode.GetTableName())!=null)
            {
                return true;
            }
        }
        return false;
    }
}