using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using ShardingConnector.CommandParser.Command;
using ShardingConnector.CommandParser.Command.DML;
using ShardingConnector.CommandParser.Segment.DML.Assignment;
using ShardingConnector.CommandParser.Segment.DML.Expr;
using ShardingConnector.CommandParser.Segment.DML.Expr.Simple;
using ShardingConnector.CommandParser.Segment.DML.Predicate.Value;
using ShardingConnector.CommandParser.Segment.Predicate;
using ShardingConnector.Exceptions;
using ShardingConnector.Extensions;
using ShardingConnector.ShardingCommon.Core.Rule;

namespace ShardingConnector.ShardingRoute.Engine.Validator.Impl
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/29 13:14:07
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ShardingUpdateCommandValidator : IShardingStatementValidator<UpdateCommand>
    {
        public void Validate(ShardingRule shardingRule, UpdateCommand sqlCommand, List<object> parameters)
        {
            String tableName = sqlCommand.Tables.First().GetTableName().GetIdentifier().GetValue();
            foreach (var assignmentSegment in sqlCommand.SetAssignment.GetAssignments())
            {
                String shardingColumn = assignmentSegment.GetColumn().GetIdentifier().GetValue();
                if (shardingRule.IsShardingColumn(shardingColumn, tableName))
                {
                    var shardingColumnSetAssignmentValue = GetShardingColumnSetAssignmentValue(assignmentSegment, parameters);
                    object shardingValue = null;
                    var whereSegmentOptional = sqlCommand.Where;
                    if (whereSegmentOptional != null)
                    {
                        shardingValue = GetShardingValue(whereSegmentOptional, parameters, shardingColumn);
                    }
                    if (shardingColumnSetAssignmentValue != null && shardingValue != null && shardingColumnSetAssignmentValue.Equals(shardingValue))
                    {
                        continue;
                    }

                    throw new ShardingException(
                        $"Can not update sharding key, logic table: [{tableName}], column: [{assignmentSegment}].");
                }
            }
        }

        private object GetShardingColumnSetAssignmentValue(AssignmentSegment assignmentSegment, List<object> parameters)
        {
            var segment = assignmentSegment.GetValue();
            int shardingSetAssignIndex = -1;
            if (segment is ParameterMarkerExpressionSegment parameterMarkerExpressionSegment)
            {
                shardingSetAssignIndex = parameterMarkerExpressionSegment.GetParameterMarkerIndex();
            }
            if (segment is LiteralExpressionSegment literalExpressionSegment)
            {
                return literalExpressionSegment.GetLiterals();
            }
            if (-1 == shardingSetAssignIndex || shardingSetAssignIndex > parameters.Count - 1)
            {
                return null;
            }
            return parameters[shardingSetAssignIndex];
        }

        private object GetShardingValue(WhereSegment whereSegment, List<object> parameters, String shardingColumn)
        {
            foreach (var andPredicate in whereSegment.GetAndPredicates())
            {
                return GetShardingValue(andPredicate, parameters, shardingColumn);
            }
            return null;
        }

        private object GetShardingValue(AndPredicateSegment andPredicate, List<object> parameters, String shardingColumn)
        {
            foreach (var predicate in andPredicate.GetPredicates())
            {
                if (!shardingColumn.EqualsIgnoreCase(predicate.GetColumn().GetIdentifier().GetValue()))
                {
                    continue;
                }
                IPredicateRightValue rightValue = predicate.GetPredicateRightValue();
                if (rightValue is PredicateCompareRightValue predicateCompareRightValue)
                {
                    var segment = predicateCompareRightValue.GetExpression();
                    return GetPredicateCompareShardingValue(segment, parameters);
                }
                if (rightValue is PredicateInRightValue predicateInRightValue)
                {
                    ICollection<IExpressionSegment> segments = predicateInRightValue.SqlExpressions;
                    return GetPredicateInShardingValue(segments, parameters);
                }
            }
            return null;
        }

        private object GetPredicateCompareShardingValue(IExpressionSegment segment, List<object> parameters)
        {
            int shardingValueParameterMarkerIndex;
            if (segment is ParameterMarkerExpressionSegment parameterMarkerExpressionSegment)
            {
                shardingValueParameterMarkerIndex = parameterMarkerExpressionSegment.GetParameterMarkerIndex();
                if (-1 == shardingValueParameterMarkerIndex || shardingValueParameterMarkerIndex > parameters.Count - 1)
                {
                    return null;
                }
                return parameters[shardingValueParameterMarkerIndex];
            }
            if (segment is LiteralExpressionSegment literalExpressionSegment)
            {
                return literalExpressionSegment.GetLiterals();
            }
            return null;
        }

        private object GetPredicateInShardingValue(ICollection<IExpressionSegment> segments, List<object> parameters)
        {
            int shardingColumnWhereIndex;
            foreach (var segment in segments)
            {

                if (segment is ParameterMarkerExpressionSegment parameterMarkerExpressionSegment)
                {
                    shardingColumnWhereIndex = parameterMarkerExpressionSegment.GetParameterMarkerIndex();
                    if (-1 == shardingColumnWhereIndex || shardingColumnWhereIndex > parameters.Count - 1)
                    {
                        continue;
                    }

                    return parameters[shardingColumnWhereIndex];
                }
                if (segment is LiteralExpressionSegment literalExpressionSegment)
                {
                    return literalExpressionSegment.GetLiterals();
                }
            }
            return null;
        }

        public void Validate(ShardingRule shardingRule, ISqlCommand sqlCommand, List<object> parameters)
        {
            Validate(shardingRule, (UpdateCommand)sqlCommand, parameters);
        }
    }
}
