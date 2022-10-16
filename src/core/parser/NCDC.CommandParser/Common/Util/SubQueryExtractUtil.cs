using NCDC.CommandParser.Common.Command.DML;
using NCDC.CommandParser.Common.Constant;
using NCDC.CommandParser.Common.Segment.DML.Expr;
using NCDC.CommandParser.Common.Segment.DML.Expr.SubQuery;
using NCDC.CommandParser.Common.Segment.DML.Item;
using NCDC.CommandParser.Common.Segment.Generic.Table;
using NCDC.Extensions;

namespace NCDC.CommandParser.Common.Util;

public static class SubQueryExtractUtil
{
    /**
     * Get subquery segment from SelectCommand.
     *
     * @param selectCommand SelectCommand
     * @return subquery segment collection
     */
    public static IEnumerable<SubQuerySegment> GetSubQuerySegments(SelectCommand selectCommand)
    {
        ICollection<SubQuerySegment> result = new LinkedList<SubQuerySegment>();
        ExtractSubQuerySegments(result, selectCommand);
        return result;
    }

    private static void ExtractSubQuerySegments(ICollection<SubQuerySegment> result, SelectCommand selectCommand)
    {
        ExtractSubQuerySegmentsFromProjections(result, selectCommand.Projections);
        ExtractSubQuerySegmentsFromTableSegment(result, selectCommand.From);
        if (selectCommand.Where is not null)
        {
            ExtractSubQuerySegmentsFromExpression(result, selectCommand.Where.Expr);
        }

        if (selectCommand.Combine is not null)
        {
            ExtractSubQuerySegments(result, selectCommand.Combine.SelectCommand);
        }
    }

    private static void ExtractSubQuerySegmentsFromProjections(ICollection<SubQuerySegment> result,
        ProjectionsSegment? projections)
    {
        if (null == projections || projections.Projections.IsEmpty())
        {
            return;
        }

        foreach (var projectionsProjection in projections.Projections)
        {
            if (projectionsProjection is SubQueryProjectionSegment subQueryProjectionSegment)
            {
                SubQuerySegment subQuery = subQueryProjectionSegment.SubQuery;
                subQuery.SubQueryType = SubQueryTypeEnum.PROJECTION_SUB_QUERY;
                result.Add(subQuery);
                ExtractSubQuerySegments(result, subQuery.Select);
            }
        }
    }

    private static void ExtractSubQuerySegmentsFromTableSegment(ICollection<SubQuerySegment> result,
        ITableSegment? tableSegment)
    {
        if (null == tableSegment)
        {
            return;
        }

        if (tableSegment is SubQueryTableSegment subQueryTableSegment)
        {
            SubQuerySegment subQuery = subQueryTableSegment.SubQuery;
            subQuery.SubQueryType = SubQueryTypeEnum.TABLE_SUB_QUERY;
            result.Add(subQuery);
            ExtractSubQuerySegments(result, subQuery.Select);
        }

        if (tableSegment is JoinTableSegment joinTableSegment)
        {
            ExtractSubQuerySegmentsFromTableSegment(result, joinTableSegment.Left);
            ExtractSubQuerySegmentsFromTableSegment(result, joinTableSegment.Right);
        }
    }

    private static void ExtractSubQuerySegmentsFromExpression(ICollection<SubQuerySegment> result,
        IExpressionSegment expressionSegment)
    {
        if (expressionSegment is SubQueryExpressionSegment subQueryExpressionSegment)
        {
            SubQuerySegment subQuery = subQueryExpressionSegment.SubQuery;
            subQuery.SubQueryType = SubQueryTypeEnum.PREDICATE_SUB_QUERY;
            result.Add(subQuery);
            ExtractSubQuerySegments(result, subQuery.Select);
        }

        if (expressionSegment is ListExpression listExpression)
        {
            foreach (var listExpressionItem in listExpression.Items)
            {
                ExtractSubQuerySegmentsFromExpression(result, listExpressionItem);
            }
        }

        if (expressionSegment is BinaryOperationExpression binaryOperationExpression)
        {
            ExtractSubQuerySegmentsFromExpression(result, binaryOperationExpression.Left);
            ExtractSubQuerySegmentsFromExpression(result, binaryOperationExpression.Right);
        }

        if (expressionSegment is InExpression inExpression)
        {
            ExtractSubQuerySegmentsFromExpression(result, inExpression.Left);
            ExtractSubQuerySegmentsFromExpression(result, inExpression.Right);
        }

        if (expressionSegment is BetweenExpression betweenExpression)
        {
            ExtractSubQuerySegmentsFromExpression(result, betweenExpression.BetweenExpr);
            ExtractSubQuerySegmentsFromExpression(result, betweenExpression.AndExpr);
        }

        if (expressionSegment is ExistsSubQueryExpression existsSubQueryExpression)
        {
            SubQuerySegment subQuery = existsSubQueryExpression.SubQuery;
            subQuery.SubQueryType = SubQueryTypeEnum.EXISTS_SUB_QUERY;
            result.Add(subQuery);
            ExtractSubQuerySegments(result, subQuery.Select);
        }
    }
}