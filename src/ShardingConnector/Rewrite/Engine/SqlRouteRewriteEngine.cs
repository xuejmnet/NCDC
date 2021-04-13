using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ShardingConnector.Common.Rule;
using ShardingConnector.Kernels.Route;
using ShardingConnector.Kernels.Route.Context;
using ShardingConnector.Rewrite.Context;
using ShardingConnector.Rewrite.Parameter.Builder.Impl;

namespace ShardingConnector.Rewrite.Engine
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/13 17:32:01
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class SqlRouteRewriteEngine
    {
        /**
         * Rewrite SQL and parameters.
         *
         * @param sqlRewriteContext SQL rewrite context
         * @param routeResult route result
         * @return SQL map of route unit and rewrite result
         */
        public IDictionary<RouteUnit, SqlRewriteResult> rewrite(SqlRewriteContext sqlRewriteContext, RouteResult routeResult)
        {
            IDictionary<RouteUnit, SqlRewriteResult> result = new Dictionary<RouteUnit, SqlRewriteResult>();
            for (Kernels.Route.Context.RouteUnit each : routeResult.getRouteUnits())
            {
                result.put(each, new SQLRewriteResult(new RouteSQLBuilder(sqlRewriteContext, each).toSQL(), getParameters(sqlRewriteContext.getParameterBuilder(), routeResult, each)));
            }
            foreach (var routeUnit in routeResult.GetRouteUnits())
            {
                result.Add(routeUnit,new SqlRewriteResult(new rout));
            }
            return result;
        }

        private List<Object> getParameters(final ParameterBuilder parameterBuilder, final RouteResult routeResult, final RouteUnit routeUnit)
        {
            if (parameterBuilder instanceof StandardParameterBuilder || routeResult.getOriginalDataNodes().isEmpty() || parameterBuilder.getParameters().isEmpty()) {
                return parameterBuilder.getParameters();
            }
            List<Object> result = new LinkedList<>();
            int count = 0;
            for (Collection<> DataNode> each : routeResult.getOriginalDataNodes())
            {
                if (isInSameDataNode(each, routeUnit))
                {
                    result.addAll(((GroupedParameterBuilder)parameterBuilder).getParameters(count));
                }
                count++;
            }
            return result;
        }

        private boolean isInSameDataNode(final Collection<DataNode> dataNodes, final RouteUnit routeUnit)
        {
            if (dataNodes.isEmpty())
            {
                return true;
            }
            for (DataNode each : dataNodes)
            {
                if (routeUnit.findTableMapper(each.getDataSourceName(), each.getTableName()).isPresent())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
