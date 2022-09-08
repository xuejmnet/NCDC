using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParser.Command.DML;
using OpenConnector.CommandParser.Segment.DML.Predicate;
using OpenConnector.CommandParser.Segment.DML.Predicate.Value;
using OpenConnector.CommandParserBinder.Command;
using OpenConnector.CommandParserBinder.Command.DML;
using OpenConnector.CommandParserBinder.MetaData;
using OpenConnector.Exceptions;
using OpenConnector.Sharding.Expressions;
using OpenConnector.ShardingAdoNet;
using OpenConnector.Shardings;

namespace OpenConnector.Sharding;

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

    public RoutePredicateExpression GetRouteParseExpression(ISqlCommandContext<ISqlCommand> sqlCommandContext,
        ParameterContext parameterContext)
    {
        DoResolve(sqlCommandContext, parameterContext);
        return _where;
    }

    private void DoResolve(ISqlCommandContext<ISqlCommand> sqlCommandContext, ParameterContext parameterContext)
    {
        if (!(sqlCommandContext.GetSqlCommand() is DMLCommand))
        {
            throw new ShardingException($"sql command not {nameof(DMLCommand)} cant resolve route");
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
                    whereSegment.GetAndPredicates(), parameterContext);
            }
        }
    }

    private void CreateShardingConditions(
        ISqlCommandContext<ISqlCommand> sqlCommandContext, ICollection<AndPredicateSegment> andPredicates,
        ParameterContext parameterContext)
    {
        foreach (var andPredicate in andPredicates)
        {
            foreach (var predicate in andPredicate.GetPredicates())
            {
                var columnName = predicate.GetColumn().GetIdentifier().GetValue();
                if (_tableMetadata.IsShardingColumn(columnName, _shardingTableRoute))
                {
                    continue;
                }

                if (predicate.GetPredicateRightValue() is PredicateCompareRightValue predicateCompareRightValue)
                {
                    var routePredicateExpression = CompareOperatorPredicateGenerator.Instance.Get(_keyTranslateFilter,
                        columnName, predicateCompareRightValue, parameterContext);
                    _where = _where.And(routePredicateExpression);
                }

                if (predicate.GetPredicateRightValue() is PredicateInRightValue predicateInRightValue)
                {
                    var routePredicateExpression = InOperatorPredicateGenerator.Instance.Get(_keyTranslateFilter,
                        columnName, predicateInRightValue, parameterContext);
                    _where = _where.And(routePredicateExpression);
                }

                if (predicate.GetPredicateRightValue() is PredicateBetweenRightValue predicateBetweenRightValue)
                {
                    var routePredicateExpression = BetweenOperatorPredicateGenerator.Instance.Get(_keyTranslateFilter,
                        columnName, predicateBetweenRightValue, parameterContext);
                    _where = _where.And(routePredicateExpression);
                }
            }
        }
    }
}