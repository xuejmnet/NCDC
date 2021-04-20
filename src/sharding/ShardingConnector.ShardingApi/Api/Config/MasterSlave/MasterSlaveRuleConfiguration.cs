using System;
using System.Collections.Generic;
using System.Linq;
using ShardingConnector.Common.Config;

namespace ShardingConnector.ShardingApi.Api.Config.MasterSlave
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/14 10:16:34
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class MasterSlaveRuleConfiguration:IRuleConfiguration
    {
        public string Name { get; }
    
        public string MasterDataSourceName { get; }

        public List<string> SlaveDataSourceNames { get; }

        public LoadBalanceStrategyConfiguration LoadBalanceStrategyConfiguration { get; }

        public MasterSlaveRuleConfiguration(string name, string masterDataSourceName, List<string> slaveDataSourceNames): this(name, masterDataSourceName, slaveDataSourceNames, null)
        {
        }

        public MasterSlaveRuleConfiguration(string name,string masterDataSourceName, List<string> slaveDataSourceNames, LoadBalanceStrategyConfiguration loadBalanceStrategyConfiguration)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");
            if (string.IsNullOrWhiteSpace(masterDataSourceName))
                throw new ArgumentNullException("masterDataSourceName");
            if (null != slaveDataSourceNames && slaveDataSourceNames.Any())
                throw new ArgumentNullException("slaveDataSourceNames");
            this.Name = name;
            this.MasterDataSourceName = masterDataSourceName;
            this.SlaveDataSourceNames = slaveDataSourceNames;
            this.LoadBalanceStrategyConfiguration = loadBalanceStrategyConfiguration;
        }
    }
}
