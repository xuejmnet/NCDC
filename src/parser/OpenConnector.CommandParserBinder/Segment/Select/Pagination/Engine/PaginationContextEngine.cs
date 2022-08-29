using OpenConnector.CommandParser.Command.DML;
using OpenConnector.CommandParser.Segment.DML.Pagination.Top;
using OpenConnector.CommandParser.Segment.Predicate;
using OpenConnector.CommandParserBinder.Segment.Select.Projection;
using OpenConnector.ShardingAdoNet;

namespace OpenConnector.CommandParserBinder.Segment.Select.Pagination.Engine
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
        public PaginationContext CreatePaginationContext(SelectCommand selectCommand, ProjectionsContext projectionsContext,ParameterContext parameterContext)
        {
            var limitSegment = selectCommand.Limit;
            if (limitSegment != null)
            {
                return new LimitPaginationContextEngine().CreatePaginationContext(limitSegment, parameterContext);
            }
            var topProjectionSegment = FindTopProjection(selectCommand);
            var whereSegment = selectCommand.Where;
            if (topProjectionSegment != null)
            {
                return new TopPaginationContextEngine().CreatePaginationContext(
                    topProjectionSegment, whereSegment?.GetAndPredicates() ?? new List<AndPredicateSegment>(0), parameterContext);
            }
            if (whereSegment != null)
            {
                return new RowNumberPaginationContextEngine().CreatePaginationContext(whereSegment.GetAndPredicates(), projectionsContext, parameterContext);
            }
            return new PaginationContext(null, null, parameterContext);
        }

        private TopProjectionSegment FindTopProjection(SelectCommand selectCommand)
        {
            return selectCommand.Projections.GetProjections().OfType<TopProjectionSegment>().FirstOrDefault();
        }
    }
}