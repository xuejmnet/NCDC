using System.Collections.Generic;
using System.Data.Common;
using OpenConnector.Common.Rule;
using OpenConnector.Extensions;
using OpenConnector.RewriteEngine.Context;
using OpenConnector.RewriteEngine.Parameter.Builder;
using OpenConnector.RewriteEngine.Parameter.Builder.Impl;
using OpenConnector.RewriteEngine.Sql.Impl;
using OpenConnector.Route.Context;
using OpenConnector.ShardingAdoNet;

namespace OpenConnector.RewriteEngine.Engine
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
        public IDictionary<RouteUnit, SqlRewriteResult> Rewrite(SqlRewriteContext sqlRewriteContext, RouteResult routeResult)
        {
            IDictionary<RouteUnit, SqlRewriteResult> result = new Dictionary<RouteUnit, SqlRewriteResult>();
            foreach (var routeUnit in routeResult.GetRouteUnits())
            {
                result.Add(routeUnit,new SqlRewriteResult(new RouteSqlBuilder(sqlRewriteContext,routeUnit).ToSql(),GetParameterContext(sqlRewriteContext.GetParameterBuilder(), routeResult, routeUnit)));
            }
            return result;
        }

        private ParameterContext GetParameterContext(IParameterBuilder parameterBuilder,RouteResult routeResult, RouteUnit routeUnit)
        {
            if (parameterBuilder is StandardParameterBuilder || routeResult.GetOriginalDataNodes().IsEmpty() || parameterBuilder.GetParameterContext().IsEmpty()) {
                return parameterBuilder.GetParameterContext();
            }
            var result = new ParameterContext();
            int count = 0;
            foreach (var originalDataNode in routeResult.GetOriginalDataNodes())
            {
                if (IsInSameDataNode(originalDataNode, routeUnit))
                {
                    result.AddParameters(((GroupedParameterBuilder)parameterBuilder).GetParameterContext(count).GetDbParameters());
                }
                count++;
            }
            return result;
        }

        private bool IsInSameDataNode(ICollection<DataNode> dataNodes, RouteUnit routeUnit)
        {
            if (dataNodes.IsEmpty())
            {
                return true;
            }
            foreach (var dataNode in dataNodes)
            {
                if (routeUnit.FindTableMapper(dataNode.GetDataSourceName(), dataNode.GetTableName())!=null)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
