using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using OpenConnector.CommandParser.Abstractions;
using OpenConnector.CommandParser.Command;
using OpenConnector.CommandParser.Command.DML;
using OpenConnector.CommandParser.Segment.DML.Assignment;
using OpenConnector.CommandParser.Segment.DML.Expr;
using OpenConnector.CommandParser.Segment.DML.Expr.Simple;
using OpenConnector.CommandParser.Segment.DML.Predicate;
using OpenConnector.CommandParser.Segment.DML.Predicate.Value;
using OpenConnector.Exceptions;
using OpenConnector.Extensions;
using OpenConnector.ShardingAdoNet;
using OpenConnector.ShardingCommon.Core.Rule;

namespace OpenConnector.ShardingRoute.Engine.Validator.Impl
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/29 13:14:07
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ShardingUpdateCommandValidator : IShardingCommandValidator<UpdateCommand>
    {
        public void Validate(ShardingRule shardingRule, UpdateCommand sqlCommand, ParameterContext parameterContext)
        {
            String tableName = sqlCommand.Tables.First().GetTableName().GetIdentifier().GetValue();
            foreach (var assignmentSegment in sqlCommand.SetAssignment.GetAssignments())
            {
                String shardingColumn = assignmentSegment.GetColumn().GetIdentifier().GetValue();
                if (shardingRule.IsShardingColumn(shardingColumn, tableName))
                {
                    var shardingColumnSetAssignmentValue = GetShardingColumnSetAssignmentValue(assignmentSegment, parameterContext);
                    object shardingValue = null;
                    var whereSegmentOptional = sqlCommand.Where;
                    if (whereSegmentOptional != null)
                    {
                        shardingValue = GetShardingValue(whereSegmentOptional, parameterContext, shardingColumn);
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

        private object GetShardingColumnSetAssignmentValue(AssignmentSegment assignmentSegment, ParameterContext parameterContext)
        {
            var segment = assignmentSegment.GetValue();
            if (segment is ParameterMarkerExpressionSegment parameterMarkerExpressionSegment)
            {
                return parameterContext.GetParameterValue(parameterMarkerExpressionSegment.GetParameterName());
            }
            if (segment is LiteralExpressionSegment literalExpressionSegment)
            {
                return literalExpressionSegment.GetLiterals();
            }
            // if (-1 == shardingValueParameterMarkerIndex || shardingValueParameterMarkerIndex > parameters.Count - 1)
            // {
            //     return null;
            // }
            return null;
        }

        private object GetShardingValue(WhereSegment whereSegment, ParameterContext parameterContext, String shardingColumn)
        {
            foreach (var andPredicate in whereSegment.GetAndPredicates())
            {
                return GetShardingValue(andPredicate, parameterContext, shardingColumn);
            }
            return null;
        }

        private object GetShardingValue(AndPredicateSegment andPredicate, ParameterContext parameterContext, String shardingColumn)
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
                    return GetPredicateCompareShardingValue(segment, parameterContext);
                }
                if (rightValue is PredicateInRightValue predicateInRightValue)
                {
                    ICollection<IExpressionSegment> segments = predicateInRightValue.SqlExpressions;
                    return GetPredicateInShardingValue(segments, parameterContext);
                }
            }
            return null;
        }

        private object GetPredicateCompareShardingValue(IExpressionSegment segment, ParameterContext parameterContext)
        {
            // int shardingValueParameterMarkerIndex;
            if (segment is ParameterMarkerExpressionSegment parameterMarkerExpressionSegment)
            {
                // shardingValueParameterMarkerIndex = parameterMarkerExpressionSegment.GetParameterMarkerIndex();
                // if (-1 == shardingValueParameterMarkerIndex || shardingValueParameterMarkerIndex > parameters.Count - 1)
                // {
                //     return null;
                // }
                return parameterContext.GetParameterValue(parameterMarkerExpressionSegment.GetParameterName());
            }
            if (segment is LiteralExpressionSegment literalExpressionSegment)
            {
                return literalExpressionSegment.GetLiterals();
            }
            return null;
        }

        private object GetPredicateInShardingValue(ICollection<IExpressionSegment> segments, ParameterContext parameterContext)
        {
            // int shardingColumnWhereIndex;
            foreach (var segment in segments)
            {

                if (segment is ParameterMarkerExpressionSegment parameterMarkerExpressionSegment)
                {
                    // shardingColumnWhereIndex = parameterMarkerExpressionSegment.GetParameterMarkerIndex();
                    // if (-1 == shardingColumnWhereIndex || shardingColumnWhereIndex > parameters.Count - 1)
                    // {
                    //     continue;
                    // }
                    return parameterContext.GetParameterValue(parameterMarkerExpressionSegment.GetParameterName());
                }
                if (segment is LiteralExpressionSegment literalExpressionSegment)
                {
                    return literalExpressionSegment.GetLiterals();
                }
            }
            return null;
        }

        public void Validate(ShardingRule shardingRule, ISqlCommand sqlCommand,ParameterContext parameterContext)
        {
            Validate(shardingRule, (UpdateCommand)sqlCommand, parameterContext);
        }
    }
}
