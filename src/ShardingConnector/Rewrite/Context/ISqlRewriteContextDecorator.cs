using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Common.Config.Properties;
using ShardingConnector.Kernels.Route.Rule;
using ShardingConnector.Spi.Order;

namespace ShardingConnector.Rewrite.Context
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/12 15:59:18
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public interface ISqlRewriteContextDecorator<T>:IOrderAware<T> where T:IBaseRule
    {
        void Decorate(T rule, ConfigurationProperties properties, SqlRewriteContext sqlRewriteContext);
    }
}
