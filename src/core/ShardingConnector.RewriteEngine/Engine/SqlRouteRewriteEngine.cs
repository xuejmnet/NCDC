using System.Collections.Generic;
using System.Data.Common;
using ShardingConnector.Common.Rule;
using ShardingConnector.Extensions;
using ShardingConnector.RewriteEngine.Context;
using ShardingConnector.RewriteEngine.Parameter.Builder;
using ShardingConnector.RewriteEngine.Parameter.Builder.Impl;
using ShardingConnector.RewriteEngine.Sql.Impl;
using ShardingConnector.Route.Context;

namespace ShardingConnector.RewriteEngine.Engine
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
                result.Add(routeUnit,new SqlRewriteResult(new RouteSqlBuilder(sqlRewriteContext,routeUnit).ToSql(),GetParameters(sqlRewriteContext.GetParameterBuilder(), routeResult, routeUnit)));
            }
            return result;
        }

        private IDictionary<string,DbParameter> GetParameters(IParameterBuilder parameterBuilder,RouteResult routeResult, RouteUnit routeUnit)
        {
            if (parameterBuilder is StandardParameterBuilder || routeResult.GetOriginalDataNodes().IsEmpty() || parameterBuilder.GetParameters().IsEmpty()) {
                return parameterBuilder.GetParameters();
            }
            var result = new Dictionary<string,DbParameter>();
            int count = 0;
            foreach (var originalDataNode in routeResult.GetOriginalDataNodes())
            {
                if (IsInSameDataNode(originalDataNode, routeUnit))
                {
                    result.AddAll(((GroupedParameterBuilder)parameterBuilder).GetParameters(count));
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
