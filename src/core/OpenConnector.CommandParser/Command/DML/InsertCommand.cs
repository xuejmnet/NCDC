using System.Collections.Generic;
using System.Linq;
using OpenConnector.CommandParser.Segment.DML.Assignment;
using OpenConnector.CommandParser.Segment.DML.Column;
using OpenConnector.CommandParser.Segment.DML.Expr;
using OpenConnector.CommandParser.Segment.Generic.Table;

namespace OpenConnector.CommandParser.Command.DML
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: Monday, 12 April 2021 22:38:40
    * @Email: 326308290@qq.com
    */
    public sealed class InsertCommand : DMLCommand
    {

        public SimpleTableSegment Table { get; set; }

        public InsertColumnsSegment InsertColumns { get; set; }

        public SetAssignmentSegment SetAssignment { get; set; }

        public OnDuplicateKeyColumnsSegment OnDuplicateKeyColumns { get; set; }

        public readonly ICollection<InsertValuesSegment> Values = new LinkedList<InsertValuesSegment>();

        public List<ColumnSegment> GetColumns()
        {
            return null == InsertColumns ? new List<ColumnSegment>(0) : InsertColumns.GetColumns();
        }


/// <summary>
/// insert into 没有指定列的情况下返回true
/// </summary>
/// <returns></returns>
        public bool UseDefaultColumns()
        {
            return !GetColumns().Any() && null == SetAssignment;
        }

        /**
         * Get column names.
         *
         * @return column names
         */
        public List<string> GetColumnNames()
        {
            return null == SetAssignment ? GetColumnNamesForInsertColumns() : GetColumnNamesForSetAssignment();
        }

        private List<string> GetColumnNamesForInsertColumns()
        {
            List<string> result = new List<string>(GetColumns().Count);
            foreach (var column in GetColumns())
            {
                result.Add(column.GetIdentifier().GetValue().ToLower());
            }
            return result;
        }

        private List<string> GetColumnNamesForSetAssignment()
        {
            List<string> result = new List<string>(SetAssignment.GetAssignments().Count);
            foreach (var assignment in SetAssignment.GetAssignments())
            {
                result.Add(assignment.GetColumn().GetIdentifier().GetValue().ToLower());
            }
            return result;
        }

        /**
         * Get value list count.
         *
         * @return value list count
         */
        public int GetValueListCount()
        {
            return null == SetAssignment ? Values.Count : 1;
        }

        /**
         * Get value count for per value list.
         * 
         * @return value count
         */
        public int GetValueCountForPerGroup()
        {
            if (Values.Any())
            {
                return Values.First().GetValues().Count;
            }
            if (null != SetAssignment)
            {
                return SetAssignment.GetAssignments().Count;
            }
            return 0;
        }

        /**
         * Get all value expressions.
         * 
         * @return all value expressions
         */
        public List<List<IExpressionSegment>> GetAllValueExpressions()
        {
            return null == SetAssignment ? GetAllValueExpressionsFromValues() : new List<List<IExpressionSegment>>(){ GetAllValueExpressionsFromSetAssignment() };
        }

        private List<List<IExpressionSegment>> GetAllValueExpressionsFromValues()
        {
            List<List<IExpressionSegment>> result = new List<List<IExpressionSegment>>(Values.Count);
            foreach (var insertValues in Values)
            {
                result.Add(insertValues.GetValues());
            }
            return result;
        }

        private List<IExpressionSegment> GetAllValueExpressionsFromSetAssignment()
        {
            List<IExpressionSegment> result = new List<IExpressionSegment>(SetAssignment.GetAssignments().Count);
            foreach (var assignment in SetAssignment.GetAssignments())
            {
                result.Add(assignment.GetValue());

            }
            return result;
        }
    }
}