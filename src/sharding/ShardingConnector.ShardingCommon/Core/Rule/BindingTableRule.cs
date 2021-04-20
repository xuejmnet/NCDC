using System;
using System.Collections.Generic;
using System.Linq;
using ShardingConnector.Exceptions;
using ShardingConnector.Extensions;

namespace ShardingConnector.ShardingCommon.Core.Rule
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
    public String GetBindingActualTable( String dataSource,  String logicTable,  String otherActualTable) {
        int index = -1;
        foreach (var tableRule in tableRules)
        {
            index = tableRule.FindActualTableIndex(dataSource, otherActualTable);
            if (-1 != index)
            {
                break;
            }
        }
        if (-1 == index) {
            throw new ShardingException($"Actual table [{dataSource}].[{otherActualTable}] is not in table config" );
        }
        foreach (var tableRule in tableRules)
        {
            if (tableRule.logicTable.Equals(logicTable.ToLower()))
            {
                return tableRule.actualDataNodes[index].GetTableName().ToLower();
            }
        }
        throw new ShardingException($"Cannot find binding actual table, data source: {dataSource}, logic table: {logicTable}, other actual table: {otherActualTable}");
    }
    
   public ICollection<String> GetAllLogicTables()
    {
        return tableRules.Select(o => o.logicTable.ToLower()).ToList();
    }
    
   public IDictionary<String, String> GetLogicAndActualTables( String dataSource,  String logicTable,  String actualTable,  ICollection<String> availableLogicBindingTables) {
       IDictionary<String, String> result = new Dictionary<string, string>();
        foreach (var availableLogicBindingTable in availableLogicBindingTables)
        {
            String availableLogicTable = availableLogicBindingTable.ToLower();
            if (!availableLogicTable.EqualsIgnoreCase(logicTable) && HasLogicTable(availableLogicTable)) {
                result.Add(availableLogicTable, GetBindingActualTable(dataSource, availableLogicTable, actualTable));
            }
        }
        return result;
    }
    }
}