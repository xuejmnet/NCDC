using System;
using System.Collections.Generic;
using System.Text;
using NCDC.CommandParser.Segment.DDL.Index;
using NCDC.CommandParser.Segment.DML.Column;
using NCDC.CommandParser.Segment.Generic.Table;

namespace NCDC.CommandParser.Segment.DDL.Constraint
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 17:07:50
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ConstraintDefinitionSegment:ICreateDefinitionSegment,IAlterDefinitionSegment
    {
        public int StartIndex { get; }
        public int StopIndex { get; }

        public readonly ICollection<ColumnSegment> PrimaryKeyColumns = new LinkedList<ColumnSegment>();
        public readonly ICollection<ColumnSegment> IndexColumns = new List<ColumnSegment>();
        public ConstraintSegment? ConstraintName { get; set; }
        public IndexSegment? IndexName { get; set; }
        public SimpleTableSegment? ReferencedTable { get; set; }

        public ConstraintDefinitionSegment(int startIndex, int stopIndex)
        {
            StartIndex = startIndex;
            StopIndex = stopIndex;
        }

        public override string ToString()
        {
            return $"{nameof(PrimaryKeyColumns)}: {PrimaryKeyColumns}, {nameof(IndexColumns)}: {IndexColumns}, {nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(ConstraintName)}: {ConstraintName}, {nameof(IndexName)}: {IndexName}, {nameof(ReferencedTable)}: {ReferencedTable}";
        }
    }
}
