using System;
using System.Collections.Generic;
using System.Linq;
using OpenConnector.CommandParser.Segment.DML.Assignment;
using OpenConnector.CommandParser.Segment.DML.Column;
using OpenConnector.CommandParser.Segment.DML.Expr;
using OpenConnector.CommandParser.Segment.Generic.Table;
using OpenConnector.Extensions;

namespace OpenConnector.CommandParser.Command.DML
{
/*
* @Author: xjm
* @Description:
* @Date: Tuesday, 20 April 2021 22:02:26
* @Email: 326308290@qq.com
*/
    public sealed class ReplaceCommand:DMLCommand
    {
        
        public SimpleTableSegment Table { get; set; }
    
        public InsertColumnsSegment ReplaceColumns{ get; set; }
    
        public SetAssignmentSegment SetAssignment{ get; set; }
    
        public readonly ICollection<InsertValuesSegment> Values = new LinkedList<InsertValuesSegment>();
    
    /**
     * Get replace columns segment.
     * 
     * @return replace columns segment
     */
    public InsertColumnsSegment GetInsertColumns() {
        return ReplaceColumns;
    }
    
    /**
     * Get columns.
     * 
     * @return columns
     */
    public ICollection<ColumnSegment> GetColumns() {
        return null == ReplaceColumns ? new List<ColumnSegment>(0) : ReplaceColumns.GetColumns();
    }
    
    
    /**
     * Judge is use default columns or not.
     * 
     * @return is use default columns or not
     */
    public bool UseDefaultColumns() {
        return GetColumns().IsEmpty() && null == SetAssignment;
    }
    
    /**
     * Get column names.
     *
     * @return column names
     */
    public List<string> GetColumnNames() {
        return null == SetAssignment ? GetColumnNamesForReplaceColumns() : GetColumnNamesForSetAssignment();
    }
    
    private List<string> GetColumnNamesForReplaceColumns() {
        return GetColumns().Select(o => o.GetIdentifier().GetValue().ToLower()).ToList();
    }
    
    private List<string> GetColumnNamesForSetAssignment()
    {
        return SetAssignment.GetAssignments().Select(o => o.GetColumn().GetIdentifier().GetValue().ToLower()).ToList();
    }
    
    /**
     * Get value list count.
     *
     * @return value list count
     */
    public int GetValueListCount() {
        return null == SetAssignment ? Values.Count : 1;
    }
    
    /**
     * Get value count for per value list.
     * 
     * @return value count
     */
    public int GetValueCountForPerGroup() {
        if (!Values.IsEmpty()) {
            return Values.First().GetValues().Count;
        }
        if (null != SetAssignment) {
            return SetAssignment.GetAssignments().Count;
        }
        return 0;
    }
    
    /**
     * Get all value expressions.
     * 
     * @return all value expressions
     */
    public List<List<IExpressionSegment>> GetAllValueExpressions() {
        return null == SetAssignment ? GetAllValueExpressionsFromValues() :new List<List<IExpressionSegment>>(){GetAllValueExpressionsFromSetAssignment()};
    }
    
    private List<List<IExpressionSegment>> GetAllValueExpressionsFromValues() {
        return Values.Select(o => o.GetValues()).ToList();
    }
    
    private List<IExpressionSegment> GetAllValueExpressionsFromSetAssignment()
    {
        return SetAssignment.GetAssignments().Select(o => o.GetValue()).ToList();
    }
    }
}