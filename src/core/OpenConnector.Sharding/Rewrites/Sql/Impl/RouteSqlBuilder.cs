using OpenConnector.Sharding.Rewrites.Sql.Token.SimpleObject;
using OpenConnector.Sharding.Routes;

namespace OpenConnector.Sharding.Rewrites.Sql.Impl
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 13 April 2021 21:03:09
* @Email: 326308290@qq.com
*/
    public class RouteSqlBuilder:AbstractSqlBuilder
    {
        private readonly RouteUnit _routeUnit;
        public RouteSqlBuilder(SqlRewriteContext context, RouteUnit routeUnit) : base(context)
        {
            _routeUnit = routeUnit;
        }

        protected override string GetSqlTokenText(SqlToken sqlToken)
        {
            if (sqlToken is IRouteUnitAware routeUnitAware) {
                return routeUnitAware.ToString(_routeUnit);
            }
            return sqlToken.ToString();
        }
    }
}