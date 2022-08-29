using System;
using System.Collections.Generic;
using System.Text;
using OpenConnector.Spi;

namespace OpenConnector.ShardingApi.Spi.MasterSlave
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 14:17:19
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public interface IMasterSlaveLoadBalanceAlgorithm:ITypeBasedSpi
    {
        String GetDataSource(String name, String masterDataSourceName, List<String> slaveDataSourceNames);
    }
}
