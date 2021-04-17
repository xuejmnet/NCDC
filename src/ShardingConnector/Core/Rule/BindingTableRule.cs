using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ShardingConnector.Core.Rule
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/04/17 15:16:59
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */

    /// <summary>
    /// 
    /// </summary>
    public sealed class BindingTableRule
    {
        
    private readonly List<TableRule> tableRules;

    public BindingTableRule(List<TableRule> tableRules)
    {
        this.tableRules = tableRules;
    }

    /**
     * Judge contains this logic table in this rule.
     * 
     * @param logicTable logic table name
     * @return contains this logic table or not
     */
    public bool HasLogicTable(string logicTable) {
        return tableRules.Any(tableRule => tableRule.logicTable.Equals(logicTable.ToLower()));
    }
    
    /**
     * Deduce actual table name from other actual table name in same binding table rule.
     * 
     * @param dataSource data source name
     * @param logicTable logic table name
     * @param otherActualTable other actual table name in same binding table rule
     * @return actual table name
     */
    public String GetBindingActualTable(final String dataSource, final String logicTable, final String otherActualTable) {
        int index = -1;
        for (TableRule each : tableRules) {
            index = each.findActualTableIndex(dataSource, otherActualTable);
            if (-1 != index) {
                break;
            }
        }
        if (-1 == index) {
            throw new ShardingSphereConfigurationException("Actual table [%s].[%s] is not in table config", dataSource, otherActualTable);
        }
        for (TableRule each : tableRules) {
            if (each.getLogicTable().equals(logicTable.toLowerCase())) {
                return each.getActualDataNodes().get(index).getTableName().toLowerCase();
            }
        }
        throw new ShardingSphereConfigurationException("Cannot find binding actual table, data source: %s, logic table: %s, other actual table: %s", dataSource, logicTable, otherActualTable);
    }
    
    Collection<String> getAllLogicTables() {
        return tableRules.stream().map(input -> input.getLogicTable().toLowerCase()).collect(Collectors.toList());
    }
    
    Map<String, String> getLogicAndActualTables(final String dataSource, final String logicTable, final String actualTable, final Collection<String> availableLogicBindingTables) {
        Map<String, String> result = new LinkedHashMap<>();
        for (System.String each : availableLogicBindingTables) {
            String availableLogicTable = each.toLowerCase();
            if (!availableLogicTable.equalsIgnoreCase(logicTable) && hasLogicTable(availableLogicTable)) {
                result.put(availableLogicTable, getBindingActualTable(dataSource, availableLogicTable, actualTable));
            }
        }
        return result;
    }
    }
}