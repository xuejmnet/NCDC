using System;
using System.Collections.Generic;
using System.Text;

namespace ShardingConnector.ShardingRoute.SPI
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/28 13:50:37
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public interface ITimeService
    {
        DateTime GetTime();
    }
}
