using OpenConnector.Extensions;
using NCDC.Sharding.Executors.Context;
using NCDC.Sharding.Executors.SqlLog;
using NCDC.Sharding.Rewrites.Abstractions;
using NCDC.Sharding.Rewrites.ParameterRewriters.ParameterBuilders;
using NCDC.Sharding.Rewrites.Sql.Impl;
using NCDC.Sharding.Routes;
using NCDC.ShardingAdoNet;

namespace NCDC.ShardingRewrite;

public sealed class ShardingExecutionContextFactory:IShardingExecutionContextFactory
{
    public ShardingExecutionContext Create(RouteContext routeContext, SqlRewriteContext sqlRewriteContext)
    {
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