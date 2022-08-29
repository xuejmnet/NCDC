using System;
using OpenConnector.Common.Rule;
using OpenConnector.Spi.Order;

namespace OpenConnector.Merge.Engine
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: Sunday, 18 April 2021 19:56:27
    * @Email: 326308290@qq.com
    */
    public interface IResultProcessEngine : IOrderAware
    {

    }
    public interface IResultProcessEngine<in T> : IResultProcessEngine where T : IBaseRule
    {

    }
}