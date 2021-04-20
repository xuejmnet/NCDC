using System;
using ShardingConnector.Common.Rule;
using ShardingConnector.Spi.Order;

namespace ShardingConnector.Merge.Engine
{
/*
* @Author: xjm
* @Description:
* @Date: Sunday, 18 April 2021 19:56:27
* @Email: 326308290@qq.com
*/
    public interface IResultProcessEngine<out T>:IOrderAware where T:IBaseRule
    {
        
    }
}