using OpenConnector.Common.Config.Properties;
using OpenConnector.Common.MetaData;
using OpenConnector.Common.Rule;
using OpenConnector.Route.Context;
using OpenConnector.Spi.Order;

namespace OpenConnector.Route
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
        RouteContext Decorate(RouteContext routeContext, OpenConnectorMetaData metaData, IBaseRule rule, ConfigurationProperties properties);
    }
    public interface IRouteDecorator<in T>: IRouteDecorator where T: IBaseRule
    {
        RouteContext Decorate(RouteContext routeContext, OpenConnectorMetaData metaData, T rule, ConfigurationProperties properties);
    }
}
