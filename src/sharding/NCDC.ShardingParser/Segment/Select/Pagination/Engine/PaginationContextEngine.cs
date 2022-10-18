using NCDC.CommandParser.Common.Command.DML;
using NCDC.CommandParser.Common.Segment.DML.Expr;
using NCDC.CommandParser.Common.Segment.DML.Pagination.Top;
using NCDC.CommandParser.Common.Segment.DML.Predicate;
using NCDC.CommandParser.Common.Util;
using NCDC.CommandParser.Dialect.Handler.DML;
using NCDC.Extensions;
using NCDC.ShardingParser.Segment.Select.Projection;
using NCDC.ShardingAdoNet;

namespace NCDC.ShardingParser.Segment.Select.Pagination.Engine
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 12:16:34
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class PaginationContextEngine
    {
        public PaginationContext CreatePaginationContext(SelectCommand selectCommand, ProjectionsContext projectionsContext,ParameterContext parameterContext,ICollection<WhereSegment> whereSegments)
        {
            var limitSegment = SelectCommandHandler.GetLimitSegment(selectCommand);
            if (limitSegment != null)
            {
                return new LimitPaginationContextEngine().CreatePaginationContext(limitSegment, parameterContext);
            }
            var topProjectionSegment = FindTopProjection(selectCommand);
            var expressions = whereSegments.Select(o => o.Expr).ToList();
            if (topProjectionSegment != null)
            {
                return new TopPaginationContextEngine().CreatePaginationContext(
                    topProjectionSegment, expressions, parameterContext);
            }
            if (expressions.IsNotEmpty()&&ContainsRowNumberPagination(selectCommand))
            {
                return new RowNumberPaginationContextEngine().CreatePaginationContext(expressions, projectionsContext, parameterContext);
            }
            return new PaginationContext(null, null, parameterContext);
        }
    
        private bool ContainsRowNumberPagination( SelectCommand selectCommand) {
            return false;
        }

        private TopProjectionSegment? FindTopProjection(SelectCommand selectCommand)
        {
            var subQueryTableSegments = SqlUtil.GetSubQueryTableSegmentFromTableSegment(selectCommand.From);
            foreach (var subQueryTableSegment in subQueryTableSegments)
            {
                var subQuerySelect = subQueryTableSegment.SubQuery.Select;
                foreach (var projectionsProjection in subQuerySelect.Projections.Projections)
                {
                    if (projectionsProjection is TopProjectionSegment topProjectionSegment)
                    {
                        return topProjectionSegment;
                    }
                }
            }
            return null;
        }
    }
}