using System;
using System.Collections.Generic;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.Extensions;
using ShardingConnector.ParserBinder.Command;
using ShardingConnector.ParserBinder.Command.DML;
using ShardingConnector.ParserBinder.Segment.Select.Pagination;
using ShardingConnector.RewriteEngine.Parameter.Builder;
using ShardingConnector.RewriteEngine.Parameter.Builder.Impl;
using ShardingConnector.RewriteEngine.Parameter.Rewrite;
using ShardingConnector.RewriteEngine.Sql.Token.Generator.Aware;
using ShardingConnector.Route.Context;

namespace ShardingConnector.ShardingRewrite.Parameter.Impl
{
/*
* @Author: xjm
* @Description:
* @Date: Monday, 26 April 2021 20:34:43
* @Email: 326308290@qq.com
*/
    public sealed class ShardingPaginationParameterRewriter : IParameterRewriter<SelectCommandContext>,IRouteContextAware
    {
        private RouteContext routeContext;

        public bool IsNeedRewrite(ISqlCommandContext<ISqlCommand> sqlCommandContext)
        {
            return sqlCommandContext is SelectCommandContext selectCommandContext
                   && selectCommandContext.GetPaginationContext().HasPagination() && !routeContext.GetRouteResult().IsSingleRouting();
        }

        public void Rewrite(IParameterBuilder parameterBuilder, ISqlCommandContext<ISqlCommand> sqlCommandContext, List<object> parameters)
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

        public void SetRouteContext(RouteContext routeContext)
        {
            this.routeContext = routeContext;
        }
    }
}