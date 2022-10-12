using System;
using System.Collections.Generic;
using System.Text;
using NCDC.CommandParser.Segment.DML.Column;
using NCDC.CommandParser.Segment.Generic;
using NCDC.CommandParser.Segment.Generic.Table;

namespace NCDC.CommandParser.Segment.DDL.Column
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 16:46:14
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ColumnDefinitionSegment : ICreateDefinitionSegment
    {
        public int StartIndex { get; }
        public int StopIndex { get; }
        public ColumnSegment ColumnName { get; set; }

        public DataTypeSegment DataType { get; set; }

        public bool PrimaryKey { get; set; }
        public bool NotNull { get; }

        public readonly ICollection<SimpleTableSegment> ReferencedTables = new LinkedList<SimpleTableSegment>();

        public ColumnDefinitionSegment(int startIndex, int stopIndex, ColumnSegment columnName,
            DataTypeSegment dataType, bool primaryKey, bool notNull)
        {
            StartIndex = startIndex;
            StopIndex = stopIndex;
            ColumnName = columnName;
            DataType = dataType;
            PrimaryKey = primaryKey;
            NotNull = notNull;
        }

        public override string ToString()
        {
            return $"{nameof(ReferencedTables)}: {ReferencedTables}, {nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(ColumnName)}: {ColumnName}, {nameof(DataType)}: {DataType}, {nameof(PrimaryKey)}: {PrimaryKey}, {nameof(NotNull)}: {NotNull}";
        }
    }
}