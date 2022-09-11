using System;

namespace OpenConnector
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/16 00:00:00
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    /// <summary>
    /// 
    /// </summary>
    
    public  class UtcTime
    {
        private UtcTime(){}
        private static readonly long UtcStartTicks = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;
        public static long CurrentTimeMillis()
        {
            return (DateTime.UtcNow.Ticks - UtcStartTicks) / 10000;
        }
    }
}