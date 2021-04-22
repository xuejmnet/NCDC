using System;
using System.Collections.Generic;
using System.Linq;
using ShardingConnector.Api.Config.Strategy;
using ShardingConnector.Common.Config;
using ShardingConnector.Common.Rule;
using ShardingConnector.Exceptions;
using ShardingConnector.Extensions;
using ShardingConnector.ShardingApi.Api.Config.MasterSlave;
using ShardingConnector.ShardingApi.Api.Config.Sharding;
using ShardingConnector.ShardingApi.Spi.KeyGen;
using ShardingConnector.ShardingCommon.Core.Strategy.Route;
using ShardingConnector.ShardingCommon.Core.Strategy.Route.None;
using ShardingConnector.ShardingCommon.Spi.Algorithm.KeyGen;

namespace ShardingConnector.ShardingCommon.Core.Rule
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/16 10:26:08
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public class ShardingRule : IBaseRule
    {

        public ShardingRuleConfiguration ruleConfiguration { get; }

        public ShardingDataSourceNames shardingDataSourceNames { get; }

        public ICollection<TableRule> tableRules { get; }

        public ICollection<BindingTableRule> bindingTableRules { get; }

        public ICollection<string> broadcastTables { get; }

        public IShardingStrategy defaultDatabaseShardingStrategy { get; }

        public IShardingStrategy defaultTableShardingStrategy { get; }

        public IShardingKeyGenerator defaultShardingKeyGenerator { get; }

        public ICollection<MasterSlaveRule> masterSlaveRules { get; }


        public ShardingRule(ShardingRuleConfiguration shardingRuleConfig, ICollection<string> dataSourceNames)
        {
            if (shardingRuleConfig == null)
                throw new ArgumentNullException(nameof(shardingRuleConfig));
            if (dataSourceNames == null || dataSourceNames.IsEmpty())
                throw new ArgumentNullException("data sources cannot be empty.");
            this.ruleConfiguration = shardingRuleConfig;
            shardingDataSourceNames = new ShardingDataSourceNames(shardingRuleConfig, dataSourceNames);
            tableRules = CreateTableRules(shardingRuleConfig);
            broadcastTables = shardingRuleConfig.BroadcastTables;
            bindingTableRules = CreateBindingTableRules(shardingRuleConfig.BindingTableGroups);
            defaultDatabaseShardingStrategy = CreateDefaultShardingStrategy(shardingRuleConfig.DefaultDatabaseShardingStrategyConfig);
            defaultTableShardingStrategy = CreateDefaultShardingStrategy(shardingRuleConfig.DefaultTableShardingStrategyConfig);
            defaultShardingKeyGenerator = CreateDefaultKeyGenerator(shardingRuleConfig.DefaultKeyGeneratorConfig);
            masterSlaveRules = CreateMasterSlaveRules(shardingRuleConfig.MasterSlaveRuleConfigs);
        }

        private ICollection<TableRule> CreateTableRules(ShardingRuleConfiguration shardingRuleConfig)
        {
            return shardingRuleConfig.TableRuleConfigs.Select(o =>
                new TableRule(o, shardingDataSourceNames, GetDefaultGenerateKeyColumn(shardingRuleConfig))).ToList();
        }

        private string GetDefaultGenerateKeyColumn(ShardingRuleConfiguration shardingRuleConfig)
        {
            return shardingRuleConfig.DefaultKeyGeneratorConfig?.Column;
        }

        private ICollection<BindingTableRule> CreateBindingTableRules(ICollection<string> bindingTableGroups)
        {
            return bindingTableGroups.Select(o => CreateBindingTableRule(o)).ToList();
        }

        private BindingTableRule CreateBindingTableRule(string bindingTableGroup)
        {
            var rules = bindingTableGroup.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(GetTableRule)
                .ToList();
            return new BindingTableRule(rules);
        }

        /**
         * Get table rule.
         *
         * @param logicTableName logic table name
         * @return table rule
         */
        public TableRule GetTableRule(string logicTableName)
        {
            var tableRule = FindTableRule(logicTableName);
            if (tableRule != null)
            {
                return tableRule;
            }
            if (IsBroadcastTable(logicTableName))
            {
                return new TableRule(shardingDataSourceNames.DataSourceNames, logicTableName);
            }
            if (!string.IsNullOrEmpty(shardingDataSourceNames.GetDefaultDataSourceName()))
            {
                return new TableRule(shardingDataSourceNames.GetDefaultDataSourceName(), logicTableName);
            }
            throw new ShardingException($"Cannot find table rule and default data source with logic table: '{logicTableName}'");
        }

        /**
         * Find table rule.
         *
         * @param logicTableName logic table name
         * @return table rule
         */
        public TableRule FindTableRule(string logicTableName)
        {
            return tableRules.FirstOrDefault(o => o.LogicTable.EqualsIgnoreCase(logicTableName));
        }

        /**
         * Judge logic table is belong to broadcast tables.
         *
         * @param logicTableName logic table name
         * @return logic table is belong to broadcast tables or not
         */
        public bool IsBroadcastTable(string logicTableName)
        {
            return broadcastTables.Any(o => o.EqualsIgnoreCase(logicTableName));
        }

        private IShardingStrategy CreateDefaultShardingStrategy(IShardingStrategyConfiguration shardingStrategyConfiguration)
        {
            if (shardingStrategyConfiguration != null)
            {
                return ShardingStrategyFactory.NewInstance(shardingStrategyConfiguration) ?? new NoneShardingStrategy();
            }
            return new NoneShardingStrategy();
        }

        private IShardingKeyGenerator CreateDefaultKeyGenerator(KeyGeneratorConfiguration keyGeneratorConfiguration)
        {
            ShardingKeyGeneratorServiceLoader serviceLoader = new ShardingKeyGeneratorServiceLoader();
            return ContainsKeyGeneratorConfiguration(keyGeneratorConfiguration)
                    ? serviceLoader.NewService(keyGeneratorConfiguration.Type, keyGeneratorConfiguration.Properties) : serviceLoader.NewService();
        }

        private bool ContainsKeyGeneratorConfiguration(KeyGeneratorConfiguration keyGeneratorConfiguration)
        {
            return null != keyGeneratorConfiguration && !string.IsNullOrEmpty(keyGeneratorConfiguration.Type);
        }

        private ICollection<MasterSlaveRule> CreateMasterSlaveRules(ICollection<MasterSlaveRuleConfiguration> masterSlaveRuleConfigurations)
        {
            return masterSlaveRuleConfigurations.Select(o => new MasterSlaveRule(o)).ToList();
        }


        /**
         * Find table rule via actual table name.
         *
         * @param actualTableName actual table name
         * @return table rule
         */
        public TableRule FindTableRuleByActualTable(string actualTableName)
        {
            return tableRules.Where(o => o.IsExisted(actualTableName)).FirstOrDefault();
        }

        /**
         * Get database sharding strategy.
         *
         * <p>
         * Use default database sharding strategy if not found.
         * </p>
         *
         * @param tableRule table rule
         * @return database sharding strategy
         */
        public IShardingStrategy GetDatabaseShardingStrategy(TableRule tableRule)
        {
            return tableRule.DatabaseShardingStrategy ?? defaultDatabaseShardingStrategy;
        }

        /**
         * Get table sharding strategy.
         *
         * <p>
         * Use default table sharding strategy if not found.
         * </p>
         *
         * @param tableRule table rule
         * @return table sharding strategy
         */
        public IShardingStrategy GetTableShardingStrategy(TableRule tableRule)
        {
            return tableRule.TableShardingStrategy ?? defaultTableShardingStrategy;
        }

        /**
         * Judge logic tables is all belong to binding encryptors.
         *
         * @param logicTableNames logic table names
         * @return logic tables is all belong to binding encryptors or not
         */
        public bool IsAllBindingTables(ICollection<string> logicTableNames)
        {
            if (logicTableNames.IsEmpty())
            {
                return false;
            }
            var bindingTableRule = FindBindingTableRule(logicTableNames);
            if (bindingTableRule==null)
            {
                return false;
            }
            ICollection<string> result = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);
            result.AddAll(bindingTableRule.GetAllLogicTables());
            return !result.IsEmpty() && logicTableNames.All(o=>result.Contains(o));
        }

        private BindingTableRule FindBindingTableRule(ICollection<string> logicTableNames)
        {
            return logicTableNames.Select(FindBindingTableRule).Where(o => null != o).FirstOrDefault();
        }

        /**
         * Find binding table rule via logic table name.
         *
         * @param logicTableName logic table name
         * @return binding table rule
         */
        public BindingTableRule FindBindingTableRule(string logicTableName)
        {
            return bindingTableRules.Where(o => o.HasLogicTable(logicTableName)).FirstOrDefault();
        }

        /**
         * Judge logic tables is all belong to broadcast encryptors.
         *
         * @param logicTableNames logic table names
         * @return logic tables is all belong to broadcast encryptors or not
         */
        public bool IsAllBroadcastTables(ICollection<string> logicTableNames)
        {
            if (logicTableNames.IsEmpty())
            {
                return false;
            }
            foreach (var logicTableName in logicTableNames)
            {
                if (!IsBroadcastTable(logicTableName))
                {
                    return false;
                }
            }
            return true;
        }

        /**
         * Judge logic tables is all belong to default data source.
         *
         * @param logicTableNames logic table names
         * @return logic tables is all belong to default data source
         */
        public bool IsAllInDefaultDataSource(ICollection<string> logicTableNames)
        {
            if (!HasDefaultDataSourceName())
            {
                return false;
            }
            foreach (var logicTableName in logicTableNames)
            {
                if (FindTableRule(logicTableName)!=null || IsBroadcastTable(logicTableName))
                {
                    return false;
                }
            }
            return !logicTableNames.IsEmpty();
        }

        /**
         * Judge if there is at least one table rule for logic tables.
         *
         * @param logicTableNames logic table names
         * @return whether a table rule exists for logic tables
         */
        public bool TableRuleExists(ICollection<string> logicTableNames)
        {
            return logicTableNames.Any(o => FindTableRule(o) != null || IsBroadcastTable(o));
        }

        /**
         * Judge is sharding column or not.
         *
         * @param columnName column name
         * @param tableName table name
         * @return is sharding column or not
         */
        public bool IsShardingColumn(string columnName, string tableName)
        {
            return tableRules.Any(o => o.LogicTable.EqualsIgnoreCase(tableName) && IsShardingColumn(o, columnName));
        }

        private bool IsShardingColumn(TableRule tableRule, string columnName)
        {
            return GetDatabaseShardingStrategy(tableRule).GetShardingColumns().Contains(columnName) || GetTableShardingStrategy(tableRule).GetShardingColumns().Contains(columnName);
        }

        /**
         * Find column name of generated key.
         *
         * @param logicTableName logic table name
         * @return column name of generated key
         */
        public string FindGenerateKeyColumnName(string logicTableName)
        {
            return tableRules
                .Where(o => o.LogicTable.EqualsIgnoreCase(logicTableName) && o.GetGenerateKeyColumn() != null)
                .Select(o => o.GetGenerateKeyColumn()).FirstOrDefault();
        }

        /**
         * Generate key.
         *
         * @param logicTableName logic table name
         * @return generated key
         */
        public IComparable GenerateKey(string logicTableName)
        {
            var tableRule = FindTableRule(logicTableName);
            if (tableRule==null)
            {
                throw new ShardingException("Cannot find strategy for generate keys.");
            }

            return tableRule.ShardingKeyGenerator?.GenerateKey() ?? defaultShardingKeyGenerator.GenerateKey();
        }

        /**
         * Get logic table names based on actual table name.
         *
         * @param actualTableName actual table name
         * @return logic table name
         */
        public ICollection<string> GetLogicTableNames(string actualTableName)
        {
            return tableRules.Where(o => o.IsExisted(actualTableName)).Select(o => o.LogicTable).ToList();
        }

        /**
         * Find data node by logic table name.
         *
         * @param logicTableName logic table name
         * @return data node
         */
        public DataNode GetDataNode(string logicTableName)
        {
            TableRule tableRule = GetTableRule(logicTableName);
            return tableRule.ActualDataNodes[0];
        }

        /**
         * Find data node by data source and logic table.
         *
         * @param dataSourceName data source name
         * @param logicTableName logic table name
         * @return data node
         */
        public DataNode GetDataNode(string dataSourceName, string logicTableName)
        {
            TableRule tableRule = GetTableRule(logicTableName);
            return tableRule.ActualDataNodes.FirstOrDefault(o => shardingDataSourceNames.DataSourceNames.Contains(o.GetDataSourceName()) &&
                                                                 o.GetDataSourceName().Equals(dataSourceName)) ?? throw new ShardingException(
                $"Cannot find actual data node for data source name: '{dataSourceName}' and logic table name: '{logicTableName}'");
        }

        /**
         * Judge if default data source mame exists.
         *
         * @return if default data source name exists
         */
        public bool HasDefaultDataSourceName()
        {
            string defaultDataSourceName = shardingDataSourceNames.GetDefaultDataSourceName();
            return !string.IsNullOrEmpty(defaultDataSourceName);
        }

        /**
         * Find actual default data source name.
         *
         * <p>If use master-slave rule, return master data source name.</p>
         *
         * @return actual default data source name
         */
        public string FindActualDefaultDataSourceName()
        {
            string defaultDataSourceName = shardingDataSourceNames.GetDefaultDataSourceName();
            if (string.IsNullOrEmpty(defaultDataSourceName))
            {
                return null;
            }
            var masterDefaultDataSourceName = FindMasterDataSourceName(defaultDataSourceName);
            return masterDefaultDataSourceName?? defaultDataSourceName;
        }

        private string FindMasterDataSourceName(string masterSlaveRuleName)
        {
            return masterSlaveRules.Where(o=>o.Name.EqualsIgnoreCase(masterSlaveRuleName)).Select(o=>o.MasterDataSourceName).FirstOrDefault();
        }

        /**
         * Find master slave rule.
         *
         * @param dataSourceName data source name
         * @return master slave rule
         */
        public MasterSlaveRule FindMasterSlaveRule(string dataSourceName)
        {
            return masterSlaveRules.Where(o=>o.ContainDataSourceName(dataSourceName)).FirstOrDefault();
        }

        /**
         * Get sharding logic table names.
         *
         * @param logicTableNames logic table names
         * @return sharding logic table names
         */
        public ICollection<string> GetShardingLogicTableNames(ICollection<string> logicTableNames)
        {
            return logicTableNames.Where(o => FindTableRule(o) != null).ToList();
        }

        /**
         * Get logic and actual binding tables.
         * 
         * @param dataSourceName data source name
         * @param logicTable logic table name
         * @param actualTable actual table name
         * @param availableLogicBindingTables available logic binding table names
         * @return logic and actual binding tables
         */
        public IDictionary<string, string> GetLogicAndActualTablesFromBindingTable(string dataSourceName,
                                                                            string logicTable, string actualTable, ICollection<string> availableLogicBindingTables)
        {
            IDictionary<string, string> result = new Dictionary<string, string>();
            var bindingTableRule = FindBindingTableRule(logicTable);
            if (bindingTableRule != null)
            {
                result.AddAll(bindingTableRule.GetLogicAndActualTables(dataSourceName,logicTable,actualTable, availableLogicBindingTables));
            }

            return result;
        }
        //
        // /**
        //  * To rules.
        //  * 
        //  * @return rules
        //  */
        // public ICollection<BaseRule> toRules() {
        //     ICollection<BaseRule> result = new LinkedList<>();
        //     result.add(this);
        //     if (!encryptRule.getEncryptTableNames().isEmpty()) {
        //         result.add(encryptRule);
        //     }
        //     result.addAll(masterSlaveRules);
        //     return result;
        // }
        public IRuleConfiguration GetRuleConfiguration()
        {
            return ruleConfiguration;
        }

        public ICollection<IBaseRule> ToRules()
        {
            ICollection<IBaseRule> result = new LinkedList<IBaseRule>();
            result.Add(this);
            // if (!encryptRule.getEncryptTableNames().isEmpty()) {
            //     result.add(encryptRule);
            // }
            foreach (var masterSlaveRule in masterSlaveRules)
            {
                result.Add(masterSlaveRule);
            }
            return result;
        }
    }
}