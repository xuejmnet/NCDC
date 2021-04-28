using System;
using System.Collections.Generic;
using System.Text;

namespace ShardingConnector.ShardingRoute.SPI
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/28 13:51:08
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class DefaultTimeService:ITimeService
    {
        public DateTime GetTime()
        {
            return DateTime.Now;
        }
    }
}
