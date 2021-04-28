using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ShardingConnector.CommandParser.Segment.DML.Expr.Simple;
using ShardingConnector.ParserBinder.Command.DML;
using ShardingConnector.ParserBinder.Segment.Insert.Keygen;
using ShardingConnector.ParserBinder.Segment.Insert.Values;
using ShardingConnector.ShardingCommon.Core.Rule;
using ShardingConnector.ShardingCommon.Core.Strategy.Route.Value;

namespace ShardingConnector.ShardingRoute.Engine.Condition.Engine
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/28 11:44:07
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class InsertClauseShardingConditionEngine
    {
        private readonly ShardingRule shardingRule;
    
    /**
     * Create sharding conditions.
     * 
     * @param insertStatementContext insert statement context
     * @param parameters SQL parameters
     * @return sharding conditions
     */
    public List<ShardingCondition> createShardingConditions(InsertCommandContext insertCommandContext, List<object> parameters)
        {
            ICollection<ShardingCondition> result = new LinkedList<ShardingCondition>();
            String tableName = insertStatementContext.getSqlStatement().getTable().getTableName().getIdentifier().getValue();
            Collection<String> columnNames = getColumnNames(insertStatementContext);
            for (InsertValueContext each : insertStatementContext.getInsertValueContexts())
            {
                result.add(createShardingCondition(tableName, columnNames.iterator(), each, parameters));
            }
            Optional<GeneratedKeyContext> generatedKey = insertStatementContext.getGeneratedKeyContext();
            if (generatedKey.isPresent() && generatedKey.get().isGenerated())
            {
                generatedKey.get().getGeneratedValues().addAll(getGeneratedKeys(tableName, insertStatementContext.getSqlStatement().getValueListCount()));
                if (shardingRule.isShardingColumn(generatedKey.get().getColumnName(), tableName))
                {
                    appendGeneratedKeyCondition(generatedKey.get(), tableName, result);
                }
            }
            return result;
        }

        private Collection<String> getColumnNames(InsertStatementContext insertStatementContext)
        {
            Optional<GeneratedKeyContext> generatedKey = insertStatementContext.getGeneratedKeyContext();
            if (generatedKey.isPresent() && generatedKey.get().isGenerated())
            {
                Collection<String> result = new LinkedList<>(insertStatementContext.getColumnNames());
                result.remove(generatedKey.get().getColumnName());
                return result;
            }
            return insertStatementContext.getColumnNames();
        }

        private ShardingCondition createShardingCondition(String tableName, Iterator<String> columnNames, InsertValueContext insertValueContext, List<Object> parameters)
        {
            ShardingCondition result = new ShardingCondition();
            SPITimeService timeService = new SPITimeService();
            for (ExpressionSegment each : insertValueContext.getValueExpressions())
            {
                String columnName = columnNames.next();
                if (shardingRule.isShardingColumn(columnName, tableName))
                {
                    if (each instanceof SimpleExpressionSegment) {
                result.getRouteValues().add(new ListRouteValue<>(columnName, tableName, Collections.singletonList(getRouteValue((SimpleExpressionSegment)each, parameters))));
            } else if (ExpressionConditionUtils.isNowExpression(each))
            {
                result.getRouteValues().add(new ListRouteValue<>(columnName, tableName, Collections.singletonList(timeService.getTime())));
            }
            else if (ExpressionConditionUtils.isNullExpression(each))
            {
                throw new ShardingSphereException("Insert clause sharding column can't be null.");
            }
        }
    }
        return result;
    }

private Comparable<?> getRouteValue(SimpleExpressionSegment expressionSegment, List<Object> parameters)
{
    Object result;
    if (expressionSegment instanceof ParameterMarkerExpressionSegment) {
    result = parameters.get(((ParameterMarkerExpressionSegment)expressionSegment).getParameterMarkerIndex());
} else
{
    result = ((LiteralExpressionSegment)expressionSegment).getLiterals();
}
Preconditions.checkArgument(result instanceof Comparable, "Sharding value must implements Comparable.");
return (Comparable)result;
    }
    
    private Collection<Comparable<?>> getGeneratedKeys(String tableName, int valueListCount)
{
    return IntStream.range(0, valueListCount).mapToObj(i->shardingRule.generateKey(tableName)).collect(Collectors.toCollection(LinkedList::new));
}

private void appendGeneratedKeyCondition(GeneratedKeyContext generatedKey, String tableName, List<ShardingCondition> shardingConditions)
{
    Iterator < Comparable <?>> generatedValuesIterator = generatedKey.getGeneratedValues().iterator();
    for (ShardingCondition each : shardingConditions) {
    each.getRouteValues().add(new ListRouteValue<>(generatedKey.getColumnName(), tableName, Collections.< Comparable <?>> singletonList(generatedValuesIterator.next())));
}
    }
    }
}
