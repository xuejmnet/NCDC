using NCDC.Basic.Configurations;
using NCDC.Basic.TableMetadataManagers;
using NCDC.CommandParser.Abstractions;
using NCDC.Extensions;
using NCDC.ShardingAdoNet;
using NCDC.ShardingParser;
using NCDC.ShardingParser.Abstractions;
using NCDC.ShardingParser.Command;
using NCDC.ShardingRewrite;
using NCDC.ShardingRewrite.Abstractions;
using NCDC.ShardingRewrite.ParameterRewriters.ParameterBuilders;
using NCDC.ShardingRewrite.Sql.Impl;
using NCDC.ShardingRoute;
using NCDC.ShardingRoute.Abstractions;

namespace NCDC.ProxyServer.Executors;

public sealed class ShardingExecutionContextFactory : IShardingExecutionContextFactory
{
    private readonly ISqlCommandParser _sqlCommandParser;
    private readonly ISqlCommandContextFactory _sqlCommandContextFactory;
    private readonly IRouteContextFactory _routeContextFactory;
    private readonly ISqlRewriterContextFactory _sqlRewriterContextFactory;
    private readonly ITableMetadataManager _tableMetadataManager;
    private readonly ShardingConfiguration _shardingConfiguration;

    public ShardingExecutionContextFactory(ISqlCommandParser sqlCommandParser,
        ISqlCommandContextFactory sqlCommandContextFactory, IRouteContextFactory routeContextFactory,
        ISqlRewriterContextFactory sqlRewriterContextFactory,ITableMetadataManager tableMetadataManager,ShardingConfiguration shardingConfiguration)
    {
        _sqlCommandParser = sqlCommandParser;
        _sqlCommandContextFactory = sqlCommandContextFactory;
        _routeContextFactory = routeContextFactory;
        _sqlRewriterContextFactory = sqlRewriterContextFactory;
        _tableMetadataManager = tableMetadataManager;
        _shardingConfiguration = shardingConfiguration;
    }

    public ShardingExecutionContext Create(string sql)
    {
        var sqlCommand = _sqlCommandParser.Parse(sql, false);
        return Create(sql, sqlCommand);
    }

    public ShardingExecutionContext Create(string sql, ISqlCommand sqlCommand)
    {
        var sqlCommandContext = _sqlCommandContextFactory.Create(sql, ParameterContext.Empty, sqlCommand);
        var shardingExecutionContext = Create0(sql,sqlCommandContext);
        if (true)
        {
            SqlLogger.LogSql(sql, false, shardingExecutionContext.GetSqlCommandContext(), shardingExecutionContext.GetExecutionUnits());
        }

        return shardingExecutionContext;
    }

    private ShardingExecutionContext Create0(string sql, ISqlCommandContext<ISqlCommand> sqlCommandContext)
    {
        var sqlParserResult = new SqlParserResult(sql, sqlCommandContext, ParameterContext.Empty,_tableMetadataManager);
        if (!sqlParserResult.NativeSql)
        {
            if (sqlParserResult.DefaultDataSourceExecute)
            {
                ShardingExecutionContext result = new ShardingExecutionContext(sqlCommandContext);
                result.GetExecutionUnits().Add(new ExecutionUnit(_shardingConfiguration.DefaultDataSourceName,new SqlUnit(sql,ParameterContext.Empty)));
                return result;
            }
            else
            {
                var routeContext = _routeContextFactory.Create(sqlParserResult);
                var sqlRewriteContext = _sqlRewriterContextFactory.Rewrite(sqlParserResult, routeContext);

                ICollection<ExecutionUnit> executionUnits = new LinkedList<ExecutionUnit>();
                var sqlRewriteResults = GetRewriteResults(sqlRewriteContext, routeContext.GetRouteResult());
                foreach (var sqlRewriteResult in sqlRewriteResults)
                {
                    executionUnits.Add(new ExecutionUnit(sqlRewriteResult.Key.DataSource,
                        new SqlUnit(sqlRewriteResult.Value.Sql, sqlRewriteResult.Value.ParameterContext)));
                }
                ShardingExecutionContext result = new ShardingExecutionContext(routeContext.GetSqlCommandContext());
                result.GetExecutionUnits().AddAll(executionUnits);
                return result;
            }
        }
        else
        {
            //广播所有datasource
            ShardingExecutionContext result = new ShardingExecutionContext(sqlCommandContext);
            result.GetExecutionUnits().AddAll(_shardingConfiguration.GetAllDataSources().Select(o=>new ExecutionUnit(o,new SqlUnit(sql,ParameterContext.Empty))));
            return result;
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