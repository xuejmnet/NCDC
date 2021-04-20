using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.Api.Config.MasterSlave;
using ShardingConnector.Common.Config;
using ShardingConnector.Common.Rule;

namespace ShardingConnector.Core.Rule
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/19 13:40:39
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    //public class MasterSlaveRule: IBaseRule
    //{
    //    private readonly String name;
    
    //private readonly String masterDataSourceName;
    
    //private readonly List<String> slaveDataSourceNames;

    //    private readonly MasterSlaveLoadBalanceAlgorithm loadBalanceAlgorithm;
    
    //private readonly MasterSlaveRuleConfiguration ruleConfiguration;
    
    //public MasterSlaveRule(final String name, final String masterDataSourceName, final List<String> slaveDataSourceNames, final MasterSlaveLoadBalanceAlgorithm loadBalanceAlgorithm)
    //    {
    //        this.name = name;
    //        this.masterDataSourceName = masterDataSourceName;
    //        this.slaveDataSourceNames = slaveDataSourceNames;
    //        this.loadBalanceAlgorithm = null == loadBalanceAlgorithm ? new MasterSlaveLoadBalanceAlgorithmServiceLoader().newService() : loadBalanceAlgorithm;
    //        ruleConfiguration = new MasterSlaveRuleConfiguration(name, masterDataSourceName, slaveDataSourceNames,
    //                new LoadBalanceStrategyConfiguration(this.loadBalanceAlgorithm.getType(), this.loadBalanceAlgorithm.getProperties()));
    //    }

    //    public MasterSlaveRule(final MasterSlaveRuleConfiguration config)
    //    {
    //        name = config.getName();
    //        masterDataSourceName = config.getMasterDataSourceName();
    //        slaveDataSourceNames = config.getSlaveDataSourceNames();
    //        loadBalanceAlgorithm = createMasterSlaveLoadBalanceAlgorithm(config.getLoadBalanceStrategyConfiguration());
    //        ruleConfiguration = config;
    //    }

    //    private MasterSlaveLoadBalanceAlgorithm createMasterSlaveLoadBalanceAlgorithm(final LoadBalanceStrategyConfiguration loadBalanceStrategyConfiguration)
    //    {
    //        MasterSlaveLoadBalanceAlgorithmServiceLoader serviceLoader = new MasterSlaveLoadBalanceAlgorithmServiceLoader();
    //        return null == loadBalanceStrategyConfiguration
    //                ? serviceLoader.newService() : serviceLoader.newService(loadBalanceStrategyConfiguration.getType(), loadBalanceStrategyConfiguration.getProperties());
    //    }

    //    /**
    //     * Judge whether contain data source name.
    //     *
    //     * @param dataSourceName data source name
    //     * @return contain or not.
    //     */
    //    public boolean containDataSourceName(final String dataSourceName)
    //    {
    //        return masterDataSourceName.equals(dataSourceName) || slaveDataSourceNames.contains(dataSourceName);
    //    }
    //    public IRuleConfiguration GetRuleConfiguration()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
