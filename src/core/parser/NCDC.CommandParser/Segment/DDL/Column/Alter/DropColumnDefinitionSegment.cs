using System;
using System.Collections.Generic;
using System.Text;
using NCDC.CommandParser.Segment.DML.Column;

namespace NCDC.CommandParser.Segment.DDL.Column.Alter
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 16:55:39
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class DropColumnDefinitionSegment:IAlterDefinitionSegment
    {
        public int StartIndex { get; }
        public int StopIndex { get; }
        public ICollection<ColumnSegment> Columns { get; }

        public DropColumnDefinitionSegment(int startIndex, int stopIndex, ICollection<ColumnSegment> columns)
        {
            StartIndex = startIndex;
            StopIndex = stopIndex;
            Columns = columns;
        }

        public override string ToString()
        {
            return $"{nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(Columns)}: {Columns}";
        }
    }
}
