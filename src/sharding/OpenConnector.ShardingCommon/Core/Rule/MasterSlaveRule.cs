using System;
using System.Collections.Generic;
using NCDC.Common.Config;
using NCDC.Common.Rule;
using OpenConnector.ShardingApi.Api.Config.MasterSlave;
using OpenConnector.ShardingApi.Spi.MasterSlave;
using OpenConnector.ShardingCommon.Spi.Algorithm.MasterSlave;

namespace OpenConnector.ShardingCommon.Core.Rule
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/19 13:40:39
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public class MasterSlaveRule : IBaseRule
    {
        public string Name { get; }

        public string MasterDataSourceName { get; }

        public List<string> SlaveDataSourceNames { get; }

        public IMasterSlaveLoadBalanceAlgorithm LoadBalanceAlgorithm { get; }

        public MasterSlaveRuleConfiguration RuleConfiguration { get; }

        public MasterSlaveRule(string name, string masterDataSourceName, List<string> slaveDataSourceNames, IMasterSlaveLoadBalanceAlgorithm loadBalanceAlgorithm)
        {
            this.Name = name;
            this.MasterDataSourceName = masterDataSourceName;
            this.SlaveDataSourceNames = slaveDataSourceNames;
            this.LoadBalanceAlgorithm = null == loadBalanceAlgorithm ? new MasterSlaveLoadBalanceAlgorithmServiceLoader().NewService() : loadBalanceAlgorithm;
            RuleConfiguration = new MasterSlaveRuleConfiguration(name, masterDataSourceName, slaveDataSourceNames,
                    new LoadBalanceStrategyConfiguration(this.LoadBalanceAlgorithm.GetAlgorithmType(), this.LoadBalanceAlgorithm.GetProperties()));
        }

        public MasterSlaveRule(MasterSlaveRuleConfiguration config)
        {
            Name = config.Name;
            MasterDataSourceName = config.MasterDataSourceName;
            SlaveDataSourceNames = config.SlaveDataSourceNames;
            LoadBalanceAlgorithm = CreateMasterSlaveLoadBalanceAlgorithm(config.LoadBalanceStrategyConfiguration);
            RuleConfiguration = config;
        }

        private IMasterSlaveLoadBalanceAlgorithm CreateMasterSlaveLoadBalanceAlgorithm(LoadBalanceStrategyConfiguration loadBalanceStrategyConfiguration)
        {
            MasterSlaveLoadBalanceAlgorithmServiceLoader serviceLoader = new MasterSlaveLoadBalanceAlgorithmServiceLoader();
            return null == loadBalanceStrategyConfiguration
                    ? serviceLoader.NewService() : serviceLoader.NewService(loadBalanceStrategyConfiguration.Type, loadBalanceStrategyConfiguration.Properties);
        }

        /**
         * Judge whether contain data source name.
         *
         * @param dataSourceName data source name
         * @return contain or not.
         */
        public bool ContainDataSourceName(string dataSourceName)
        {
            return MasterDataSourceName.Equals(dataSourceName) || SlaveDataSourceNames.Contains(dataSourceName);
        }
        public IRuleConfiguration GetRuleConfiguration()
        {
            return RuleConfiguration;
        }
    }
}
