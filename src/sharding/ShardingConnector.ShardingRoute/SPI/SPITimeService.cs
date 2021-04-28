using System;
using System.Collections.Generic;
using System.Text;

namespace ShardingConnector.ShardingRoute.SPI
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/28 13:52:14
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class SPITimeService
    {
        private readonly ICollection<ITimeService> timeServices = NewInstanceServiceLoader.NewServiceInstances<ITimeService>();

        static SPITimeService()
        {
            NewInstanceServiceLoader.Register<ITimeService>();
            _instance = new SPITimeService();
        }

        private static SPITimeService _instance;
        public static SPITimeService GetInstance()
        {
            return _instance;
        }

        public DateTime? GetTime()
        {
            DateTime? result = null;
            foreach (var timeService in timeServices)
            {
                result = timeService.GetTime();
                if (!(timeService is DefaultTimeService))
                {
                    return result;
                }
            }
            return result;
        }
    }
}
