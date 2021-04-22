using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ShardingConnector.Common.Rule;
using ShardingConnector.NewConnector.DataSource;
using ShardingConnector.ParserBinder.MetaData.Schema;
using ShardingConnector.ParserBinder.MetaData.Table;
using ShardingConnector.ShardingCommon.Core.Rule;
using ShardingConnector.Spi.DataBase.DataBaseType;

namespace ShardingConnector.ShardingCommon.Core.MetaData
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/19 13:33:41
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ShardingMetaDataLoader
    {
        private static readonly int CORES = Environment.ProcessorCount;

        private static readonly int FUTURE_GET_TIME_OUT_SEC = 5;

        private readonly IDictionary<string, IDataSource> dataSourceMap;

        private readonly ShardingRule shardingRule;
    
        private readonly int maxConnectionsSizePerQuery;

        private readonly bool isCheckingMetaData;

        public ShardingMetaDataLoader(IDictionary<string, IDataSource> dataSourceMap, ShardingRule shardingRule, int maxConnectionsSizePerQuery, bool isCheckingMetaData)
        {
            this.dataSourceMap = dataSourceMap;
            this.shardingRule = shardingRule;
            this.maxConnectionsSizePerQuery = maxConnectionsSizePerQuery;
            this.isCheckingMetaData = isCheckingMetaData;
        }
        /**
         * Load table meta data.
         *
         * @param logicTableName logic table name
         * @param databaseType database type
         * @return table meta data
         * @throws SQLException SQL exception
         */
        public TableMetaData Load(string logicTableName, IDatabaseType databaseType)
        {
            TableRule tableRule = shardingRule.GetTableRule(logicTableName);
        if (!isCheckingMetaData) {
            DataNode dataNode = tableRule.ActualDataNodes.First();
            return TableMetaDataLoader.load(dataSourceMap.get(shardingRule.getShardingDataSourceNames().getRawMasterDataSourceName(
                dataNode.getDataSourceName())), dataNode.getTableName(), databaseType.getName());
        }
    Map<String, List<DataNode>> dataNodeGroups = tableRule.getDataNodeGroups();
    Map<String, TableMetaData> actualTableMetaDataMap = new HashMap<>(dataNodeGroups.size(), 1);
    Map<String, Future<TableMetaData>> tableFutureMap = new HashMap<>(dataNodeGroups.size(), 1);
    ExecutorService executorService = Executors.newFixedThreadPool(Math.min(CORES * 2, dataNodeGroups.size()));
        for (Entry<String, List<DataNode>> entry : dataNodeGroups.entrySet()) {
            for (DataNode each : entry.getValue()) {
                Future<TableMetaData> futures = executorService.submit(()->load(each, databaseType));
    tableFutureMap.put(each.getTableName(), futures);
            }
        }
        tableFutureMap.forEach((key, value)-> {
    try
    {
        TableMetaData tableMetaData = value.get(FUTURE_GET_TIME_OUT_SEC, TimeUnit.SECONDS);
        actualTableMetaDataMap.put(key, tableMetaData);
    }
    catch (InterruptedException | ExecutionException | TimeoutException e) {
        throw new IllegalStateException(String.format("Error while fetching tableMetaData with key= %s and Value=%s", key, value), e);
    }
    });
    executorService.shutdownNow();
    checkUniformed(logicTableName, actualTableMetaDataMap);
    return actualTableMetaDataMap.values().iterator().next();
}

private TableMetaData load(final DataNode dataNode, final DatabaseType databaseType)
{
    try
    {
        return TableMetaDataLoader.load(dataSourceMap.get(dataNode.getDataSourceName()), dataNode.getTableName(), databaseType.getName());
    }
    catch (SQLException e)
    {
        throw new IllegalStateException(String.format("SQLException for DataNode=%s and databaseType=%s", dataNode, databaseType.getName()), e);
    }
}

/**
 * Load schema Meta data.
 *
 * @param databaseType database type
 * @return schema Meta data
 * @throws SQLException SQL exception
 */
public SchemaMetaData load(final DatabaseType databaseType) throws SQLException
{
    SchemaMetaData result = loadShardingSchemaMetaData(databaseType);
    result.merge(loadDefaultSchemaMetaData(databaseType));
        return result;
}

private SchemaMetaData loadShardingSchemaMetaData(final DatabaseType databaseType) throws SQLException
{
    log.info("Loading {} logic tables' meta data.", shardingRule.getTableRules().size());
    Map<String, TableMetaData> tableMetaDataMap = new HashMap<>(shardingRule.getTableRules().size(), 1);
for (TableRule each : shardingRule.getTableRules())
{
    tableMetaDataMap.put(each.getLogicTable(), load(each.getLogicTable(), databaseType));
}
return new SchemaMetaData(tableMetaDataMap);
    }
    
    private SchemaMetaData loadDefaultSchemaMetaData(final DatabaseType databaseType) throws SQLException
{
    Optional<String> actualDefaultDataSourceName = shardingRule.findActualDefaultDataSourceName();
        return actualDefaultDataSourceName.isPresent()
            ? SchemaMetaDataLoader.load(dataSourceMap.get(actualDefaultDataSourceName.get()), maxConnectionsSizePerQuery, databaseType.getName()) : new SchemaMetaData(Collections.emptyMap());
    }


    private void checkUniformed(final String logicTableName, final Map<String, TableMetaData> actualTableMetaDataMap)
{
    ShardingTableMetaDataDecorator decorator = new ShardingTableMetaDataDecorator();
    TableMetaData sample = decorator.decorate(actualTableMetaDataMap.values().iterator().next(), logicTableName, shardingRule);
    Collection<TableMetaDataViolation> violations = actualTableMetaDataMap.entrySet().stream()
        .filter(entry-> !sample.equals(decorator.decorate(entry.getValue(), logicTableName, shardingRule)))
        .map(entry-> new TableMetaDataViolation(entry.getKey(), entry.getValue())).collect(Collectors.toList());
    throwExceptionIfNecessary(violations, logicTableName);
}

private void throwExceptionIfNecessary(final Collection<TableMetaDataViolation> violations, final String logicTableName)
{
    if (!violations.isEmpty())
    {
        StringBuilder errorMessage = new StringBuilder(
            "Cannot get uniformed table structure for logic table `%s`, it has different meta data of actual tables are as follows:").append(LINE_SEPARATOR);
        for (TableMetaDataViolation each : violations) {
    errorMessage.append("actual table: ").append(each.getActualTableName()).append(", meta data: ").append(each.getTableMetaData()).append(LINE_SEPARATOR);
}
throw new ShardingSphereException(errorMessage.toString(), logicTableName);
        }
    }
    
    @RequiredArgsConstructor
    @Getter
    private final class TableMetaDataViolation
{

    private final String actualTableName;
        
        private final TableMetaData tableMetaData;
    }
    }
}
