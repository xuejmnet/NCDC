using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ShardingConnector.Parser.Sql.Segment.DML.Column;
using ShardingConnector.Parser.Sql.Segment.Generic.Table;

namespace ShardingConnector.Parser.Sql.Command.DML
{
/*
* @Author: xjm
* @Description:
* @Date: Monday, 12 April 2021 22:38:40
* @Email: 326308290@qq.com
*/
    public sealed class InsertCommand:DMLCommand
    {
        
    public SimpleTableSegment Table;
    
    public InsertColumnsSegment InsertColumns;
    
    public SetAssignmentSegment SetAssignment;
    
    public OnDuplicateKeyColumnsSegment OnDuplicateKeyColumns;
    
    public readonly ICollection<InsertValuesSegment> Values = new LinkedList<InsertValuesSegment>();
    
    /**
     * Get insert columns segment.
     * 
     * @return insert columns segment
     */
    public Optional<InsertColumnsSegment> getInsertColumns() {
        return Optional.ofNullable(insertColumns);
    }
    
    /**
     * Get columns.
     * 
     * @return columns
     */
    public Collection<ColumnSegment> getColumns() {
        return null == insertColumns ? Collections.emptyList() : insertColumns.getColumns();
    }
    
    /**
     * Get set assignment segment.
     * 
     * @return set assignment segment
     */
    public Optional<SetAssignmentSegment> getSetAssignment() {
        return Optional.ofNullable(setAssignment);
    }
    
    /**
     * Get on duplicate key columns segment.
     *
     * @return on duplicate key columns segment
     */
    public Optional<OnDuplicateKeyColumnsSegment> getOnDuplicateKeyColumns() {
        return Optional.ofNullable(onDuplicateKeyColumns);
    }
    
    /**
     * Judge is use default columns or not.
     * 
     * @return is use default columns or not
     */
    public boolean useDefaultColumns() {
        return getColumns().isEmpty() && null == setAssignment;
    }
    
    /**
     * Get column names.
     *
     * @return column names
     */
    public List<String> getColumnNames() {
        return null == setAssignment ? getColumnNamesForInsertColumns() : getColumnNamesForSetAssignment();
    }
    
    private List<String> getColumnNamesForInsertColumns() {
        List<String> result = new LinkedList<>();
        for (ColumnSegment each : getColumns()) {
            result.add(each.getIdentifier().getValue().toLowerCase());
        }
        return result;
    }
    
    private List<String> getColumnNamesForSetAssignment() {
        List<String> result = new LinkedList<>();
        for (AssignmentSegment each : setAssignment.getAssignments()) {
            result.add(each.getColumn().getIdentifier().getValue().toLowerCase());
        }
        return result;
    }
    
    /**
     * Get value list count.
     *
     * @return value list count
     */
    public int getValueListCount() {
        return null == setAssignment ? values.size() : 1;
    }
    
    /**
     * Get value count for per value list.
     * 
     * @return value count
     */
    public int getValueCountForPerGroup() {
        if (!values.isEmpty()) {
            return values.iterator().next().getValues().size();
        }
        if (null != setAssignment) {
            return setAssignment.getAssignments().size();
        }
        return 0;
    }
    
    /**
     * Get all value expressions.
     * 
     * @return all value expressions
     */
    public List<List<ExpressionSegment>> getAllValueExpressions() {
        return null == setAssignment ? getAllValueExpressionsFromValues() : Collections.singletonList(getAllValueExpressionsFromSetAssignment());
    }
    
    private List<List<ExpressionSegment>> getAllValueExpressionsFromValues() {
        List<List<ExpressionSegment>> result = new ArrayList<>(values.size());
        for (InsertValuesSegment each : values) {
            result.add(each.getValues());
        }
        return result;
    }
    
    private List<ExpressionSegment> getAllValueExpressionsFromSetAssignment() {
        List<ExpressionSegment> result = new ArrayList<>(setAssignment.getAssignments().size());
        for (AssignmentSegment each : setAssignment.getAssignments()) {
            result.add(each.getValue());
        }
        return result;
    }
    }
}