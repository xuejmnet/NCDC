using System;
using System.Collections.Generic;
using System.Text;
using NCDC.CommandParser.Common.Segment.DDL.Column.Position;

namespace NCDC.CommandParser.Common.Segment.DDL.Column.Alter
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 16:57:12
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ModifyColumnDefinitionSegment:IAlterDefinitionSegment
    {
        public int StartIndex { get; }
        public int StopIndex { get; }
        public ColumnDefinitionSegment ColumnDefinition { get; }
        public ColumnPositionSegment? ColumnPosition { get; set; }

        public ModifyColumnDefinitionSegment(int startIndex, int stopIndex, ColumnDefinitionSegment columnDefinition)
        {
            StartIndex = startIndex;
            StopIndex = stopIndex;
            ColumnDefinition = columnDefinition;
        }

        public override string ToString()
        {
            return $"{nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(ColumnDefinition)}: {ColumnDefinition}, {nameof(ColumnPosition)}: {ColumnPosition}";
        }
    }
}
