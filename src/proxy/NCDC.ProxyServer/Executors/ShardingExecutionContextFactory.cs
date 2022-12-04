using NCDC.Basic.Configurations;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Common.Command;
using NCDC.CommandParser.Common.Command.DML;
using NCDC.Extensions;
using NCDC.ProxyServer.Abstractions;
using NCDC.ProxyServer.Connection;
using NCDC.ProxyServer.Connection.Abstractions;
using NCDC.ShardingAdoNet;
using NCDC.ShardingParser;
using NCDC.ShardingParser.Abstractions;
using NCDC.ShardingParser.Command;
using NCDC.ShardingParser.MetaData;
using NCDC.ShardingRewrite;
using NCDC.ShardingRewrite.Abstractions;
using NCDC.ShardingRewrite.ParameterRewriters.ParameterBuilders;
using NCDC.ShardingRewrite.Sql.Impl;
using NCDC.ShardingRoute;
using NCDC.ShardingRoute.Abstractions;

namespace NCDC.ProxyServer.Executors;

public sealed class ShardingExecutionContextFactory : IShardingExecutionContextFactory
{
    // private readonly ISqlCommandContextFactory _sqlCommandContextFactory;
    private readonly IRouteContextFactory _routeContextFactory;
    private readonly ISqlRewriterContextFactory _sqlRewriterContextFactory;
    private readonly ITableMetadataManager _tableMetadataManager;
    private readonly ShardingConfiguration _shardingConfiguration;

    public ShardingExecutionContextFactory(
        // ISqlCommandContextFactory sqlCommandContextFactory, 
        IRouteContextFactory routeContextFactory,
        ISqlRewriterContextFactory sqlRewriterContextFactory,ITableMetadataManager tableMetadataManager,ShardingConfiguration shardingConfiguration)
    {
        // _sqlCommandContextFactory = sqlCommandContextFactory;
        _routeContextFactory = routeContextFactory;
        _sqlRewriterContextFactory = sqlRewriterContextFactory;
        _tableMetadataManager = tableMetadataManager;
        _shardingConfiguration = shardingConfiguration;
    }

    public ShardingExecutionContext Create(IQueryContext queryContext)
    {
        var shardingExecutionContext = Create0(queryContext);
        if (true)
        {
            SqlLogger.LogSql(queryContext.Sql, false, shardingExecutionContext.GetSqlCommandContext(), shardingExecutionContext.GetExecutionUnits());
        }
        return shardingExecutionContext;
    }
    private bool IsDefaultDataSourceExecute(ISqlCommandContext<ISqlCommand> sqlCommandContext)
    {
        if (sqlCommandContext.GetSqlCommand() is IDMLCommand)
        {
            var tableNames = sqlCommandContext.GetTablesContext().GetTableNames();
            return !tableNames.Any(o => _tableMetadataManager.IsSharding(o));
        }

        return false;
    }

    private ShardingExecutionContext Create0(IQueryContext queryContext)
    {
        //判断是否是全数据库直接执行
        if (queryContext.DirectAllDataSourceSql)
        {
            //广播所有datasource
            var executionUnits = _shardingConfiguration.GetAllDataSources().Select(o=>new ExecutionUnit(o,new SqlUnit(queryContext.Sql,ParameterContext.Empty))).ToList();
            ShardingExecutionContext result = new ShardingExecutionContext(queryContext.SqlCommandContext,executionUnits);
            return result;
        }
        else
        {
            //默认数据源
            var isDefaultDataSourceExecute = IsDefaultDataSourceExecute(queryContext.SqlCommandContext);
            if (isDefaultDataSourceExecute)
            {
                var executionUnit = new ExecutionUnit(_shardingConfiguration.DefaultDataSourceName,new SqlUnit(queryContext.Sql,ParameterContext.Empty));
                ShardingExecutionContext result = new ShardingExecutionContext(queryContext.SqlCommandContext,executionUnit);
                return result;
            }
            else
            {
                var sqlParserResult = new SqlParserResult(queryContext.Sql,queryContext.SqlCommandContext,queryContext.ParameterContext);
                var routeContext = _routeContextFactory.Create(sqlParserResult);
                var sqlRewriteContext = _sqlRewriterContextFactory.Rewrite(sqlParserResult, routeContext);

                var sqlRewriteResults = GetRewriteResults(sqlRewriteContext, routeContext.GetRouteResult());
                List<ExecutionUnit> executionUnits = new List<ExecutionUnit>(sqlRewriteResults.Count);
                foreach (var sqlRewriteResult in sqlRewriteResults)
                {
                    executionUnits.Add(new ExecutionUnit(sqlRewriteResult.Key.DataSource,
                        new SqlUnit(sqlRewriteResult.Value.Sql, sqlRewriteResult.Value.ParameterContext)));
                }
                ShardingExecutionContext result = new ShardingExecutionContext(routeContext.GetSqlCommandContext(),executionUnits);
                return result;
            }
        }
    }

    public IDictionary<RouteUnit, SqlRewriteResult> GetRewriteResults(SqlRewriteContext sqlRewriteContext,
        RouteResult routeResult)
    {
        IDictionary<RouteUnit, SqlRewriteResult> result = new Dictionary<RouteUnit, SqlRewriteResult>();
        foreach (var routeUnit in routeResult.GetRouteUnits())
        {
            var sql = new RouteSqlBuilder(sqlRewriteContext, routeUnit).ToSql();
            var parameterContext = GetParameterContext(sqlRewriteContext.GetParameterBuilder(), routeResult, routeUnit);
            result.Add(routeUnit, new SqlRewriteResult(sql, parameterContext));
        }

        return result;
    }

    private ParameterContext GetParameterContext(IParameterBuilder parameterBuilder, RouteResult routeResult,
        RouteUnit routeUnit)
    {
        if (parameterBuilder is StandardParameterBuilder || routeResult.GetOriginalDataNodes().IsEmpty() ||
            parameterBuilder.GetParameterContext().IsEmpty())
        {
            return parameterBuilder.GetParameterContext();
        }

        var result = new ParameterContext();
        int count = 0;
        foreach (var originalDataNode in routeResult.GetOriginalDataNodes())
        {
            if (IsInSameDataNode(originalDataNode, routeUnit))
            {
                result.AddParameters(((GroupedParameterBuilder)parameterBuilder).GetParameterContext(count)
                    .GetDbParameters());
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
            if (routeUnit.FindTableMapper(dataNode.GetDataSourceName(), dataNode.GetTableName()) != null)
            {
                return true;
            }
        }

        return false;
    }
}