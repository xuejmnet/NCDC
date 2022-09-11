using NCDC.CommandParser.Abstractions;
using NCDC.CommandParserBinder.Command;
using NCDC.CommandParserBinder.Command.DML;
using NCDC.CommandParserBinder.Segment.Select.Pagination;
using OpenConnector.Extensions;
using NCDC.Sharding.Rewrites.Abstractions;
using NCDC.Sharding.Rewrites.ParameterRewriters.ParameterBuilders;
using NCDC.Sharding.Routes;
using NCDC.ShardingAdoNet;

namespace NCDC.Sharding.Rewrites.ParameterRewriters.Parameters
{
/*
* @Author: xjm
* @Description:
* @Date: Monday, 26 April 2021 20:34:43
* @Email: 326308290@qq.com
*/
    public sealed class ShardingPaginationParameterRewriter : IParameterRewriter<SelectCommandContext>
    {
        private readonly RouteContext _routeContext;

        public ShardingPaginationParameterRewriter(RouteContext routeContext)
        {
            _routeContext = routeContext;
        }

        public bool IsNeedRewrite(ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            return sqlCommandContext is SelectCommandContext selectCommandContext
                   && selectCommandContext.GetPaginationContext().HasPagination() && !_routeContext.GetRouteResult().IsSingleRouting();
        }

        public void Rewrite(IParameterBuilder parameterBuilder, ISqlCommandContext<ISqlCommand> sqlCommandContext, ParameterContext parameterContext)
        {
            var selectCommandContext = ((SelectCommandContext) sqlCommandContext);
            var pagination = selectCommandContext.GetPaginationContext();
            pagination.GetOffsetParameterIndex().IfPresent(offsetParameterIndex => RewriteOffset(pagination, offsetParameterIndex.Value, (StandardParameterBuilder) parameterBuilder));
            pagination.GetRowCountParameterIndex().IfPresent(
                rowCountParameterIndex => rewriteRowCount(pagination, rowCountParameterIndex.Value, (StandardParameterBuilder) parameterBuilder, selectCommandContext));
        }

        private void RewriteOffset(PaginationContext pagination, int offsetParameterIndex, StandardParameterBuilder parameterBuilder)
        {
            parameterBuilder.AddReplacedParameters(offsetParameterIndex, pagination.GetRevisedOffset());
        }

        private void rewriteRowCount(PaginationContext pagination, int rowCountParameterIndex, StandardParameterBuilder parameterBuilder, SelectCommandContext selectCommandContext)
        {
            parameterBuilder.AddReplacedParameters(rowCountParameterIndex, pagination.GetRevisedRowCount(selectCommandContext));
        }
    }
}