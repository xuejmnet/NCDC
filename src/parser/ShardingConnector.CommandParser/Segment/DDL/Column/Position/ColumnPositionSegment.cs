using System;
using System.Collections.Generic;
using System.Text;
using ShardingConnector.CommandParser.Segment.DML.Column;

namespace ShardingConnector.CommandParser.Segment.DDL.Column.Position
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
        private readonly int _startIndex;

        private readonly int _stopIndex;

        public  ColumnSegment ColumnName { get; }

        public ColumnPositionSegment(int startIndex, int stopIndex, ColumnSegment columnName)
        {
            this._startIndex = startIndex;
            this._stopIndex = stopIndex;
            ColumnName = columnName;
        }
        public int GetStartIndex()
        {
            return _startIndex;
        }

        public int GetStopIndex()
        {
            return _stopIndex;
        }

        public int CompareTo(ColumnPositionSegment other)
        {
            return _startIndex - other._startIndex;
        }
    }
}
