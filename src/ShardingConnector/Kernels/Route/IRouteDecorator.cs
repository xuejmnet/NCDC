using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Common.Config.Properties;
using ShardingConnector.Common.MetaData;
using ShardingConnector.Common.Rule;
using ShardingConnector.Spi.Order;

namespace ShardingConnector.Kernels.Route
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/5 14:29:59
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public interface IRouteDecorator<T>:IOrderAware where T: IBaseRule
    {
        RouteContext Decorate(RouteContext routeContext, ShardingConnectorMetaData metaData, T rule, ConfigurationProperties properties);
    }
}
