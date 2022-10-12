using System;
using System.Collections.Generic;
using System.Text;
using NCDC.CommandParser.Segment.DML.Column;

namespace NCDC.CommandParser.Segment.DDL.Column.Position
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/20 16:58:11
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public abstract class ColumnPositionSegment:ISqlSegment,IComparable<ColumnPositionSegment>
    {
        public int StartIndex { get; }
        public int StopIndex { get; }
        public  ColumnSegment ColumnName { get; }

        public ColumnPositionSegment(int startIndex, int stopIndex, ColumnSegment columnName)
        {
            StartIndex = startIndex;
            StopIndex = stopIndex;
            ColumnName = columnName;
        }

        public int CompareTo(ColumnPositionSegment? other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));
            return StartIndex - other.StartIndex;
        }
    }
}
