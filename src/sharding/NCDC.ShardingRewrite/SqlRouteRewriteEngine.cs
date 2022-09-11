using NCDC.ShardingRewrite.Sql.Impl;
using NCDC.ShardingRoute;

namespace NCDC.ShardingRewrite;


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
                var sql = new RouteSqlBuilder(sqlRewriteContext,routeUnit).ToSql();
                result.Add(routeUnit,new SqlRewriteResult(sql,sqlRewriteContext.GetParameterContext()));
            }
            return result;
        }

        // private ParameterContext GetParameterContext(IParameterBuilder parameterBuilder,RouteResult routeResult, RouteUnit routeUnit)
        // {
        //     if (parameterBuilder is StandardParameterBuilder || parameterBuilder.GetParameterContext().IsEmpty()) {
        //         return parameterBuilder.GetParameterContext();
        //     }
        //     var result = new ParameterContext();
        //     int count = 0;
        //     foreach (var originalDataNode in routeResult.GetOriginalDataNodes())
        //     {
        //         if (IsInSameDataNode(originalDataNode, routeUnit))
        //         {
        //             result.AddParameters(((GroupedParameterBuilder)parameterBuilder).GetParameterContext(count).GetDbParameters());
        //         }
        //         count++;
        //     }
        //     return result;
        // }
        //
        // private bool IsInSameDataNode(ICollection<DataNode> dataNodes, RouteUnit routeUnit)
        // {
        //     if (dataNodes.IsEmpty())
        //     {
        //         return true;
        //     }
        //     foreach (var dataNode in dataNodes)
        //     {
        //         if (routeUnit.FindTableMapper(dataNode.GetDataSourceName(), dataNode.GetTableName())!=null)
        //         {
        //             return true;
        //         }
        //     }
        //     return false;
        // }
    }