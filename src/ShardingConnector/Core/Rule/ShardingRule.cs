using System.Collections.Generic;
using ShardingConnector.Api.Config.MasterSlave;
using ShardingConnector.Api.Config.Sharding;
using ShardingConnector.Common.Config;
using ShardingConnector.Common.Rule;
using ShardingConnector.Encrypt.Api;

namespace ShardingConnector.Core.Rule
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
    public class ShardingRule:IBaseRule
    {
        
    // public  ShardingRuleConfiguration ruleConfiguration { get; }
    //
    // public  ShardingDataSourceNames shardingDataSourceNames { get; }
    //
    // public  ICollection<TableRule> tableRules { get; }
    //
    // public  ICollection<BindingTableRule> bindingTableRules { get; }
    //
    // public  ICollection<string> broadcastTables { get; }
    //
    // public  ShardingStrategy defaultDatabaseShardingStrategy { get; }
    //
    // public  ShardingStrategy defaultTableShardingStrategy { get; }
    //
    // public  ShardingKeyGenerator defaultShardingKeyGenerator { get; }
    //
    // public  ICollection<MasterSlaveRule> masterSlaveRules { get; }
    //
    // public  EncryptRule encryptRule { get; }
    //
    // public ShardingRule( ShardingRuleConfiguration shardingRuleConfig,  ICollection<string> dataSourceNames) {
    //     Preconditions.checkArgument(null != shardingRuleConfig, "ShardingRuleConfig cannot be null.");
    //     Preconditions.checkArgument(null != dataSourceNames && !dataSourceNames.isEmpty(), "Data sources cannot be empty.");
    //     this.ruleConfiguration = shardingRuleConfig;
    //     shardingDataSourceNames = new ShardingDataSourceNames(shardingRuleConfig, dataSourceNames);
    //     tableRules = createTableRules(shardingRuleConfig);
    //     broadcastTables = shardingRuleConfig.getBroadcastTables();
    //     bindingTableRules = createBindingTableRules(shardingRuleConfig.getBindingTableGroups());
    //     defaultDatabaseShardingStrategy = createDefaultShardingStrategy(shardingRuleConfig.getDefaultDatabaseShardingStrategyConfig());
    //     defaultTableShardingStrategy = createDefaultShardingStrategy(shardingRuleConfig.getDefaultTableShardingStrategyConfig());
    //     defaultShardingKeyGenerator = createDefaultKeyGenerator(shardingRuleConfig.getDefaultKeyGeneratorConfig());
    //     masterSlaveRules = createMasterSlaveRules(shardingRuleConfig.getMasterSlaveRuleConfigs());
    //     encryptRule = createEncryptRule(shardingRuleConfig.getEncryptRuleConfig());
    // }
    //
    // private ICollection<TableRule> createTableRules( ShardingRuleConfiguration shardingRuleConfig) {
    //     return shardingRuleConfig.getTableRuleConfigs().stream().map(each ->
    //             new TableRule(each, shardingDataSourceNames, getDefaultGenerateKeyColumn(shardingRuleConfig))).collect(Collectors.toList());
    // }
    //
    // private string getDefaultGenerateKeyColumn( ShardingRuleConfiguration shardingRuleConfig) {
    //     return Optional.ofNullable(shardingRuleConfig.getDefaultKeyGeneratorConfig()).map(KeyGeneratorConfiguration::getColumn).orElse(null);
    // }
    //
    // private ICollection<BindingTableRule> createBindingTableRules( ICollection<string> bindingTableGroups) {
    //     return bindingTableGroups.stream().map(this::createBindingTableRule).collect(Collectors.toList());
    // }
    //
    // private BindingTableRule createBindingTableRule( string bindingTableGroup) {
    //     List<TableRule> tableRules = Splitter.on(",").trimResults().splitToList(bindingTableGroup).stream().map(this::getTableRule).collect(Collectors.toICollection(LinkedList::new));
    //     return new BindingTableRule(tableRules);
    // }
    //
    // private ShardingStrategy createDefaultShardingStrategy( ShardingStrategyConfiguration shardingStrategyConfiguration) {
    //     return Optional.ofNullable(shardingStrategyConfiguration).map(ShardingStrategyFactory::newInstance).orElse(new NoneShardingStrategy());
    // }
    //
    // private ShardingKeyGenerator createDefaultKeyGenerator( KeyGeneratorConfiguration keyGeneratorConfiguration) {
    //     ShardingKeyGeneratorServiceLoader serviceLoader = new ShardingKeyGeneratorServiceLoader();
    //     return containsKeyGeneratorConfiguration(keyGeneratorConfiguration)
    //             ? serviceLoader.newService(keyGeneratorConfiguration.getType(), keyGeneratorConfiguration.getProperties()) : serviceLoader.newService();
    // }
    //
    // private boolean containsKeyGeneratorConfiguration( KeyGeneratorConfiguration keyGeneratorConfiguration) {
    //     return null != keyGeneratorConfiguration && !strings.isNullOrEmpty(keyGeneratorConfiguration.getType());
    // }
    //
    // private ICollection<MasterSlaveRule> createMasterSlaveRules( ICollection<MasterSlaveRuleConfiguration> masterSlaveRuleConfigurations) {
    //     return masterSlaveRuleConfigurations.stream().map(MasterSlaveRule::new).collect(Collectors.toList());
    // }
    //
    // private EncryptRule createEncryptRule( EncryptRuleConfiguration encryptRuleConfig) {
    //     return Optional.ofNullable(encryptRuleConfig).map(e -> new EncryptRule(ruleConfiguration.getEncryptRuleConfig())).orElse(new EncryptRule());
    // }
    //
    // /**
    //  * Find table rule.
    //  *
    //  * @param logicTableName logic table name
    //  * @return table rule
    //  */
    // public Optional<TableRule> findTableRule( string logicTableName) {
    //     return tableRules.stream().filter(each -> each.getLogicTable().equalsIgnoreCase(logicTableName)).findFirst();
    // }
    //
    // /**
    //  * Find table rule via actual table name.
    //  *
    //  * @param actualTableName actual table name
    //  * @return table rule
    //  */
    // public Optional<TableRule> findTableRuleByActualTable( string actualTableName) {
    //     return tableRules.stream().filter(each -> each.isExisted(actualTableName)).findFirst();
    // }
    //
    // /**
    //  * Get table rule.
    //  *
    //  * @param logicTableName logic table name
    //  * @return table rule
    //  */
    // public TableRule getTableRule( string logicTableName) {
    //     Optional<TableRule> tableRule = findTableRule(logicTableName);
    //     if (tableRule.isPresent()) {
    //         return tableRule.get();
    //     }
    //     if (isBroadcastTable(logicTableName)) {
    //         return new TableRule(shardingDataSourceNames.getDataSourceNames(), logicTableName);
    //     }
    //     if (!strings.isNullOrEmpty(shardingDataSourceNames.getDefaultDataSourceName())) {
    //         return new TableRule(shardingDataSourceNames.getDefaultDataSourceName(), logicTableName);
    //     }
    //     throw new ShardingSphereConfigurationException("Cannot find table rule and default data source with logic table: '%s'", logicTableName);
    // }
    //
    // /**
    //  * Get database sharding strategy.
    //  *
    //  * <p>
    //  * Use default database sharding strategy if not found.
    //  * </p>
    //  *
    //  * @param tableRule table rule
    //  * @return database sharding strategy
    //  */
    // public ShardingStrategy getDatabaseShardingStrategy( TableRule tableRule) {
    //     return null == tableRule.getDatabaseShardingStrategy() ? defaultDatabaseShardingStrategy : tableRule.getDatabaseShardingStrategy();
    // }
    //
    // /**
    //  * Get table sharding strategy.
    //  *
    //  * <p>
    //  * Use default table sharding strategy if not found.
    //  * </p>
    //  *
    //  * @param tableRule table rule
    //  * @return table sharding strategy
    //  */
    // public ShardingStrategy getTableShardingStrategy( TableRule tableRule) {
    //     return null == tableRule.getTableShardingStrategy() ? defaultTableShardingStrategy : tableRule.getTableShardingStrategy();
    // }
    //
    // /**
    //  * Judge logic tables is all belong to binding encryptors.
    //  *
    //  * @param logicTableNames logic table names
    //  * @return logic tables is all belong to binding encryptors or not
    //  */
    // public boolean isAllBindingTables( ICollection<string> logicTableNames) {
    //     if (logicTableNames.isEmpty()) {
    //         return false;
    //     }
    //     Optional<BindingTableRule> bindingTableRule = findBindingTableRule(logicTableNames);
    //     if (!bindingTableRule.isPresent()) {
    //         return false;
    //     }
    //     ICollection<string> result = new TreeSet<>(string.CASE_INSENSITIVE_ORDER);
    //     result.addAll(bindingTableRule.get().getAllLogicTables());
    //     return !result.isEmpty() && result.containsAll(logicTableNames);
    // }
    //
    // private Optional<BindingTableRule> findBindingTableRule( ICollection<string> logicTableNames) {
    //     return logicTableNames.stream().map(this::findBindingTableRule).filter(Optional::isPresent).findFirst().orElse(Optional.empty());
    // }
    //
    // /**
    //  * Find binding table rule via logic table name.
    //  *
    //  * @param logicTableName logic table name
    //  * @return binding table rule
    //  */
    // public Optional<BindingTableRule> findBindingTableRule( string logicTableName) {
    //     return bindingTableRules.stream().filter(each -> each.hasLogicTable(logicTableName)).findFirst();
    // }
    //
    // /**
    //  * Judge logic tables is all belong to broadcast encryptors.
    //  *
    //  * @param logicTableNames logic table names
    //  * @return logic tables is all belong to broadcast encryptors or not
    //  */
    // public boolean isAllBroadcastTables( ICollection<string> logicTableNames) {
    //     if (logicTableNames.isEmpty()) {
    //         return false;
    //     }
    //     for (string each : logicTableNames) {
    //         if (!isBroadcastTable(each)) {
    //             return false;
    //         }
    //     }
    //     return true;
    // }
    //
    // /**
    //  * Judge logic table is belong to broadcast tables.
    //  *
    //  * @param logicTableName logic table name
    //  * @return logic table is belong to broadcast tables or not
    //  */
    // public boolean isBroadcastTable( string logicTableName) {
    //     return broadcastTables.stream().anyMatch(each -> each.equalsIgnoreCase(logicTableName));
    // }
    //
    // /**
    //  * Judge logic tables is all belong to default data source.
    //  *
    //  * @param logicTableNames logic table names
    //  * @return logic tables is all belong to default data source
    //  */
    // public boolean isAllInDefaultDataSource( ICollection<string> logicTableNames) {
    //     if (!hasDefaultDataSourceName()) {
    //         return false;
    //     }
    //     for (string each : logicTableNames) {
    //         if (findTableRule(each).isPresent() || isBroadcastTable(each)) {
    //             return false;
    //         }
    //     }
    //     return !logicTableNames.isEmpty();
    // }
    //
    // /**
    //  * Judge if there is at least one table rule for logic tables.
    //  *
    //  * @param logicTableNames logic table names
    //  * @return whether a table rule exists for logic tables
    //  */
    // public boolean tableRuleExists( ICollection<string> logicTableNames) {
    //     return logicTableNames.stream().anyMatch(each -> findTableRule(each).isPresent() || isBroadcastTable(each));
    // }
    //
    // /**
    //  * Judge is sharding column or not.
    //  *
    //  * @param columnName column name
    //  * @param tableName table name
    //  * @return is sharding column or not
    //  */
    // public boolean isShardingColumn( string columnName,  string tableName) {
    //     return tableRules.stream().anyMatch(each -> each.getLogicTable().equalsIgnoreCase(tableName) && isShardingColumn(each, columnName));
    // }
    //
    // private boolean isShardingColumn( TableRule tableRule,  string columnName) {
    //     return getDatabaseShardingStrategy(tableRule).getShardingColumns().contains(columnName) || getTableShardingStrategy(tableRule).getShardingColumns().contains(columnName);
    // }
    //
    // /**
    //  * Find column name of generated key.
    //  *
    //  * @param logicTableName logic table name
    //  * @return column name of generated key
    //  */
    // public Optional<string> findGenerateKeyColumnName( string logicTableName) {
    //     return tableRules.stream().filter(each -> each.getLogicTable().equalsIgnoreCase(logicTableName) && each.getGenerateKeyColumn().isPresent())
    //             .map(TableRule::getGenerateKeyColumn).findFirst().orElse(Optional.empty());
    // }
    //
    // /**
    //  * Generate key.
    //  *
    //  * @param logicTableName logic table name
    //  * @return generated key
    //  */
    // public Comparable<?> generateKey( string logicTableName) {
    //     Optional<TableRule> tableRule = findTableRule(logicTableName);
    //     if (!tableRule.isPresent()) {
    //         throw new ShardingSphereConfigurationException("Cannot find strategy for generate keys.");
    //     }
    //     return Optional.ofNullable(tableRule.get().getShardingKeyGenerator()).orElse(defaultShardingKeyGenerator).generateKey();
    // }
    //
    // /**
    //  * Get logic table names based on actual table name.
    //  *
    //  * @param actualTableName actual table name
    //  * @return logic table name
    //  */
    // public ICollection<string> getLogicTableNames( string actualTableName) {
    //     return tableRules.stream().filter(each -> each.isExisted(actualTableName)).map(TableRule::getLogicTable).collect(Collectors.toICollection(LinkedList::new));
    // }
    //
    // /**
    //  * Find data node by logic table name.
    //  *
    //  * @param logicTableName logic table name
    //  * @return data node
    //  */
    // public DataNode getDataNode( string logicTableName) {
    //     TableRule tableRule = getTableRule(logicTableName);
    //     return tableRule.getActualDataNodes().get(0);
    // }
    //
    // /**
    //  * Find data node by data source and logic table.
    //  *
    //  * @param dataSourceName data source name
    //  * @param logicTableName logic table name
    //  * @return data node
    //  */
    // public DataNode getDataNode( string dataSourceName,  string logicTableName) {
    //     TableRule tableRule = getTableRule(logicTableName);
    //     return tableRule.getActualDataNodes().stream().filter(each -> shardingDataSourceNames.getDataSourceNames().contains(each.getDataSourceName())
    //             && each.getDataSourceName().equals(dataSourceName)).findFirst()
    //             .orElseThrow(() -> new ShardingSphereConfigurationException("Cannot find actual data node for data source name: '%s' and logic table name: '%s'", dataSourceName, logicTableName));
    // }
    //
    // /**
    //  * Judge if default data source mame exists.
    //  *
    //  * @return if default data source name exists
    //  */
    // public boolean hasDefaultDataSourceName() {
    //     string defaultDataSourceName = shardingDataSourceNames.getDefaultDataSourceName();
    //     return !strings.isNullOrEmpty(defaultDataSourceName);
    // }
    //
    // /**
    //  * Find actual default data source name.
    //  *
    //  * <p>If use master-slave rule, return master data source name.</p>
    //  *
    //  * @return actual default data source name
    //  */
    // public Optional<string> findActualDefaultDataSourceName() {
    //     string defaultDataSourceName = shardingDataSourceNames.getDefaultDataSourceName();
    //     if (strings.isNullOrEmpty(defaultDataSourceName)) {
    //         return Optional.empty();
    //     }
    //     Optional<string> masterDefaultDataSourceName = findMasterDataSourceName(defaultDataSourceName);
    //     return masterDefaultDataSourceName.isPresent() ? masterDefaultDataSourceName : Optional.of(defaultDataSourceName);
    // }
    //
    // private Optional<string> findMasterDataSourceName( string masterSlaveRuleName) {
    //     return masterSlaveRules.stream().filter(each -> each.getName().equalsIgnoreCase(masterSlaveRuleName)).map(e -> Optional.of(e.getMasterDataSourceName())).findFirst().orElse(Optional.empty());
    // }
    //
    // /**
    //  * Find master slave rule.
    //  *
    //  * @param dataSourceName data source name
    //  * @return master slave rule
    //  */
    // public Optional<MasterSlaveRule> findMasterSlaveRule( string dataSourceName) {
    //     return masterSlaveRules.stream().filter(each -> each.containDataSourceName(dataSourceName)).findFirst();
    // }
    //
    // /**
    //  * Get sharding logic table names.
    //  *
    //  * @param logicTableNames logic table names
    //  * @return sharding logic table names
    //  */
    // public ICollection<string> getShardingLogicTableNames( ICollection<string> logicTableNames) {
    //     return logicTableNames.stream().filter(each -> findTableRule(each).isPresent()).collect(Collectors.toICollection(LinkedList::new));
    // }
    //
    // /**
    //  * Get logic and actual binding tables.
    //  * 
    //  * @param dataSourceName data source name
    //  * @param logicTable logic table name
    //  * @param actualTable actual table name
    //  * @param availableLogicBindingTables available logic binding table names
    //  * @return logic and actual binding tables
    //  */
    // public Map<string, string> getLogicAndActualTablesFromBindingTable( string dataSourceName, 
    //                                                                     string logicTable,  string actualTable,  ICollection<string> availableLogicBindingTables) {
    //     Map<string, string> result = new LinkedHashMap<>();
    //     findBindingTableRule(logicTable).ifPresent(
    //         bindingTableRule -> result.putAll(bindingTableRule.getLogicAndActualTables(dataSourceName, logicTable, actualTable, availableLogicBindingTables)));
    //     return result;
    // }
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
        throw new System.NotImplementedException();
    }
    }
}