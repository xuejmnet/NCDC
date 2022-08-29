using System;
using System.Collections.Generic;
using System.Data.Common;
using OpenConnector.CommandParser.Command;
using OpenConnector.Extensions;
using OpenConnector.CommandParserBinder.Command;
using OpenConnector.CommandParserBinder.Command.DML;
using OpenConnector.CommandParserBinder.Segment.Select.Pagination;
using OpenConnector.RewriteEngine.Parameter.Builder;
using OpenConnector.RewriteEngine.Parameter.Builder.Impl;
using OpenConnector.RewriteEngine.Parameter.Rewrite;
using OpenConnector.RewriteEngine.Sql.Token.Generator.Aware;
using OpenConnector.Route.Context;
using OpenConnector.ShardingAdoNet;

namespace OpenConnector.ShardingRewrite.Parameter.Impl
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

        public void SetRouteContext(RouteContext routeContext)
        {
            this.routeContext = routeContext;
        }
    }
}