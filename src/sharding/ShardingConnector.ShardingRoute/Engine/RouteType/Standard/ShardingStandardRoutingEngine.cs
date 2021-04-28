using System;
using System.Collections.Generic;
using ShardingConnector.CommandParser.Command;
using ShardingConnector.Common.Config.Properties;
using ShardingConnector.Common.Rule;
using ShardingConnector.ParserBinder.Command;
using ShardingConnector.Route.Context;
using ShardingConnector.ShardingCommon.Core.Rule;
using ShardingConnector.ShardingRoute.Engine.Condition;

namespace ShardingConnector.ShardingRoute.Engine.RouteType.Standard
{
/*
* @Author: xjm
* @Description:
* @Date: Wednesday, 28 April 2021 22:15:46
* @Email: 326308290@qq.com
*/
    public sealed class ShardingStandardRoutingEngine:IShardingRouteEngine
    {
        private readonly String logicTableName;
    
        private readonly ISqlCommandContext<ISqlCommand> sqlStatementContext;
    
        private readonly ShardingConditions shardingConditions;
    
        private readonly ConfigurationProperties properties;
    
        private readonly ICollection<ICollection<DataNode>> originalDataNodes = new LinkedList<ICollection<DataNode>>();
        public RouteResult Route(ShardingRule shardingRule)
        {
             if (isDMLForModify(sqlStatementContext) && 1 != ((TableAvailable) sqlStatementContext).getAllTables().size()) {
            throw new ShardingSphereException("Cannot support Multiple-Table for '%s'.", sqlStatementContext.getSqlStatement());
        }
        return generateRouteResult(getDataNodes(shardingRule, shardingRule.getTableRule(logicTableName)));
    }
    
    private boolean isDMLForModify(final SQLStatementContext sqlStatementContext) {
        return sqlStatementContext instanceof InsertStatementContext || sqlStatementContext instanceof UpdateStatementContext || sqlStatementContext instanceof DeleteStatementContext;
    }
    
    private RouteResult generateRouteResult(final Collection<DataNode> routedDataNodes) {
        RouteResult result = new RouteResult();
        result.getOriginalDataNodes().addAll(originalDataNodes);
        for (DataNode each : routedDataNodes) {
            result.getRouteUnits().add(
                    new RouteUnit(new RouteMapper(each.getDataSourceName(), each.getDataSourceName()), Collections.singletonList(new RouteMapper(logicTableName, each.getTableName()))));
        }
        return result;
    }
    
    private Collection<DataNode> getDataNodes(final ShardingRule shardingRule, final TableRule tableRule) {
        if (isRoutingByHint(shardingRule, tableRule)) {
            return routeByHint(shardingRule, tableRule);
        }
        if (isRoutingByShardingConditions(shardingRule, tableRule)) {
            return routeByShardingConditions(shardingRule, tableRule);
        }
        return routeByMixedConditions(shardingRule, tableRule);
    }
    
    private boolean isRoutingByHint(final ShardingRule shardingRule, final TableRule tableRule) {
        return shardingRule.getDatabaseShardingStrategy(tableRule) instanceof HintShardingStrategy && shardingRule.getTableShardingStrategy(tableRule) instanceof HintShardingStrategy;
    }
    
    private Collection<DataNode> routeByHint(final ShardingRule shardingRule, final TableRule tableRule) {
        return route0(shardingRule, tableRule, getDatabaseShardingValuesFromHint(), getTableShardingValuesFromHint());
    }
    
    private boolean isRoutingByShardingConditions(final ShardingRule shardingRule, final TableRule tableRule) {
        return !(shardingRule.getDatabaseShardingStrategy(tableRule) instanceof HintShardingStrategy || shardingRule.getTableShardingStrategy(tableRule) instanceof HintShardingStrategy);
    }
    
    private Collection<DataNode> routeByShardingConditions(final ShardingRule shardingRule, final TableRule tableRule) {
        return shardingConditions.getConditions().isEmpty()
                ? route0(shardingRule, tableRule, Collections.emptyList(), Collections.emptyList()) : routeByShardingConditionsWithCondition(shardingRule, tableRule);
    }
    
    private Collection<DataNode> routeByShardingConditionsWithCondition(final ShardingRule shardingRule, final TableRule tableRule) {
        Collection<DataNode> result = new LinkedList<>();
        for (ShardingCondition each : shardingConditions.getConditions()) {
            Collection<DataNode> dataNodes = route0(shardingRule, tableRule, 
                    getShardingValuesFromShardingConditions(shardingRule, shardingRule.getDatabaseShardingStrategy(tableRule).getShardingColumns(), each),
                    getShardingValuesFromShardingConditions(shardingRule, shardingRule.getTableShardingStrategy(tableRule).getShardingColumns(), each));
            result.addAll(dataNodes);
            originalDataNodes.add(dataNodes);
        }
        return result;
    }
    
    private Collection<DataNode> routeByMixedConditions(final ShardingRule shardingRule, final TableRule tableRule) {
        return shardingConditions.getConditions().isEmpty() ? routeByMixedConditionsWithHint(shardingRule, tableRule) : routeByMixedConditionsWithCondition(shardingRule, tableRule);
    }
    
    private Collection<DataNode> routeByMixedConditionsWithCondition(final ShardingRule shardingRule, final TableRule tableRule) {
        Collection<DataNode> result = new LinkedList<>();
        for (ShardingCondition each : shardingConditions.getConditions()) {
            Collection<DataNode> dataNodes = route0(shardingRule, tableRule, getDatabaseShardingValues(shardingRule, tableRule, each), getTableShardingValues(shardingRule, tableRule, each));
            result.addAll(dataNodes);
            originalDataNodes.add(dataNodes);
        }
        return result;
    }
    
    private Collection<DataNode> routeByMixedConditionsWithHint(final ShardingRule shardingRule, final TableRule tableRule) {
        if (shardingRule.getDatabaseShardingStrategy(tableRule) instanceof HintShardingStrategy) {
            return route0(shardingRule, tableRule, getDatabaseShardingValuesFromHint(), Collections.emptyList());
        }
        return route0(shardingRule, tableRule, Collections.emptyList(), getTableShardingValuesFromHint());
    }
    
    private List<RouteValue> getDatabaseShardingValues(final ShardingRule shardingRule, final TableRule tableRule, final ShardingCondition shardingCondition) {
        ShardingStrategy dataBaseShardingStrategy = shardingRule.getDatabaseShardingStrategy(tableRule);
        return isGettingShardingValuesFromHint(dataBaseShardingStrategy)
                ? getDatabaseShardingValuesFromHint() : getShardingValuesFromShardingConditions(shardingRule, dataBaseShardingStrategy.getShardingColumns(), shardingCondition);
    }
    
    private List<RouteValue> getTableShardingValues(final ShardingRule shardingRule, final TableRule tableRule, final ShardingCondition shardingCondition) {
        ShardingStrategy tableShardingStrategy = shardingRule.getTableShardingStrategy(tableRule);
        return isGettingShardingValuesFromHint(tableShardingStrategy)
                ? getTableShardingValuesFromHint() : getShardingValuesFromShardingConditions(shardingRule, tableShardingStrategy.getShardingColumns(), shardingCondition);
    }
    
    private boolean isGettingShardingValuesFromHint(final ShardingStrategy shardingStrategy) {
        return shardingStrategy instanceof HintShardingStrategy;
    }
    
    private List<RouteValue> getDatabaseShardingValuesFromHint() {
        return getRouteValues(HintManager.isDatabaseShardingOnly() ? HintManager.getDatabaseShardingValues() : HintManager.getDatabaseShardingValues(logicTableName));
    }
    
    private List<RouteValue> getTableShardingValuesFromHint() {
        return getRouteValues(HintManager.getTableShardingValues(logicTableName));
    }
    
    private List<RouteValue> getRouteValues(final Collection<Comparable<?>> shardingValue) {
        return shardingValue.isEmpty() ? Collections.emptyList() : Collections.singletonList(new ListRouteValue<>("", logicTableName, shardingValue));
    }
    
    private List<RouteValue> getShardingValuesFromShardingConditions(final ShardingRule shardingRule, final Collection<String> shardingColumns, final ShardingCondition shardingCondition) {
        List<RouteValue> result = new ArrayList<>(shardingColumns.size());
        for (RouteValue each : shardingCondition.getRouteValues()) {
            Optional<BindingTableRule> bindingTableRule = shardingRule.findBindingTableRule(logicTableName);
            if ((logicTableName.equals(each.getTableName()) || bindingTableRule.isPresent() && bindingTableRule.get().hasLogicTable(logicTableName)) 
                    && shardingColumns.contains(each.getColumnName())) {
                result.add(each);
            }
        }
        return result;
    }
    
    private Collection<DataNode> route0(final ShardingRule shardingRule, final TableRule tableRule, final List<RouteValue> databaseShardingValues, final List<RouteValue> tableShardingValues) {
        Collection<String> routedDataSources = routeDataSources(shardingRule, tableRule, databaseShardingValues);
        Collection<DataNode> result = new LinkedList<>();
        for (String each : routedDataSources) {
            result.addAll(routeTables(shardingRule, tableRule, each, tableShardingValues));
        }
        return result;
    }
    
    private Collection<String> routeDataSources(final ShardingRule shardingRule, final TableRule tableRule, final List<RouteValue> databaseShardingValues) {
        if (databaseShardingValues.isEmpty()) {
            return tableRule.getActualDatasourceNames();
        }
        Collection<String> result = new LinkedHashSet<>(shardingRule.getDatabaseShardingStrategy(tableRule).doSharding(tableRule.getActualDatasourceNames(), databaseShardingValues, this.properties));
        Preconditions.checkState(!result.isEmpty(), "no database route info");
        Preconditions.checkState(tableRule.getActualDatasourceNames().containsAll(result), 
                "Some routed data sources do not belong to configured data sources. routed data sources: `%s`, configured data sources: `%s`", result, tableRule.getActualDatasourceNames());
        return result;
    }
    
    private Collection<DataNode> routeTables(final ShardingRule shardingRule, final TableRule tableRule, final String routedDataSource, final List<RouteValue> tableShardingValues) {
        Collection<String> availableTargetTables = tableRule.getActualTableNames(routedDataSource);
        Collection<String> routedTables = new LinkedHashSet<>(tableShardingValues.isEmpty() ? availableTargetTables
                : shardingRule.getTableShardingStrategy(tableRule).doSharding(availableTargetTables, tableShardingValues, this.properties));
        Preconditions.checkState(!routedTables.isEmpty(), "no table route info");
        Collection<DataNode> result = new LinkedList<>();
        for (String each : routedTables) {
            result.add(new DataNode(routedDataSource, each));
        }
        return result;
    }
    }
}