using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ShardingConnector.Api.Config.MasterSlave;
using ShardingConnector.Api.Config.Sharding;
using ShardingConnector.Extensions;

namespace ShardingConnector.Core.Rule
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/16 10:29:36
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public sealed class ShardingDataSourceNames
    {
        private readonly ShardingRuleConfiguration shardingRuleConfig;

        public  ICollection<string> DataSourceNames { get; }

        public ShardingDataSourceNames(ShardingRuleConfiguration shardingRuleConfig,
            ICollection<string> rawDataSourceNames)
        {
            this.shardingRuleConfig = shardingRuleConfig ??
                                      throw new ArgumentNullException(
                                          "can not construct ShardingDataSourceNames with null ShardingRuleConfig");
            DataSourceNames = GetAllDataSourceNames(rawDataSourceNames);
        }

        private ICollection<string> GetAllDataSourceNames(ICollection<string> dataSourceNames)
        {
            ICollection<string> result = new LinkedList<string>(dataSourceNames);
            foreach (var masterSlaveRuleConfig in shardingRuleConfig.MasterSlaveRuleConfigs)
            {
                result.Remove(masterSlaveRuleConfig.MasterDataSourceName);
                result.RemoveAll(masterSlaveRuleConfig.SlaveDataSourceNames);
                result.Add(masterSlaveRuleConfig.Name);
            }

            return result;
        }

        /**
     * Get default data source name.
     *
     * @return default data source name
     */
        public string GetDefaultDataSourceName()
        {
            return 1 == DataSourceNames.Count ? DataSourceNames.First() : shardingRuleConfig.DefaultDataSourceName;
        }

        /**
     * Get raw master data source name.
     *
     * @param dataSourceName data source name
     * @return raw master data source name
     */
        public string GetRawMasterDataSourceName(string dataSourceName)
        {
            foreach (var masterSlaveRuleConfig in shardingRuleConfig.MasterSlaveRuleConfigs)
            {
                if (masterSlaveRuleConfig.Name == dataSourceName)
                {
                    return masterSlaveRuleConfig.MasterDataSourceName;
                }
            }

            return dataSourceName;
        }

        /**
     * Get random data source name.
     *
     * @return random data source name
     */
        public string GetRandomDataSourceName()
        {
            return GetRandomDataSourceName(DataSourceNames);
        }

        /**
     * Get random data source name.
     *
     * @param dataSourceNames available data source names
     * @return random data source name
     */
        public string GetRandomDataSourceName(ICollection<string> dataSourceNames)
        {
            return dataSourceNames.ToList()[new Random().Next(0, dataSourceNames.Count)];
        }
    }
}