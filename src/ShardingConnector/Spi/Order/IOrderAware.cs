using System;
using System.Collections.Generic;
using System.Text;

namespace ShardingConnector.Spi.Order
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/5 13:03:55
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public interface IOrderAware<T>
    {
        int GetOrder();
        /// <summary>
        /// 泛型类型
        /// </summary>
        /// <returns></returns>
        Type GetGenericType();
    }
}
