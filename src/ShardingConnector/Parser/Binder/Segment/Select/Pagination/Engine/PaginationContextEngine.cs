using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShardingConnector.Parser.Binder.Segment.Select.Projection;
using ShardingConnector.Parser.Sql.Command.DML;
using ShardingConnector.Parser.Sql.Segment.DML.Pagination.limit;
using ShardingConnector.Parser.Sql.Segment.DML.Pagination.Top;
using ShardingConnector.Parser.Sql.Segment.Predicate;

namespace ShardingConnector.Parser.Binder.Segment.Select.Pagination.Engine
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
        public PaginationContext CreatePaginationContext(SelectCommand selectCommand, ProjectionsContext projectionsContext, List<Object> parameters)
        {
            var limitSegment = selectCommand.Limit;
            if (limitSegment != null)
            {
                return new LimitPaginationContextEngine().CreatePaginationContext(limitSegment, parameters);
            }
            var topProjectionSegment = FindTopProjection(selectCommand);
            var whereSegment = selectCommand.Where;
            if (topProjectionSegment != null)
            {
                return new TopPaginationContextEngine().CreatePaginationContext(
                    topProjectionSegment, whereSegment?.GetAndPredicates() ?? new List<AndPredicateSegment>(0), parameters);
            }
            if (whereSegment != null)
            {
                return new RowNumberPaginationContextEngine().CreatePaginationContext(whereSegment.GetAndPredicates(), projectionsContext, parameters);
            }
            return new PaginationContext(null, null, parameters);
        }

        private TopProjectionSegment FindTopProjection(SelectCommand selectCommand)
        {
            return selectCommand.Projections.GetProjections().OfType<TopProjectionSegment>().FirstOrDefault();
        }
    }
}