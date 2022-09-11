using NCDC.Sharding.Routes;

namespace NCDC.ShardingRewrite.Sql.Token.SimpleObject
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 13 April 2021 21:05:09
* @Email: 326308290@qq.com
*/
    public interface IRouteUnitAware
    {
        string ToString(RouteUnit routeUnit);
    }
}