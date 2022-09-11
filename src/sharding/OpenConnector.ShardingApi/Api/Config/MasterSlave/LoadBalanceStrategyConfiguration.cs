using System.Collections.Generic;
using NCDC.Common.Config;

namespace OpenConnector.ShardingApi.Api.Config.MasterSlave
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/14 10:17:48
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class LoadBalanceStrategyConfiguration:TypeBasedSPIConfiguration
    {
        public LoadBalanceStrategyConfiguration(string type) : base(type)
        {
        }

        public LoadBalanceStrategyConfiguration(string type, IDictionary<string, object> properties) : base(type, properties)
        {
        }
    }
}
