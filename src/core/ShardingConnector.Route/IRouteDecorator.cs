using ShardingConnector.Common.Config.Properties;
using ShardingConnector.Common.MetaData;
using ShardingConnector.Common.Rule;
using ShardingConnector.Route.Context;
using ShardingConnector.Spi.Order;

namespace ShardingConnector.Route
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/5 14:29:59
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public interface IRouteDecorator:IOrderAware
    {
        RouteContext Decorate(RouteContext routeContext, ShardingConnectorMetaData metaData, IBaseRule rule, ConfigurationProperties properties);
    }
    public interface IRouteDecorator<in T>: IRouteDecorator where T: IBaseRule
    {
        RouteContext Decorate(RouteContext routeContext, ShardingConnectorMetaData metaData, T rule, ConfigurationProperties properties);
    }
}
