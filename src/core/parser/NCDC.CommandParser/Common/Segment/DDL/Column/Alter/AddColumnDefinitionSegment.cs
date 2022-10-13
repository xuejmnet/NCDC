using System;
using System.Collections.Generic;
using System.Text;
using NCDC.CommandParser.Common.Segment.DDL.Column.Position;

namespace NCDC.CommandParser.Common.Segment.DDL.Column.Alter
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 16:42:11
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class AddColumnDefinitionSegment: IAlterDefinitionSegment
    {
        public AddColumnDefinitionSegment(int startIndex, int stopIndex, ICollection<ColumnDefinitionSegment> columnDefinitions)
        {
            StartIndex = startIndex;
            StopIndex = stopIndex;
            ColumnDefinitions = columnDefinitions;
        }

        public int StartIndex { get; }
        public int StopIndex { get; }
        public  ICollection<ColumnDefinitionSegment> ColumnDefinitions { get; }

        public ColumnPositionSegment? ColumnPosition { get; set; }

        public override string ToString()
        {
            return $"{nameof(StartIndex)}: {StartIndex}, {nameof(StopIndex)}: {StopIndex}, {nameof(ColumnDefinitions)}: {ColumnDefinitions}, {nameof(ColumnPosition)}: {ColumnPosition}";
        }
    }
}
