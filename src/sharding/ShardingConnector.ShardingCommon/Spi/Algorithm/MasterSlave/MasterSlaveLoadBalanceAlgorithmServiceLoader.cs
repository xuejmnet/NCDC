using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.ShardingApi.Spi.MasterSlave;
using ShardingConnector.Spi;

namespace ShardingConnector.ShardingCommon.Spi.Algorithm.MasterSlave
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 14:54:46
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class MasterSlaveLoadBalanceAlgorithmServiceLoader: TypeBasedSPIServiceLoader<IMasterSlaveLoadBalanceAlgorithm>
    {
        static MasterSlaveLoadBalanceAlgorithmServiceLoader()
        {

            NewInstanceServiceLoader.Register<IMasterSlaveLoadBalanceAlgorithm>();
        }

        public MasterSlaveLoadBalanceAlgorithmServiceLoader()
        {
        }
    }
}
