using NCDC.Basic.TableMetadataManagers;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Common.Command;
using NCDC.CommandParser.Common.Command.DML;
using NCDC.CommandParser.Common.Segment.DML.Expr;
using NCDC.CommandParser.Common.Segment.DML.Predicate;
using NCDC.CommandParser.Common.Segment.DML.Predicate.Value;
using NCDC.CommandParser.Common.Util;
using NCDC.Enums;
using NCDC.Exceptions;
using NCDC.ShardingAdoNet;
using NCDC.ShardingParser;
using NCDC.ShardingParser.Command;
using NCDC.ShardingParser.Command.DML;
using NCDC.ShardingRoute.Expressions;
using NCDC.Extensions;
using NCDC.Plugin.Enums;

namespace NCDC.ShardingRoute;

public class SqlRoutePredicateDiscover
{
    private readonly TableMetadata _tableMetadata;
    private readonly Func<IComparable, ShardingOperatorEnum, string, Func<string, bool>> _keyTranslateFilter;
    private readonly bool _shardingTableRoute;

    private RoutePredicateExpression _where = RoutePredicateExpression.Default;

    public SqlRoutePredicateDiscover(TableMetadata tableMetadata,
        Func<IComparable, ShardingOperatorEnum, string, Func<string, bool>> keyTranslateFilter, bool shardingTableRoute)
    {
        _tableMetadata = tableMetadata;
        _keyTranslateFilter = keyTranslateFilter;
        _shardingTableRoute = shardingTableRoute;
    }

    public RoutePredicateExpression GetRouteParseExpression(SqlParserResult sqlParserResult)
    {
        DoResolve(sqlParserResult);
        return _where;
    }

    private void DoResolve(SqlParserResult sqlParserResult)
    {
        var sqlCommandContext = sqlParserResult.SqlCommandContext;
        var parameterContext = sqlParserResult.ParameterContext;
        if (!(sqlCommandContext.GetSqlCommand() is IDMLCommand))
        {
            throw new ShardingException($"sql command not {nameof(IDMLCommand)} cant resolve route");
        }

        if (sqlCommandContext is InsertCommandContext insertCommandContext)
        {
            DoInsertResolve(insertCommandContext, parameterContext);
        }
        else
        {
            DoWhereResolve(sqlCommandContext, parameterContext);
        }
    }

    private void DoInsertResolve(InsertCommandContext insertCommandContext,
        ParameterContext parameterContext)
    {
        throw new ShardingInvalidOperationException("sharding for insert");
        // new InsertClauseShardingConditionCreator().CreateShardingConditions(insertCommandContext,parameterContext,_tableMetadata)
    }

    private void DoWhereResolve(ISqlCommandContext<ISqlCommand> sqlCommandContext,
        ParameterContext parameterContext)
    {
        if (sqlCommandContext is IWhereAvailable whereAvailable)
        {
            var whereSegment = whereAvailable.GetWhere();
            if (whereSegment != null)
            {
                CreateShardingConditions(sqlCommandContext,
                    whereSegment.Expr, parameterContext);
            }
        }
    }

    private void CreateShardingConditions(
        ISqlCommandContext<ISqlCommand> sqlCommandContext, IExpressionSegment expression,
        ParameterContext parameterContext)
    {
        var andPredicates = ExpressionExtractUtil.GetAndPredicateSegments(expression);
        if (andPredicates.IsNotEmpty())
        {
            _where = _where.And(RoutePredicateExpression.DefaultFalse);
            foreach (var andPredicate in andPredicates)
            {
                var where = RoutePredicateExpression.Default;
                foreach (var predicate in andPredicate.Predicates)
                {
                    var columnSegments = ColumnExtractor.Extract(predicate);
                    foreach (var columnSegment in columnSegments)
                    {
                        var columnName = columnSegment.IdentifierValue.Value;
                        if (!_tableMetadata.IsShardingColumn(columnName, _shardingTableRoute))
                        {
                            continue;
                        }

                        if (predicate is BinaryOperationExpression predicateOperationExpression)
                        {
                            var routePredicateExpression = CompareOperatorPredicateGenerator.Instance.Get(
                                _keyTranslateFilter,
                                columnName, predicateOperationExpression, parameterContext);
                            where = where.And(routePredicateExpression);
                        }
                        else if (predicate is InExpression predicateInExpression)
                        {
                            var routePredicateExpression = InOperatorPredicateGenerator.Instance.Get(_keyTranslateFilter,
                                columnName, predicateInExpression, parameterContext);
                            where = where.And(routePredicateExpression);
                        }
                        else if
                            (predicate is BetweenExpression predicateBetweenExpression)
                        {
                            var routePredicateExpression = BetweenOperatorPredicateGenerator.Instance.Get(
                                _keyTranslateFilter,
                                columnName, predicateBetweenExpression, parameterContext);
                            where = where.And(routePredicateExpression);
                        }
                    }
                }

                _where = _where.Or(where);
            }
        }
    }
}